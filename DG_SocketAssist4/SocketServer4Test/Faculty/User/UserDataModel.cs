using ChatGlobal;
using DG_SocketAssist4.Server;
using SocketServer4Test.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer4Test.Faculty.User
{
    /// <summary>
	/// 리스트로 관리될 유저 모델
	/// </summary>
	public class UserDataModel
    {
        #region 외부에 연결될 이벤트
        /// <summary>
        /// 로그 작성 이벤트
        /// </summary>
        internal event LogDelegate OnLog;
        /// <summary>
        /// 로그 작성 이벤트를 알림
        /// </summary>
        internal void OnLogCall(int nLogType, string sMessage)
        {
            if(null != this.OnLog)
            {
                this.OnLog(nLogType, sMessage);
            }
        }


        /// <summary>
        /// 유저 준비
        /// </summary>
        /// <param name="sender"></param>
        public delegate void UserReadyDelegate(UserDataModel sender);
        /// <summary>
        /// 유저 준비 이벤트
        /// <para>유저가 접속되기만 한 상태로 아직 서버의 허가가 나지 않은 상태이다.<br />
        /// </summary>
        public event UserReadyDelegate OnUserReady;
        /// <summary>
        /// 유저 준비가 완료되었음을 외부에 알림
        /// </summary>
        private void UserReadyCall()
        {
            if (null != OnUserReady)
            {
                this.OnUserReady(this);
            }
        }

        /// <summary>
        /// 서버로 메시지를 보내는 대리자
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void MessageDelegate(UserDataModel sender, MessageEventArgs e);
        /// <summary>
        /// 서버로 메시지를 보내려고 알리는 이벤트
        /// </summary>
        public event MessageDelegate OnSendMessage;
        /// <summary>
        /// 서버로 메시지를 보내려고 알리는 요청
        /// </summary>
        /// <param name="e"></param>
        private void OnSendMessageCall(MessageEventArgs e)
        {
            if(null != OnSendMessage)
            {
                this.OnSendMessage(this, e);
            }
        }

        /// <summary>
        /// 유저 로그인 완료
        /// </summary>
        /// <param name="sender"></param>
        public delegate void LoginCompletDelegate(UserDataModel sender);
        /// <summary>
        /// 유저 로그인 완료되면 발생함
        /// </summary>
        public event LoginCompletDelegate OnLoginComplet;

        /// <summary>
        /// 클라이언트 끊김 처리 완료
        /// </summary>
        /// <param name="sender"></param>
        public delegate void DisconnectCompletedDelegate(UserDataModel sender);
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
        #endregion


        /// <summary>
        /// 이 유저의 클라이언트 리스너 개체
        /// </summary>
        public ClientListener ClientListenerMe { get; private set; }

        /// <summary>
        /// 이 유저의 구분용 ID
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 유저 객체를 생성합니다.
        /// </summary>
        /// <param name="newClientListener">접속된 클라이언트의 리스너 개체</param>
        public UserDataModel(ClientListener newClientListener)
        {
            //소켓 저장
            this.ClientListenerMe = newClientListener;

            this.ClientListenerMe.OnValidationComplete


            //메시지 분석 연결
            this.ClientListenerMe.OnMessaged += ClientListenerMe_OnMessaged;
            //끊김 이벤트 연결
            this.ClientListenerMe.OnDisconnectCompleted += ClientListenerMe_OnDisconnectCompleted;

            //유저 준비를 알림
            this.UserReadyCall();
        }

        private void ClientListenerMe_OnDisconnectCompleted(ClientListener sender)
        {
            DisconnectCompletedCall();
        }

        private void ClientListenerMe_OnMessaged(ClientListener sender, string message)
        {
            //구분자로 명령을 구분 한다.
            string[] sData = GlobalStatic.ChatCmd.ChatCommandCut(message);


            //데이터 개수 확인
            if ((1 <= sData.Length))
            {
                //0이면 빈메시지이기 때문에 별도의 처리는 없다.

                //넘어온 명령
                ChatCommandType typeCommand
                    = GlobalStatic.ChatCmd.StrIntToType(sData[0]);

                switch (typeCommand)
                {
                    case ChatCommandType.None:   //없다
                        break;
                    case ChatCommandType.Msg:    //메시지인 경우
                        SendMeg_Main(sData[1], typeCommand);
                        break;
                    case ChatCommandType.ID_Check:   //아이디 체크
                        SendMeg_Main(sData[1], typeCommand);
                        break;

                    case ChatCommandType.User_List_Get:  //유저리스트 갱신 요청
                        SendMeg_Main("", typeCommand);
                        break;

                    case ChatCommandType.Login:  //로그인 완료
                        OnLoginComplet(this);
                        break;
                }
            }
        }


        /// <summary>
        /// 서버로 메시지를 보냅니다.
        /// </summary>
        /// <param name="sMag"></param>
        /// <param name="typeCommand"></param>
        private void SendMeg_Main(string sMag, ChatCommandType typeCommand)
        {
            MessageEventArgs e = new MessageEventArgs(sMag, typeCommand);

            this.OnSendMessageCall(e);
        }

        /// <summary>
        /// 이 유저에게 명령 문자열을 만들어 메시지를 보낸다.
        /// </summary>
        /// <param name="typeChatCommand"></param>
        /// <param name="sMsg"></param>
        public void SendMsg_User(ChatCommandType typeChatCommand, string sMsg)
        {
            string sToss
                = GlobalStatic.ChatCmd.ChatCommandString(
                    typeChatCommand
                    , sMsg);
            this.ClientListenerMe.Send(sToss);
        }

        /// <summary>
        /// 이 유저에게 메시지를 보낸다.
        /// </summary>
        /// <param name="sMsg"></param>
        public void SendMsg_User(string sMsg)
        {
            this.ClientListenerMe.Send(sMsg);
        }

        /// <summary>
        /// 이 유저를 끊는다.
        /// </summary>
        public void Disconnect()
        {
            this.ClientListenerMe.Disconnect(true);
        }
    }
}
