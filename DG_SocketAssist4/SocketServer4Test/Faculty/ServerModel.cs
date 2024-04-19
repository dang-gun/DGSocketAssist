
using ChatGlobal;
using DG_SocketAssist4.Server;

using SocketServer4Test.Faculty.User;
using SocketServer4Test.Global;
using System.Text;

namespace SocketServer4Test.Faculty
{
    internal class ServerModel
    {
        private ServerSocket Server;

        /// <summary>
        /// 관리할 유저 리스트
        /// </summary>
        private UserListModel UserList;

        public ServerModel()
        {
            
        }


        /// <summary>
        /// 서버 시작
        /// </summary>
        public void Start(int nPort)
        {
            //서버 개체 생성
            this.Server = new ServerSocket(nPort);
            this.Server.OnLog += this.Server_OnLog;
            this.Server.OnConnected += Server_OnConnected;

            this.UserList = new UserListModel();
            this.UserList.OnLog += UserList_OnLog;

            this.UserList.OnUserReady += UserList_OnUserReady;
            this.UserList.OnMessaged += UserList_OnMessaged;

            this.Server.Start();
        }

        /// <summary>
        /// 서버 종료
        /// </summary>
        public void Stop()
        {
            this.Server.Stop();
        }


        #region 유저 리스트 이벤트

        /// <summary>
        /// 서버에서 절달된 로그
        /// </summary>
        /// <param name="nLogType"></param>
        /// <param name="sMessage"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void Server_OnLog(int nLogType, string sMessage)
        {
            this.Log(string.Format("[server:{0}] {1}"
                                    , nLogType
                                    , sMessage));
        }

        /// <summary>
        /// 클라이언트 접속함
        /// </summary>
        /// <param name="sender"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void Server_OnConnected(ClientListener sender)
        {
            //유저 체크 시작
            this.UserList.UserCheckStart(sender);
        }

        private void Server_OnMessaged(ClientListener sender, string message)
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region 유저 리스트 이벤트

        private void UserList_OnLog(int nLogType, string sMessage)
        {
            this.Log(string.Format("[UserList:{0}] {1}"
                        , nLogType
                        , sMessage));
        }


        private void UserList_OnUserReady(UserDataModel sender)
        {
            
        }

        private void UserList_OnMessaged(
            UserDataModel sender
            , ChatGlobal.MessageEventArgs e)
        {
            StringBuilder sbMsg = new StringBuilder();

            switch (e.m_typeCommand)
            {
                case ChatCommandType.Msg:
                    this.Commd_ReceiveMsg(sender, e.m_strMsg);
                    break;

                case ChatCommandType.ID_Check:   //id체크
                    this.Commd_IDCheck(sender, e.m_strMsg);
                    break;
                case ChatCommandType.User_List_Get:  //유저 리스트 갱신 요청
                    //Commd_User_List_Get(sender);
                    break;
            }
        }
        #endregion

        #region 명령 처리 관련
        /// <summary>
		/// 명령 처리 - ID체크
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="sID"></param>
		private void Commd_IDCheck(UserDataModel sender, string sID)
        {
            //사용 가능 여부
            bool bReturn = true;

            //모든 유저의 아이디 체크
            UserDataModel findUser = this.UserList.FindUser(sID);
            if(null != findUser)
            {//같은 유저가 있다!
                bReturn = false;
            }

            if (true == bReturn)
            {//사용 가능

                //아이디를 지정하고
                sender.UserId = sID;

                //명령어 만들기
                string sSendData
                    = GlobalStatic.ChatCmd
                        .ChatCommandString(
                            ChatCommandType.ID_Check_Ok
                            , string.Empty);

                sender.SendMsg_User(sSendData);

                //유저 체크 성공
                this.UserList.UserCheckOk(sender);
                this.UserList_Add(sender.UserId);
            }
            else
            {
                //검사 실패를 알린다.
                string sSendData
                    = GlobalStatic.ChatCmd
                        .ChatCommandString(
                            ChatCommandType.ID_Check_Fail
                            , string.Empty);

                sender.SendMsg_User(sSendData);

                //유저 체크 실패
                this.UserList.UserCheckFail(sender);
            }
        }

        /// <summary>
        /// 받은 메시지 체팅창에 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sMsg"></param>
        private void Commd_ReceiveMsg(UserDataModel sender, string sMsg)
        {
            string sTossMsg
                = string.Format("{0} : {1}"
                    , sender.UserId
                    , sMsg);

            GlobalStatic.MainForm.DisplayMsg(sTossMsg);

            //모든 유저에게 메시지 전달
            this.SendMsg_All(sTossMsg);
        }
        #endregion

        #region 유저 리스트 UI 관련
        /// <summary>
        /// 유저 리스트 UI에 ID 추가
        /// </summary>
        /// <param name="sId"></param>
        public void UserList_Add(string sId)
        {
            GlobalStatic.MainForm.UserList_Add(sId);
        }

        /// <summary>
        /// 유저 리스트 UI에 ID 제거
        /// </summary>
        /// <param name="sId"></param>
        public void UserList_Remove(string sId)
        {
            GlobalStatic.MainForm.UserList_Remove(sId);
        }

        /// <summary>
        /// 유저 리스트 UI를 모두 지우고 접속자 리스트 기준으로 다시 ID를 넣는다.
        /// </summary>
        public void UserList_Refresh()
        {
            GlobalStatic.MainForm.UserList_Clear();

            string[] arrUserId = this.UserList.UserIdList;
            for (int i = 0; i < arrUserId.Length; i++)
            {
                string sItem = arrUserId[i];

                this.UserList_Add(sItem);
            }
        }
        #endregion

        #region 메시지 관련

        
        /// <summary>
        /// 접속 완료된 모든 유저에게 메시지를 보낸다.
        /// </summary>
        /// <param name="sMsg"></param>
        public void SendMsg_All(string sMsg)
        {
            string sSendData
                = GlobalStatic.ChatCmd
                    .ChatCommandString(
                        ChatCommandType.Msg
                        , sMsg);
            this.UserList.SendMsg_All(sSendData);
        }
        #endregion

        /// <summary>
        /// 로그 출력
        /// </summary>
        /// <param name="sMsg"></param>
        private void Log(string sMsg)
        {
            GlobalStatic.MainForm.DisplayLog(sMsg);
        }
    }
}
