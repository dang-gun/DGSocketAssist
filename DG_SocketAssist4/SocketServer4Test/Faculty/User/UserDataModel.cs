﻿using ChatGlobal;
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

        #endregion


        /// <summary>
        /// 이 유저의 클라이언트 리스너 개체
        /// </summary>
        public ClientModel ClientMe { get; private set; }

        /// <summary>
        /// 이 유저의 구분용 ID
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 유저 객체를 생성합니다.
        /// </summary>
        /// <param name="newClientListener">접속된 클라이언트의 리스너 개체</param>
        public UserDataModel(ClientModel newClientListener)
        {
            //리스너 저장
            this.ClientMe = newClientListener;
            //로그
            this.ClientMe.OnLog += ClientListenerMe_OnLog;


            //메시지 분석 연결
            this.ClientMe.OnMessaged += ClientListenerMe_OnMessaged;
        }

        
        /// <summary>
        /// 로그 전달 요청
        /// </summary>
        /// <param name="nLogType"></param>
        /// <param name="sMessage"></param>
        private void ClientListenerMe_OnLog(int nLogType, string sMessage)
        {
            this.OnLogCall(nLogType
                , string.Format("[ClientListener:{0}] {1}"
                                , nLogType
                                , sMessage));
        }

        /// <summary>
        /// 메시지 수신
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void ClientListenerMe_OnMessaged(ClientModel sender, string message)
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
                        this.SendMeg_Main(typeCommand, sData[1]);
                        break;

                    case ChatCommandType.SignIn:   //아이디 체크
                        this.SendMeg_Main(typeCommand, sData[1]);
                        break;

                    case ChatCommandType.User_List_Get:  //유저리스트 갱신 요청
                        this.SendMeg_Main(typeCommand, "");
                        break;
                }
            }
        }


        /// <summary>
        /// 서버로 메시지를 보냅니다.
        /// </summary>
        /// <param name="sMag"></param>
        /// <param name="typeCommand"></param>
        private void SendMeg_Main(ChatCommandType typeCommand, string sMag)
        {
            MessageEventArgs e = new MessageEventArgs(typeCommand, sMag);

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
            this.SendMsg_User(sToss);
        }

        /// <summary>
        /// 이 유저에게 메시지를 보낸다.
        /// </summary>
        /// <param name="sMsg"></param>
        public void SendMsg_User(string sMsg)
        {
            this.OnLogCall(0, string.Format("[UserDataModel.SendMsg_User] 보내기 요청({0}) : {1}"
                                            , this.UserName
                                            , sMsg));
            this.ClientMe.Send(sMsg);
        }

        /// <summary>
        /// 이 유저를 끊는다.
        /// </summary>
        public void Disconnect()
        {
            this.ClientMe.Disconnect();
        }
    }
}
