using DG_SocketAssist4.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DG_SocketAssist4.Client
{
    /// <summary>
    /// 서버연결에 사용하는 클라이언트.
	/// <para>클라이언트 입장에서 서버에 접속하기위한 클라이언트</para>
    /// </summary>
    public class ClientSocket
	{
        #region 클라이언트 이벤트
        /// <summary>
        /// 로그용 대리자
        /// </summary>
        /// <param name="nLogType">로그 성격</param>
        /// <param name="sMessage"></param>
        public delegate void LogDelegate(int nLogType, string sMessage);
        /// <summary>
        /// 로그 발생 이벤트
        /// </summary>
        public event LogDelegate OnLog;
        /// <summary>
        /// 로그 발생 이벤트 호출
        /// </summary>
        /// <param name="nLogType"></param>
        /// <param name="sMessage"></param>
        private void OnLogCall(int nLogType, string sMessage)
        {
            if (null != this.OnLog)
            {
                this.OnLog(nLogType, sMessage);
            }
        }

        /// <summary>
        /// 서버 접속 완료
        /// </summary>
        /// <param name="sender"></param>
        public delegate void ConnectionCompleteDelegate(ClientSocket sender);
		/// <summary>
		/// 서버 접속 완료
		/// </summary>
		/// <remarks>
		/// 로그인 처리와 같은 무결성 검사가 끝나서 서버에 진입한(혹은 가능한) 
		/// 상태가 되면 발생한다.
		/// </remarks>
		public event ConnectionCompleteDelegate OnConnectionComplete;
		/// <summary>
		/// 서버 접속 완료를 외부에 알림
		/// </summary>
		private void ConnectionCompleteCall()
		{
			if (null != OnConnectionComplete)
			{
				this.OnConnectionComplete(this);
			}
		}

		/// <summary>
		/// 클라이언트 끊김 처리가 시작되었음을 알린다.
		/// </summary>
		/// <param name="sender"></param>
		public delegate void DisconnectDelegate(ClientSocket sender);
		/// <summary>
		/// 클라이언 끊김 처리가 시작되었음을 알린다.
		/// <para>클라이언트가 어떤 사유에서든 끊겼음을 의미한다.</para>
		/// <para>정상 종료라면 서버에서 먼저 메시지를 보내 직접 끊는 것이 좋다.</para>
		/// </summary>
		public event DisconnectDelegate OnDisconnect;
		/// <summary>
		/// 클라이언트 끊김 처리가 시작되었음을 외부에 알림
		/// </summary>
		private void DisconnectCall()
		{
			if (null != OnDisconnect)
			{
				this.OnDisconnect(this);
			}
		}

		/// <summary>
		/// 클라이언트 끊김 처리 완료
		/// </summary>
		/// <param name="sender"></param>
		public delegate void DisconnectCompletedDelegate(ClientSocket sender);
		/// <summary>
		/// 클라이언트가 끊김처리가 완료 되었다.
		/// </summary>
		public event DisconnectCompletedDelegate OnDisconnectCompleted;
		/// <summary>
		/// 클라이언트 끊김 처리 완료되었음을 외부에 알림
		/// </summary>
		private void DisconnectCompletedCall()
		{
			if (null != OnDisconnectCompleted)
			{
				this.OnDisconnectCompleted(this);
			}
		}

		/// <summary>
		/// 데이터 수신 준비 완료
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		public delegate void ReceiveReadyDelegate(ClientSocket sender);
		/// <summary>
		/// 데이터 수신 준비 완료.
		/// </summary>
		/// <remarks>
		/// <para>서버와 정상 연결되고 메시지 수신이 준비되면 발생한다.</para>
		/// <para>서버의 메시지를 한번 다받고 나면 다시 수신 준비상태가 되므로.</para>
		/// <para>이때도 발생한다.</para>
		/// </remarks>
		public event ReceiveReadyDelegate OnReceiveReady;
		/// <summary>
		/// 데이터 수신 준비 완료되었음을 외부에 알림
		/// </summary>
		private void ReceiveReadyCall()
		{
			if (null != OnReceiveReady)
			{
				this.OnReceiveReady(this);
			}
		}

		/// <summary>
		/// 메시지가 수신
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="byteData"></param>
		public delegate void MessagedDelegate(ClientSocket sender, byte[] byteData);
		/// <summary>
		/// 메시지가 수신됨
		/// </summary>
		public event MessagedDelegate OnMessaged;
		/// <summary>
		/// 메시지가 수신 되었음을 외부에 알림
		/// </summary>
		private void MessagedCall(byte[] byteData)
		{
			if (null != OnMessaged)
			{
				this.OnMessaged(this, byteData);
			}
		}
		#endregion

		/// <summary>
		/// 이 클라이언트가 연결된 Socket
		/// </summary>
		public Socket SocketMe { get; private set; }

		/// <summary>
		/// 서버로 전송용 SocketAsyncEventArgs
		/// </summary>
		private SocketAsyncEventArgs m_saeaSend = null;
        /// <summary>
        /// 서버 전송시 m_saeaSend가 사용중일때 처리해주는 큐
        /// </summary>
        private SendQueue m_SendQueue = new SendQueue();

        /// <summary>
        /// 수신용 SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs m_saeaReceive = null;
        /// <summary>
        /// 리시브 데이터 임시보관할 버퍼
        /// </summary>
        private ReceiveBufferTemp m_ReceiveBuffer = new ReceiveBufferTemp();

        /// <summary>
        /// 바이트 처리 도우미
        /// </summary>
        private BtyeAssist BtyeAssist = new BtyeAssist();

        /// <summary>
        /// 서버 주소
        /// </summary>
        public IPEndPoint ServerIP { get; private set; }

		/// <summary>
		/// 서버와 연결할 클라이언트 생성. 
		/// </summary>
		/// <param name="sIP">서버 ip</param>
		/// <param name="nPort">서버 포트</param>
		public ClientSocket(string sIP, int nPort)
		{
			this.SocketSetting(
				new IPEndPoint(
					IPAddress.Parse(sIP)
					, nPort));
		}
		/// <summary>
		/// 서버와 연결할 클라이언트 생성. 
		/// </summary>
		/// <param name="address">서버 주소</param>
		public ClientSocket(IPEndPoint address)
		{
			this.SocketSetting(address);
		}

		/// <summary>
		/// 이 개체가 사용할 소켓을 생성한다.
		/// </summary>
		/// <param name="ip">서버 주소</param>
		private void SocketSetting(IPEndPoint ip)
		{
			//소켓 생성
			SocketMe 
				= new Socket(AddressFamily.InterNetwork
					, SocketType.Stream
					, ProtocolType.Tcp);
			this.ServerIP = ip;

            //KeepAlive 설정
            //닷넷을 통한 KeepAlive설정은 .NET Core 3.0이상에서만 지원한다.
            //https://learn.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketoptionname?view=netcore-3.0#system-net-sockets-socketoptionname-tcpkeepalivetime
            //윈도우의 경우 IOControl를 통해서 적용할 수 있다.
            //this.socketServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            //this.socketServer.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 10000);
            //this.socketServer.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 5000);
            // Windows 10 version 1703 or later
            //https://learn.microsoft.com/en-us/windows/win32/winsock/ipproto-tcp-socket-options?WT.mc_id=DT-MVP-4038148
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            //    && Environment.OSVersion.Version >= new Version(10, 0, 15063))
            //{//윈도우10, 1703 이후 버전(windows server 2019)
            //    this.socketServer.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);
            //}

            //전송용 SocketAsyncEventArgs 세팅
            this.m_saeaSend = new SocketAsyncEventArgs();
			this.m_saeaSend.RemoteEndPoint = this.ServerIP;
			this.m_saeaSend.Completed -= SaeaSend_Completed;
			this.m_saeaSend.Completed += SaeaSend_Completed;

			//수신용 SocketAsyncEventArgs 세팅
			this.m_saeaReceive = new SocketAsyncEventArgs();
			this.m_saeaReceive.RemoteEndPoint = this.ServerIP;
			this.m_saeaReceive.SetBuffer(new Byte[SettingData.BufferFullSize], 0, SettingData.BufferFullSize);
			this.m_saeaReceive.Completed -= SaeaReceive_Completed;
			this.m_saeaReceive.Completed += SaeaReceive_Completed;
		}

		/// <summary>
		/// 서버에 연결시도를 한다.
		/// </summary>
		public void ConnectServer()
		{
			//접속용 SocketAsyncEventArgs를 생성
			SocketAsyncEventArgs saeaConnect = new SocketAsyncEventArgs();
			saeaConnect.RemoteEndPoint = this.ServerIP;
			//연결 완료 이벤트 연결
			saeaConnect.Completed -= SaeaConnect_Completed;
			saeaConnect.Completed += SaeaConnect_Completed;

            this.OnLogCall(0, "서버 연결 중");
			//서버 메시지 대기
			if (false == this.SocketMe.ConnectAsync(saeaConnect))
			{
				this.SaeaConnect_Completed(this.SocketMe, saeaConnect);
            };
		}

		/// <summary>
		/// 연결 완료 이벤트에 연결됨
		/// <para>서버에 연결되었음에만 사용하는 이벤트이다.</para>
		/// </summary>
		/// <param name="sender">호출한 개체</param>
		/// <param name="e">SocketAsync 이벤트</param>
		private void SaeaConnect_Completed(object sender, SocketAsyncEventArgs e)
		{
			this.SocketMe = (Socket)sender;

			if (true == this.SocketMe.Connected)
			{
                BufferDataModel mdReceiveMsg = new BufferDataModel();

                //서버에 수신대기할 개체를 설정한다.
                //보낼 데이터를 설정하고
                this.m_saeaReceive.SetBuffer(mdReceiveMsg.Buffer, 0, mdReceiveMsg.Length);

                //.NET5 부터는 ReceiveAsync 상황에 따라서 동기/비동기로 돌아간다.
                //이 부분은 별도의 스래드를 만들어 사용했더니 가끔 데이터를 받지 못하는현상이 일어났다.
                //그래서 별도 스래드 처리를 제거하였다.
                //첫 메시지 받기 준비 
                if (false == this.SocketMe.ReceiveAsync(this.m_saeaReceive))
                {
                    this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
                }
                this.ReceiveReadyCall();

                this.OnLogCall(0, "서버 연결 성공");
                //서버 연결 성공을 알림
                this.ConnectionCompleteCall();
			}
			else
			{
				//접속 끊김을 알린다.
				Disconnect(true);
			}
		}


		/// <summary>
		/// 수신 완료 이벤트 연결됨
		/// <para>실제 데이터를 수신받는 이벤트이다.</para>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaeaReceive_Completed(object sender, SocketAsyncEventArgs e)
		{
			Socket socketClient = (Socket)sender;
			

			if (true == socketClient.Connected)
			{//연결이 되어 있다.

                if (1 > e.BytesTransferred)
                {//6바이트 보다 적게 수신했다.

                    //최소 6이상 수신했을때만 동작해야 한다.
                    //운영체제 기준으로 1틱(166666666)동안 대기한다.
                    Thread.Sleep(17);
                }
                if (1 <= e.BytesTransferred)
				{

                    //임시 버퍼에 데이터 추가
                    this.m_ReceiveBuffer.Add(e.Buffer, e.BytesTransferred);

                    byte[] byteReceiveCompleteData 
						= this.m_ReceiveBuffer.FirstSizeData_Int();

                    if (0 < byteReceiveCompleteData.Length)
                    {//완성된 데이터가 있다.

                        //수신된 버퍼 만큼 확보
                        BufferDataModel bdRecieveMsg
                            = new BufferDataModel(byteReceiveCompleteData);
                        //메시지 수신을 알림
                        this.MessagedCall(bdRecieveMsg.Buffer);
                    }
                }


                this.OnLogCall(0, "다음 데이터 받을 준비 ");
                //다음 메시지를 받을 준비를 한다.
                if (false == socketClient.ReceiveAsync(e))
                {
                    this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
                }
                this.ReceiveReadyCall();
			}
			else
			{
				//접속 끊김을 알린다.
				Disconnect(true);
			}
		}


		/// <summary>
		/// 연결된 이 서버로 메시지를 전송 한다.
		/// </summary>
		/// <param name="byteData"></param>
		public void Send(byte[] byteData)
		{
            if (0 < byteData.Length)
            {//전달할 데이터가 있다.

                //데이터에 헤더를 붙이고
                byte[] byteHeader = this.BtyeAssist.SizeAddData(byteData);
                //전송 시도
                this.m_SendQueue.Add(byteHeader);
            }


            //여기서부터는 큐를 가지고 동작한다.
            if (false == this.m_SendQueue.Used
                && 0 < this.m_SendQueue.Count)
			{
                //사용중임을 알리고
                this.m_SendQueue.Used = true;

                //맨 앞에 있는 데이터를 읽는다.
                byte[] sMsg_Send = this.m_SendQueue.Get();

				if (0 < sMsg_Send.Length)
                {//값이 있으면 처리 시작

                    //버퍼 데이터를 만들고
                    BufferDataModel bdSendMsg = new BufferDataModel(sMsg_Send);

                    this.OnLogCall(0, "Send : 메시지 보내기 시작 : " + sMsg_Send);
                    //데이터 넣기
                    this.m_saeaSend.SetBuffer(bdSendMsg.Buffer, 0, bdSendMsg.Length);
                    //보내기 시작
                    if (false == this.SocketMe.SendAsync(this.m_saeaSend))
                    {
                        this.SaeaSend_Completed(this.SocketMe, this.m_saeaSend);
                    }
                }

                
            }

        }

		/// <summary>
		/// 전송 완료 이벤트 연결됨.
		/// <para>'Send'에서 전송이 시작되면 이 곳에서 마무리 한다.</para>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaeaSend_Completed(object sender, SocketAsyncEventArgs e)
		{

            this.OnLogCall(0, "SaeaSend_Completed : 메시지 보내기 완료");

            //큐 사용이 끝남을 알림
            this.m_SendQueue.Used = false;
            //보낸게 완료되었으니 다음 큐를 진행 시킨다.
            this.Send(new byte[0]);
        }

		/// <summary>
		/// 연결을 끊는다.
		/// <para>bEvent를 true로 사용하는 경우 무한루프에 빠질수 있으니 조심해야 한다.</para>
		/// </summary>
		/// <param name="bEvent">연결끊김 이벤트 발생 여부.</param>
		public void Disconnect(bool bEvent)
		{
			if (true == bEvent)
			{
				this.DisconnectCall();
			}

			if (null != this.SocketMe)
			{
				this.SocketMe.Close();
				this.SocketMe = null;
			}

			if (true == bEvent)
			{
				this.DisconnectCompletedCall();
			}
		}

		/// <summary>
		/// 연결을 끊는다.
		/// <para>외부용 - 이벤트 발생 안함.</para>
		/// </summary>
		public void Disconnect()
		{
			this.Disconnect(false);
		}

	}
}
