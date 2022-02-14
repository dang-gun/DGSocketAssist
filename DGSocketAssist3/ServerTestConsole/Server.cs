using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DGSocketAssist3_Server
{
	/// <summary>
	/// 클라이언트의 접속을 기다리는 서버.<br />
	/// 소켓 서버에 대한 연결 논리를 구현합니다.
	/// <para>연결을 수락한 후 클라이언트에서 읽은 모든 데이터는 클라이언트로 다시 전송됩니다.<br />
	/// 클라이언트가 연결을 끊을 때까지 클라이언트 패턴에 대한 읽기 및 에코백이 계속됩니다.</para>
	/// <see href="https://docs.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketasynceventargs?view=net-6.0">SocketAsyncEventArgs 클래스</see>
	/// </summary>
	public class Server
	{
		/// <summary>
		/// 샘플이 동시에 처리하도록 설계된 최대 연결 수
		/// </summary>
		private int m_numConnections;
		/// <summary>
		/// 각 소켓 I/O 작업에 사용할 버퍼 크기
		/// </summary>
		private int m_receiveBufferSize;
		/// <summary>
		/// 모든 소켓 작업에 대해 재사용 가능한 큰 버퍼 세트를 나타냅니다.
		/// </summary>
		BufferManager m_bufferManager;
		/// <summary>
		/// 읽기, 쓰기(수락을 위해 버퍼 공간을 할당하지 않음)
		/// <para>클라이언트 하나는 받고/보내기 를 위해 최소 2개의 풀을 사용해야 한다.</para>
		/// </summary>
		const int opsToPreAlloc = 2;
		/// <summary>
		/// 들어오는 연결 요청을 수신 대기하는 데 사용되는 소켓
		/// </summary>
		Socket listenSocket;
		/// <summary>
		/// 쓰기, 읽기 및 소켓 작업 수락을 위한 재사용 가능한 SocketAsyncEventArgs 개체 풀
		/// </summary>
		SocketAsyncEventArgsPool m_readWritePool;
		/// <summary>
		/// 서버가 수신한 총 #바이트의 카운터
		/// </summary>
		int m_totalBytesRead;
		/// <summary>
		/// 서버에 연결된 총 클라이언트 수
		/// </summary>
		int m_numConnectedSockets;
		/// <summary>
		/// 한번에 사용될 쓰래드 개수
		/// </summary>
		Semaphore m_maxNumberAcceptedClients;

		/// <summary>
		/// 초기화되지 않은 서버 인스턴스를 생성합니다.
		/// <para>연결 요청을 수신하는 서버를 시작하려면 
		/// Init 메서드를 호출한 다음 Start 메서드를 호출합니다.</para>
		/// </summary>
		/// <param name="numConnections">샘플이 동시에 처리하도록 설계된 최대 연결 수</param>
		/// <param name="receiveBufferSize">각 소켓 I/O 작업에 사용할 버퍼 크기</param>
		public Server(int numConnections, int receiveBufferSize)
		{
			m_totalBytesRead = 0;
			m_numConnectedSockets = 0;
			m_numConnections = numConnections;
			m_receiveBufferSize = receiveBufferSize;
			//최대 소켓 수에 하나의 미결 읽기 및 쓰기가
			//동시에 소켓에 ​​게시될 수 있도록 버퍼를 할당합니다.
			m_bufferManager 
				= new BufferManager(
					m_receiveBufferSize * numConnections * opsToPreAlloc
					, m_receiveBufferSize);

			m_readWritePool = new SocketAsyncEventArgsPool(numConnections);
			
			//한번에 실행될 쓰래드 수는 최대 접속자 만큼으로 제한 한다.
			m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
		}

		/// <summary>
		/// 재사용 가능한 버퍼를 컨텍스트 개체에 미리 할당하여 서버를 초기화합니다.
		/// <para>이러한 개체를 미리 할당하거나 재사용할 필요는 없지만 
		/// API를 사용하여 서버 성능을 향상시키기 위해 
		/// 재사용 가능한 개체를 쉽게 만드는 방법을 설명하기 위해 이렇게 합니다.</para>
		/// </summary>
		public void Init()
		{
			//모든 I/O 작업이 사용하는 하나의 큰 바이트 버퍼를 할당합니다.
			//이것은 메모리 단편화를 방지합니다.
			m_bufferManager.InitBuffer();

			//SocketAsyncEventArgs 개체의 풀을 미리 할당합니다.
			SocketAsyncEventArgs readWriteEventArg;

			for (int i = 0; i < m_numConnections; i++)
			{
				//재사용 가능한 SocketAsyncEventArgs 세트를 미리 할당합니다.
				readWriteEventArg = new SocketAsyncEventArgs();
				readWriteEventArg.Completed 
					+= new EventHandler<SocketAsyncEventArgs>(IO_Completed);
				readWriteEventArg.UserToken = new AsyncUserToken();

				//버퍼 풀의 바이트 버퍼를 SocketAsyncEventArg 개체에 할당합니다.
				m_bufferManager.SetBuffer(readWriteEventArg);

				//SocketAsyncEventArg를 풀에 추가
				m_readWritePool.Push(readWriteEventArg);
			}
		}

		/// <summary>
		/// 들어오는 연결 요청을 수신 대기하도록 서버를 시작합니다.
		/// </summary>
		/// <param name="localEndPoint">서버가 연결 요청을 수신할 끝점</param>
		public void Start(IPEndPoint localEndPoint)
		{
			//들어오는 연결을 수신하는 소켓을 만듭니다.
			listenSocket 
				= new Socket(localEndPoint.AddressFamily
							, SocketType.Stream
							, ProtocolType.Tcp);
			listenSocket.Bind(localEndPoint);
			//100개 연결의 수신 대기 백로그로 서버 시작
			listenSocket.Listen(100);

			//리스닝 소켓에서 포스트 수락
			StartAccept(null);

			//Console.WriteLine("{0}개의 연결 소켓에 하나의 미해결 수신이 게시됨....아무 키나 누르십시오.", m_outstandingReadCount);
			Console.WriteLine("Press any key to terminate the server process....");
			Console.ReadKey();
		}

		/// <summary>
		/// 클라이언트의 연결 요청을 수락하는 작업을 시작합니다.
		/// </summary>
		/// <param name="acceptEventArg">서버의 수신 소켓에서 수락 작업을 실행할 때 
		/// 사용할 컨텍스트 개체</param>
		public void StartAccept(SocketAsyncEventArgs acceptEventArg)
		{
			if (acceptEventArg == null)
			{//첫 
				acceptEventArg = new SocketAsyncEventArgs();
				acceptEventArg.Completed 
					+= new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
			}
			else
			{
				//컨텍스트 객체가 재사용되기 때문에 소켓을 지워야 합니다.
				acceptEventArg.AcceptSocket = null;
			}

			//사용할수 있는 쓰래드가 없다면 대기
			m_maxNumberAcceptedClients.WaitOne();

			bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArg);
			if (!willRaiseEvent)
			{
				ProcessAccept(acceptEventArg);
			}
		}

		/// <summary>
		/// 이 메서드는 Socket.AcceptAsync 작업과 연결된 콜백 메서드이며 
		/// 수락 작업이 완료되면 호출됩니다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
		{
			ProcessAccept(e);
		}

		/// <summary>
		/// 유저가 접속 되었다.
		/// </summary>
		/// <param name="e"></param>
		private void ProcessAccept(SocketAsyncEventArgs e)
		{
			Interlocked.Increment(ref m_numConnectedSockets);
			Console.WriteLine("Client connection accepted. There are {0} clients connected to the server",
				m_numConnectedSockets);

			//수락된 클라이언트 연결에 대한 소켓을 가져와
			//ReadEventArg 개체 사용자 토큰에 넣습니다.
			SocketAsyncEventArgs readEventArgs = m_readWritePool.Pop();
			((AsyncUserToken)readEventArgs.UserToken).Socket = e.AcceptSocket;

			//클라이언트가 연결되자마자 연결에 수신을 게시합니다.
			bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);
			if (!willRaiseEvent)
			{
				ProcessReceive(readEventArgs);
			}

			//다음 연결 요청 수락
			StartAccept(e);
		}

		/// <summary>
		/// 이 메소드는 소켓에서 수신 또는 전송 작업이 완료될 때마다 호출됩니다.
		/// <para>작업이 완료되는 기준은 설정된 버퍼(receiveBufferSize)가 꽉차거나
		/// 모든 데이터가 전송되었을 때다.<br />
		/// 만약 버퍼를 4로 설정했는데 10자가 오면 완료이벤트는 3번(4+4+2)온다.</para>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <exception cref="ArgumentException">소켓에서 완료된 마지막 작업은
		/// 수신 또는 전송이 아닙니다.</exception>
		void IO_Completed(object sender, SocketAsyncEventArgs e)
		{
			//방금 완료된 작업 유형을 결정하고 관련 핸들러를 호출합니다.
			switch (e.LastOperation)
			{
				case SocketAsyncOperation.Receive:
					ProcessReceive(e);
					break;
				case SocketAsyncOperation.Send:
					ProcessSend(e);
					break;
				default:
					throw new ArgumentException("The last operation completed on the socket was not a receive or send");
			}
		}

		/// <summary>
		/// 이 메서드는 비동기 수신 작업이 완료될 때 호출됩니다.<br />
		/// 원격 호스트가 연결을 닫으면 소켓이 닫힙니다.<br />
		/// 데이터가 수신되면 데이터가 클라이언트로 다시 에코됩니다.
		/// </summary>
		/// <param name="e"></param>
		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			//원격 호스트가 연결을 닫았는지 확인
			AsyncUserToken token = (AsyncUserToken)e.UserToken;
			if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
			{
				//서버가 수신한 총 바이트 수 증가
				Interlocked.Add(ref m_totalBytesRead, e.BytesTransferred);
				Console.WriteLine("The server has read a total of {0} bytes"
					, m_totalBytesRead);

				
				
				for (int i = 0; i < e.BytesTransferred; ++i)
				{
					Console.WriteLine(e.Buffer[i + e.Offset]);
				}



				Task.Delay(1000)
					.ContinueWith((task) =>
					{
						//받은 데이터를 클라이언트로 다시 에코
						e.SetBuffer(e.Offset, e.BytesTransferred);
						bool willRaiseEvent = token.Socket.SendAsync(e);
						if (!willRaiseEvent)
						{
							ProcessSend(e);
						}
					});

				
			}
			else
			{
				CloseClientSocket(e);
			}
		}


		/// <summary>
		/// 이 메서드는 비동기 전송 작업이 완료될 때 호출됩니다.<br />
		/// 메소드는 소켓에서 다른 수신을 발행하여 추가 정보를 읽습니다.<br />
		/// 클라이언트에서 보낸 데이터
		/// </summary>
		/// <param name="e"></param>
		private void ProcessSend( SocketAsyncEventArgs e)
		{
			if (e.SocketError == SocketError.Success)
			{
				//클라이언트에 데이터를 다시 반향하는 완료
				AsyncUserToken token = (AsyncUserToken)e.UserToken;
				//클라이언트가 보낸 다음 데이터 블록을 읽습니다.
				bool willRaiseEvent = token.Socket.ReceiveAsync(e);

				if (!willRaiseEvent)
				{
					ProcessReceive(e);
				}
			}
			else
			{
				CloseClientSocket(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		private void CloseClientSocket(SocketAsyncEventArgs e)
		{
			AsyncUserToken token = e.UserToken as AsyncUserToken;

			//클라이언트와 연결된 소켓을 닫습니다
			try
			{
				token.Socket.Shutdown(SocketShutdown.Send);
			}
			catch (Exception)
			{//클라이언트 프로세스가 이미 닫힌 경우 throw
			}
			token.Socket.Close();

			//서버에 연결된 총 클라이언트 수를 추적하면서 카운터를 감소시킵니다.
			Interlocked.Decrement(ref m_numConnectedSockets);

			//다른 클라이언트에서 재사용할 수 있도록 SocketAsyncEventArg를 해제합니다.
			m_readWritePool.Push(e);

			//사용이 끝난 쓰래드 반환
			m_maxNumberAcceptedClients.Release();

			//서버에서 클라이언트 연결이 끊어졌습니다. 서버에 연결된 클라이언트가 {0}개 있습니다."
			Console.WriteLine("A client has been disconnected from the server. There are {0} clients connected to the server", m_numConnectedSockets);
		}

	}
}
