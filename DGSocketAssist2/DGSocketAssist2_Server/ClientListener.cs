using DGSocketAssist2_Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DGSocketAssist2_Server
{
	/// <summary>
	/// 연결된 클라이언트
	/// </summary>
	public class ClientListener
	{
		#region 클라이언트 이벤트
		/// <summary>
		/// 무결성 검사 완료
		/// </summary>
		/// <param name="sender"></param>
		public delegate void ValidationCompleteDelegate(ClientListener sender);
		/// <summary>
		/// 무결성 검사가 끝남.
		/// 로그인 처리와 같은 무결성 검사가 끝나서 서버에 진입한(혹은 가능한)
		/// 상태가 되면 발생한다.
		/// </summary>
		public event ValidationCompleteDelegate OnValidationComplete;
		/// <summary>
		/// 무결성 검사 완료되었음을 외부에 알린다.
		/// </summary>
		private void ValidationCompleteCall()
		{
			if (null != OnValidationComplete)
			{
				this.OnValidationComplete(this);
			}
		}

		/// <summary>
		/// 클라이언트 끊김 처리가 시작되었음을 알린다.
		/// </summary>
		/// <param name="sender"></param>
		public delegate void DisconnectDelegate(ClientListener sender);
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
		public delegate void DisconnectCompletedDelegate(ClientListener sender);
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
		/// 메시지가 수신
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		public delegate void MessagedDelegate(ClientListener sender, byte[] byteMessage);
		/// <summary>
		/// 메시지가 수신됨
		/// </summary>
		public event MessagedDelegate OnMessaged;
		/// <summary>
		/// 메시지가 수신되었음을을 외부에 알림
		/// </summary>
		private void MessagedCall(byte[] byteMessage)
		{
			if (null != OnMessaged)
			{
				this.OnMessaged(this, byteMessage);
			}
		}
		#endregion

		/// <summary>
		/// 이 클라이언트가 연결된 Socket
		/// </summary>
		public Socket SocketMe { get; private set; }

		/// <summary>
		/// 이 클라이언트에게 전송용 SocketAsyncEventArgs
		/// </summary>
		private SocketAsyncEventArgs m_saeaSend = null;
		/// <summary>
		/// 이 클라이언트에게서오는 수신용 SocketAsyncEventArgs
		/// </summary>
		private SocketAsyncEventArgs m_saeaReceive = null;

		#region 클라이언트 유효성(validation) 검사용 함수 정의
		/// <summary>
		/// 유효성 검사에 사용할 함수를 전달하기위한 대리자.
		/// </summary>
		/// <param name="sender">검사를 요청한 클라이언트</param>
		/// <returns></returns>
		public delegate bool ValidationDelegate(ClientListener sender);
		/// <summary>
		/// 유효성 검사에 사용할 함수
		/// </summary>
		private ValidationDelegate m_ValidationFunc = null;
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="socketMe">전달받은 Socket</param>
		public ClientListener(Socket socketMe)
		{
			//소캣을 저장한다.
			this.SocketMe = socketMe;

			//전송용 SocketAsyncEventArgs 세팅
			this.m_saeaSend = new SocketAsyncEventArgs();
			this.m_saeaSend.Completed -= SaeaSend_Completed;
			this.m_saeaSend.Completed += SaeaSend_Completed;

			//수신용 SocketAsyncEventArgs 세팅
			this.m_saeaReceive = new SocketAsyncEventArgs();
			this.m_saeaReceive.SetBuffer(new Byte[SettingData.BufferFullSize], 0, SettingData.BufferFullSize);
			this.m_saeaReceive.Completed -= SaeaReceive_Completed;
			this.m_saeaReceive.Completed += SaeaReceive_Completed;

			//여기서 바로 Listening을 시작하면 이벤트가 연결되기 전에 동작이 진행될수 있다.
		}

		/// <summary>
		/// 연결된 클라이언트에서 전송한 데이터를 읽기위해 대기한다.
		/// <para>모든 이벤트 연결이 끝난 후 호출하는 것이 좋다.</para>
		/// </summary>
		public void FirstListening()
		{
			//데이터 구조 생성
			BufferData bdMsgData = new BufferData();
			//리시브용 데이터 구조 지정
			this.m_saeaReceive.SetBuffer(bdMsgData.Buffer, 0, bdMsgData.Length);
			Debug.WriteLine("첫 데이터 받기 준비");
			//첫 데이터 받기 시작
			this.SocketMe.ReceiveAsync(this.m_saeaReceive);


			if (null != m_ValidationFunc)
			{//유효성 검사용 함수가 있다.
				if (false == m_ValidationFunc(this))
				{//유효성 검사 실패
					//접속을 끊는다.
					this.Disconnect(true);
					return;
				}
			}
			//유효성 검사 함수가 없다면 검사를 하지 않는다.

			//외부에 접속허가를 알림
			this.ValidationCompleteCall();
		}

		/// <summary>
		/// 클라리언트에서 넘어온 데이터 받음 완료
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaeaReceive_Completed(object sender, SocketAsyncEventArgs e)
		{
			//서버에서 넘어온 정보
			Socket socketClient = (Socket)sender;

			//유저가 연결 상태인지?
			if (true == socketClient.Connected)
			{//연결 상태이다

				//수신된 데이터 복사
				//이 프로젝트에서는 버퍼가 찰때까지 기다리는 처리를 하지 않는다.
				//이 때문에 완성되지 않은 버퍼가 들어올 가능성과
				//SocketAsyncEventArgs.Completed가 여러번 오는것을 대비할수 없다.
				BufferData bdMsgData = new BufferData(e.Buffer, true);

				
				//헤더를 자른다.
				bdMsgData.CutHeader();
				bdMsgData.CutBody();

				//넘어온 메시지 전달
				this.MessagedCall(bdMsgData.Buffer);
				Debug.WriteLine("전달된 데이터 : {0}", bdMsgData.Buffer);

				try
				{
					//다음 데이터를 기다린다.
					//'Read'에서 무한루프 없이 구현하기 위해 두번째부터는 여기서 대기하도록
					//구성되어 있다.
					socketClient.ReceiveAsync(e);
					Debug.WriteLine("데이터 받기 준비");
				}
				catch (Exception ex)
				{//유저가 중간에 끊어지면 여기서 에러가 날가능성이 있다.
					Debug.WriteLine("error SaeaReceive_Completed : " + ex.Message);
				}
			}
			else
			{//아니다
				//접속 끊김을 알린다.
				Disconnect(true);
			}
		}

		/// <summary>
		/// 연결된 이 클라이언트에게 메시지를 전송 한다.
		/// </summary>
		/// <param name="sMsg"></param>
		public void Send(byte[] byteMessage)
		{
			BufferData bdMsg = new BufferData(byteMessage, false);
			//헤더 붙이기
			bdMsg.AddHeader();

			//데이터 길이 세팅
			this.m_saeaSend.SetBuffer(bdMsg.Buffer, 0, bdMsg.Length);
			//보낼 데이터 설정
			Debug.WriteLine("데이터 전달 : {0}", bdMsg.Buffer);
			//보내기
			this.SocketMe.SendAsync(this.m_saeaSend);
		}

		private void SaeaSend_Completed(object sender, SocketAsyncEventArgs e)
		{
			try
			{
				
			}
			catch
			{
				//보통은 클라이언트가 끊기면 여기서 에러가 난다.
			}
		}

		/// <summary>
		/// 연결을 끊는다.
		/// <para>이미 끊는 이벤트가 발생했는데 bEvent를 true로 사용하는 경우
		/// 무한루프에 빠질수 있으니 조심해야 한다.</para>
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
		/// 외부용 - 이벤트 발생 안함.
		/// </summary>
		public void Disconnect()
		{
			this.Disconnect(false);
		}

	}
}
