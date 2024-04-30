using System;
using System.Net.Sockets;

using DG_SocketAssist4.Global;
using DG_SocketAssist4.Global.Faculty;
using DG_SocketAssist4.Global.ReceiveAssists;
using DG_SocketAssist4.Global.SendAssists;

namespace DG_SocketAssist4.Server
{
    /// <summary>
    /// 연결된 클라이언트
    /// <para>직접적인 서버 연결과 관련된 처리만 한다.</para>
    /// <para>ClientModel와 1:1 매칭된다.</para>
    /// </summary>
    /// <remarks>
    /// ClientListener를 외부에 노출시키기 않기 위해 ClientModel를 사용해야 한다.
    /// <para>내부에서는 ClientListener를 사용하고 외부에서는 ClientModel를 사용한 것이 컨샙이다.</para>
    /// <para>이 솔류션에서는 한 클라이언트는 2개의 SocketAsyncEventArgs를 가지는 것을 원칙으로 한다.</para>
    /// <para>스래드 풀 사용을 가정하지 않으므로 SocketAsyncEventArgs.Completed의 sender는 
    /// 항상 SocketMe와 동일하다고 가정한다.</para>
    /// <para>만약 스래드 풀을 사용할 생각이면 SocketAsyncEventArgs를 해제할때
    /// 이벤트관리를 철저하게 하여 sender가 다른 클라이언트가 들어오지 않도록 해야 한다.</para>
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
		private void OnDisconnectCall()
		{
			this.DisconnectEventIs = true;

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
		/// <param name="byteData"></param>
		public delegate void MessagedDelegate(ClientListener sender, byte[] byteData);
		/// <summary>
		/// 메시지가 수신됨
		/// </summary>
		public event MessagedDelegate OnMessaged;
		/// <summary>
		/// 메시지가 수신되었음을을 외부에 알림
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
        /// 연결된 SocketMe의 IP
        /// </summary>
        public string Ip
        {
            get
            {
                return (new SocketInfo()).ToIp(this.SocketMe);
            }
        }

        /// <summary>
        /// 이 클라이언트에게 전송용 SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs m_saeaSend = null;
        /// <summary>
        /// 서버 전송시 m_saeaSend가 사용중일때 처리해주는 큐
        /// </summary>
        private SendAssist m_SendAssist = new SendAssist();

        /// <summary>
        /// 이 클라이언트에게서오는 수신용 SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs m_saeaReceive = null;
		/// <summary>
		/// 리시브 데이터 임시보관할 버퍼
		/// </summary>
        private ReceiveAssist m_ReceiveBuffer = new ReceiveAssist();


        /// <summary>
        /// 클라이언트 끊어짐 이벤트가 발생했었는지 여부
        /// </summary>
        /// <remarks>
        /// 이 클래스는 클라이언트가 끊어지면 제거되므로 OnDisconnect이벤트는 한번만 발생해야 한다.
		/// <para>OnDisconnect이벤트가 여러번 발생하면 외부에서는 제거된 개체를 제거하려는 문제가 생길 수 있다.</para>
		/// <para>이 문제를 방지하기위한 변수로 OnDisconnect가 한번이라도 발생하면 true로 변경된다. </para>
        /// </remarks>
        private bool DisconnectEventIs = false;

		/// <summary>
		/// 이 개체를 구분하기위한 고유번호
		/// <para>외부에서 이 개체를 구분하기위한 인덱스</para>
		/// </summary>
		/// <remarks>
		/// 이 라이브러리에서는 여기에 값을 부여하지 않는다.
		/// <para>이 클래스를 관리하는 쪽에서 필요에 따라 인덱스를 부여해야 한다.</para>
		/// </remarks>
        public long ClientIndex { get; set; } = 0;

		/// <summary>
		/// 연결 확인 개체
		/// </summary>
		//private KeepAliveChecker ConnectChecker;


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
            BufferDataModel mdReceiveMsg = new BufferDataModel();

            //서버에 수신대기할 개체를 설정한다.
            //보낼 데이터를 설정하고
            this.m_saeaReceive.SetBuffer(mdReceiveMsg.Buffer, 0, mdReceiveMsg.Length);

            //첫 데이터 받기 시작
            if (false == this.SocketMe.ReceiveAsync(this.m_saeaReceive))
			{
                this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
            }
            this.OnLogCall(0, "첫 데이터 받기 준비");
        }

