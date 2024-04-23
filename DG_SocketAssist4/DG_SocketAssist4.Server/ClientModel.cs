using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG_SocketAssist4.Server
{
    /// <summary>
    /// 직접적인 클라이언트 처리를 하는 클래스
    /// <para>서버입장에서 접속된 클라이언트 1개</para>
    /// </summary>
    /// <remarks>
    /// 접속관련 처리는 ServerSocket과 ClientListener에 맞긴다.
    /// <para>이 클래스는 접속 외의 처리를 대리해주는 클래스로 
    /// 주된 목적은 ClientListener를 숨기는데 있다.</para>
    /// </remarks>
    public class ClientModel
    {
        #region 외부에 노출할 이벤트
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
        /// 메시지 수신
        /// </summary>
        /// <param name="sender"></param>
        public delegate void MessagedDelegate(ClientModel sender, string message);
        /// <summary>
        /// 메시지가 수신됨
        /// </summary>
        public event MessagedDelegate OnMessaged;
        /// <summary>
        /// 메시지 수신을 외부에 알림
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void MessagedCall(
            ClientModel sender
            , string message)
        {
            if (null != this.OnMessaged)
            {
                this.OnMessaged(sender, message);
            }
        }
        #endregion

        #region 외부에 노출할 이벤트(internal) - ServerSocket를 통해서 나가야하는 이벤트들

        /// <summary>
        /// 클라이언트 끊김 처리가 시작되었음을 알린다.
        /// </summary>
        /// <param name="sender"></param>
        internal delegate void DisconnectDelegate(ClientModel sender);
        /// <summary>
        /// 클라이언 끊김 처리가 시작되었음을 알린다.
        /// <para>클라이언트가 어떤 사유에서든 끊겼음을 의미한다.</para>
        /// <para>정상 종료라면 서버에서 먼저 메시지를 보내 직접 끊는 것이 좋다.</para>
        /// </summary>
        internal event DisconnectDelegate OnDisconnect;
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
        internal delegate void DisconnectCompletedDelegate(ClientModel sender);
        /// <summary>
        /// 클라이언트가 끊김처리가 완료 되었다.
        /// </summary>
        internal event DisconnectCompletedDelegate OnDisconnectCompleted;
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
        #endregion

        /// <summary>
        /// 연결된 클라이언트 리스너
        /// </summary>
        private ClientListener ClientLis;

        #region 클라이언트 리스너에게 바로 전달할 함수
        
        /// <summary>
        /// 가지고 있는 클라이언트에게 매시지를 보낸다.
        /// </summary>
        /// <param name="sMessage"></param>
        public void Send(string sMessage)
        {
            this.ClientLis.Send(sMessage);
        }

        /// <summary>
        /// 이 클라이언트를 끊는다.
        /// </summary>
        public void Disconnect()
        {
            this.ClientLis.Disconnect(false);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientLis"></param>
        internal ClientModel(ClientListener clientLis)
        {
            this.ClientLis = clientLis;
            this.ClientLis.OnLog += ClientLis_OnLog;

            this.ClientLis.OnDisconnect += ClientLis_OnDisconnect;
            this.ClientLis.OnDisconnectCompleted += ClientLis_OnDisconnectCompleted;

            this.ClientLis.OnMessaged += ClientLis_OnMessaged;

        }

        private void ClientLis_OnLog(int nLogType, string sMessage)
        {
            this.OnLogCall(nLogType, sMessage);
        }



        /// <summary>
        /// 끊김 처리가 시작됨
        /// </summary>
        /// <param name="sender"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ClientLis_OnDisconnect(ClientListener sender)
        {
            this.DisconnectCall();
        }
        /// <summary>
        /// 끊김 처리가 완료됨
        /// </summary>
        /// <param name="sender"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ClientLis_OnDisconnectCompleted(ClientListener sender)
        {
            this.DisconnectCompletedCall();
        }

        /// <summary>
        /// 첫 메시지 대기
        /// </summary>
        internal void FirstListening()
        {
            this.ClientLis.FirstListening();
        }

        /// <summary>
        /// 메시지 수신
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ClientLis_OnMessaged(ClientListener sender, string message)
        {
            this.MessagedCall(this, message);
        }
    }
}
