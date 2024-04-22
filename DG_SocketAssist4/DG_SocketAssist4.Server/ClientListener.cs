using DG_SocketAssist4.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DG_SocketAssist4.Server
{
    /// <summary>
    /// 연결된 클라이언트
    /// <para>직접적인 서버 연결과 관련된 처리만 한다.</para>
	/// <para>ClientModel와 1:1 매칭된다.</para>
    /// </summary>
	/// <remarks>
	/// ClientListener를 외부에 노출시키기 않기 위해 ClientModel를 사용해야 한다.
	/// <para>
	/// 내부에서는 ClientListener를 사용하고 외부에서는 ClientModel를 사용한 것이 컨샙이다.
	/// </para>
	/// </remarks>
    internal class ClientListener
	{
        #region 외부로 알릴 이벤트
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
		/// 메시지 수신
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		public delegate void MessagedDelegate(ClientListener sender, string message);
		/// <summary>
		/// 메시지가 수신됨
		/// </summary>
		public event MessagedDelegate OnMessaged;
		/// <summary>
		/// 메시지가 수신되었음을을 외부에 알림
		/// </summary>
		private void MessagedCall(string message)
		{
			if (null != OnMessaged)
			{
				this.OnMessaged(this, message);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketMe">전달받은 Socket</param>
        internal ClientListener(Socket socketMe)
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
            //외부에서 원하는 타이밍에 FirstListening를 호출해야 한다.
        }

        /// <summary>
        /// 연결된 클라이언트에서 전송한 첫 데이터를 읽기위해 대기한다.
        /// </summary>
        /// <remarks>
        /// 모든 이벤트 연결이 끝난 후 호출하는 것이 좋다.
        /// </remarks>
        public void FirstListening()
		{
			//데이터 구조 생성
			MessageData MsgData = new MessageData();
			//커낵트용 데이터 구조 지정
			this.m_saeaReceive.UserToken = MsgData;
			
			this.OnLogCall(0, "첫 데이터 받기 준비");
            //첫 데이터 받기 시작
            this.SocketMe.ReceiveAsync(this.m_saeaReceive);
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
			//서버에서 넘어온 데이터
			MessageData MsgData = (MessageData)e.UserToken;
			MsgData.SetLength(e.Buffer);
			MsgData.InitData();

			//유저가 연결 상태인지?
			if (true == socketClient.Connected)
			{//연결 상태이다

				//넘어온 메시지 읽기
				socketClient.Receive(MsgData.Data, MsgData.DataLength, SocketFlags.None);
				//넘어온 메시지 전달
				this.MessagedCall(MsgData.GetData());
                this.OnLogCall(0, string.Format("전달된 데이터 : {0}", MsgData.GetData()));

                //다음 데이터를 기다린다.
                //'Read'에서 무한루프 없이 구현하기 위해 두번째부터는 여기서 대기하도록
                //구성되어 있다.
                socketClient.ReceiveAsync(e);
                this.OnLogCall(0, "데이터 받기 준비");
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
		public void Send(string sMsg)
		{
			MessageData mdMsg = new MessageData();
			mdMsg.SetData(sMsg);

			//데이터 길이 세팅
			this.m_saeaSend.SetBuffer(BitConverter.GetBytes(mdMsg.DataLength), 0, 4);
			//보낼 데이터 설정
			this.m_saeaSend.UserToken = mdMsg;

            this.OnLogCall(0, string.Format("데이터 전달 : {0}", sMsg));
            //보내기
            this.SocketMe.SendAsync(this.m_saeaSend);
		}

		private void SaeaSend_Completed(object sender, SocketAsyncEventArgs e)
		{
			//유저 소켓
			Socket socketClient = (Socket)sender;
			MessageData mdMsg = (MessageData)e.UserToken;
			//데이터 보내기 마무리
			socketClient.Send(mdMsg.Data);
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