		/// <summary>
		/// 클라리언트에서 넘어온 데이터 받음 완료
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaeaReceive_Completed(object sender, SocketAsyncEventArgs e)
		{
            //다른 클라이언트의 소켓이 올 가능성이 있다면 아래와 같은 방법으로
            //다른 클라이언트의 소켓을 처리해야 한다.
            //Socket socketClient = (Socket)sender;


			if (true == this.SocketMe.Connected)
			{//연결 상태이다

                //버퍼에서 데이터가 완성되었는지 확인한다.
                byte[] byteReceiveCompleteData
                    = this.m_ReceiveBuffer.ReceiveDataCheck(e);

                if (0 < byteReceiveCompleteData.Length)
                {//완성된 데이터가 있다.

                    //수신된 버퍼 만큼 확보
                    BufferDataModel bdRecieveMsg
                        = new BufferDataModel(byteReceiveCompleteData);
                    //메시지 수신을 알림
                    this.MessagedCall(bdRecieveMsg.Buffer);
                }

                //다음 데이터를 기다린다.
                this.OnLogCall(0, "다음 데이터 받기 준비");
                //'Read'에서 무한루프 없이 구현하기 위해 두번째부터는 여기서 대기하도록 구성되어 있다.
                //다음 메시지를 받을 준비를 한다.
                if (false == this.SocketMe.ReceiveAsync(e))
                {
                    this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
                }
            }
			else
			{//아니다

                //연결이 끊김이 감지되면 리시브가 전달되게 된다.
                //문제는 끊김이 일어났을때 100% 전달되지는 않는다.
				//(상황에 따라서 끊어졌는데도 안오는 경우가 있음.)

                //접속 끊김을 알린다.
                this.Disconnect();
			}
		}

		/// <summary>
		/// 연결된 이 클라이언트에게 메시지를 전송 한다.
		/// </summary>
		/// <param name="byteData"></param>
		public void Send(byte[] byteData)
		{
            this.m_SendAssist.SendCheck(byteData
                , delegate (byte[] byteMsg_Send)
                {
                    //버퍼 데이터를 만들고
                    BufferDataModel bdSendMsg = new BufferDataModel(byteMsg_Send);

                    this.OnLogCall(0, "Send : 메시지 보내기 시작 : " + byteMsg_Send);
                    //데이터 넣기
                    this.m_saeaSend.SetBuffer(bdSendMsg.Buffer, 0, bdSendMsg.Length);
                    //보내기 시작
                    if (false == this.SocketMe.SendAsync(this.m_saeaSend))
                    {
                        this.SaeaSend_Completed(this.SocketMe, this.m_saeaSend);
                    }
                });
        }

		/// <summary>
		/// 보내기 완료 이벤트
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaeaSend_Completed(object sender, SocketAsyncEventArgs e)
		{
            this.OnLogCall(0, "SaeaSend_Completed : 메시지 보내기 완료");


            //큐 사용이 끝남을 알림
            this.m_SendAssist.SendCheckCompleted();
            //보낸게 완료되었으니 다음 큐를 진행 시킨다.
            this.Send(new byte[0]);
        }

		/// <summary>
		/// 연결을 끊는다.
		/// </summary>
		public void Disconnect()
		{
			if (false == this.DisconnectEventIs)
			{
				this.OnDisconnectCall();
			}

			if (null != this.SocketMe)
			{
				this.SocketMe.Close();
				this.SocketMe.Dispose();
                this.SocketMe = null;
			}

			if (false == this.DisconnectEventIs)
			{
				this.DisconnectCompletedCall();
			}
		}


	}
}
