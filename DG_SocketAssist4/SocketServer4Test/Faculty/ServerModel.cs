
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
            this.Server.OnConnected += this.Server_OnConnected;
            this.Server.OnDisconnect += Server_OnDisconnect;
            this.Server.OnDisconnectCompleted += Server_OnDisconnectCompleted;


            this.UserList = new UserListModel();
            this.UserList.OnLog += UserList_OnLog;

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


        #region 서버 소켓 이벤트

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
        /// 클라이언트가 접속 성공함
        /// </summary>
        /// <param name="sender"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void Server_OnConnected(ClientModel sender)
        {
            //유저 체크 시작
            this.UserList.UserCheckStart(sender);
        }

        
        /// <summary>
        /// 클라이언트 접속끊김이 감지되어 끊김 작업이 시작됨
        /// </summary>
        /// <param name="sender"></param>
        private void Server_OnDisconnect(ClientModel sender)
        {
            //끊김 처리가 시작되었으면 중간에 취소될리가 없으므로 그냥 끊어졌다고 판단하고 작업한다.

            //끊어진 대상 이름
            string sName = this.UserList.FindUser(sender.ClientIndex).UserName;

            //UI에서 제거
            this.UserListUi_Remove(sName);
            //리스트에서 제거
            this.UserList.UserList_Remove(sender);
            

            //끊어진 대상을 알려줌
            this.Send_All(ChatCommandType.User_Disonnect, sName);
        }

        /// <summary>
        /// 클라이언트 끊김 작업이 완료됨
        /// </summary>
        /// <param name="nClientIndex"></param>
        private void Server_OnDisconnectCompleted(long nClientIndex)
        {
            
        }
        #endregion


        #region 유저 리스트 이벤트

        private void UserList_OnLog(int nLogType, string sMessage)
        {
            this.Log(string.Format("[UserList:{0}] {1}"
                        , nLogType
                        , sMessage));
        }


        /// <summary>
        /// 유저로 부터 전달된 메시지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserList_OnMessaged(
            UserDataModel sender
            , ChatGlobal.MessageEventArgs e)
        {


            this.Log(string.Format("[UserList_OnMessaged] {0} : {1}"
                                    , e.CommandType
                                    , e.Message));

            StringBuilder sbMsg = new StringBuilder();

            switch (e.CommandType)
            {
                case ChatCommandType.Msg:
                    this.Commd_ReceiveMsg(sender, e.Message);
                    break;

                case ChatCommandType.SignIn:   //id체크
                    this.Commd_SignIn(sender, e.Message);
                    break;
                case ChatCommandType.User_List_Get:  //유저 리스트 갱신 요청
                    this.Commd_User_List_Get(sender);
                    break;
            }
        }
        #endregion

        #region 명령 처리 관련


        /// <summary>
        /// 받은 메시지 체팅창에 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sMsg"></param>
        private void Commd_ReceiveMsg(UserDataModel sender, string sMsg)
        {
            string sTossMsg
                = string.Format("{0} : {1}"
                    , sender.UserName
                    , sMsg);

            GlobalStatic.MainForm.DisplayMsg(sTossMsg);

            //모든 유저에게 메시지 전달
            this.SendMsg_All(sTossMsg);
        }

        /// <summary>
		/// 명령 처리 - ID체크
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="sID"></param>
		private void Commd_SignIn(UserDataModel sender, string sID)
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
                sender.UserName = sID;

                //명령어 만들기
                string sSendData
                    = GlobalStatic.ChatCmd
                        .ChatCommandString(
                            ChatCommandType.SignIn_Ok
                            , string.Empty);

                sender.SendMsg_User(sSendData);

                //유저 체크 성공
                this.UserList.UserCheckOk(sender);
                this.UserListUi_Add(sender.UserName);

                //접속한 유저를 제외하고 전체를 유제에게 알린다.
                this.Send_All(sender, ChatCommandType.User_Connect, sender.UserName);
            }
            else
            {
                //검사 실패를 알린다.
                string sSendData
                    = GlobalStatic.ChatCmd
                        .ChatCommandString(
                            ChatCommandType.SignIn_Fail
                            , string.Empty);

                sender.SendMsg_User(sSendData);

                //유저 체크 실패
                this.UserList.UserCheckFail(sender);
            }
        }

        /// <summary>
        /// 유저 리스트 요청 처리
        /// </summary>
        /// <param name="sender">요청자</param>
        private void Commd_User_List_Get(UserDataModel sender)
        {
            StringBuilder sbList = new StringBuilder();

            //리스트 만들기
            foreach (string sItem in this.UserList.UserNameList)
            {
                sbList.Append(sItem);
                sbList.Append(",");
            }

            string sSendData
                = GlobalStatic.ChatCmd
                    .ChatCommandString(
                        ChatCommandType.User_List
                        , sbList.ToString());

            sender.SendMsg_User(sSendData);
        }

        #endregion

        #region 유저 리스트 UI 관련
        /// <summary>
        /// 유저 리스트 UI에 ID 추가
        /// </summary>
        /// <param name="sId"></param>
        public void UserListUi_Add(string sId)
        {
            GlobalStatic.MainForm.UserList_Add(sId);
        }

        /// <summary>
        /// 유저 리스트 UI에 ID 제거
        /// </summary>
        /// <param name="sId"></param>
        public void UserListUi_Remove(string sId)
        {
            GlobalStatic.MainForm.UserList_Remove(sId);
        }

        /// <summary>
        /// 유저 리스트 UI를 모두 지우고 접속자 리스트 기준으로 다시 ID를 넣는다.
        /// </summary>
        public void UserListUi_Refresh()
        {
            GlobalStatic.MainForm.UserList_Clear();

            string[] arrUserId = this.UserList.UserNameList;
            for (int i = 0; i < arrUserId.Length; i++)
            {
                string sItem = arrUserId[i];

                this.UserListUi_Add(sItem);
            }
        }
        #endregion

        #region 메시지 관련

        /// <summary>
        /// 접속 완료된 모든 유저에게 지정된 명령을 보낸다.
        /// </summary>
        /// <param name="targetExcept">제외할 대상</param>
        /// <param name="typeChatCmd"></param>
        /// <param name="sMsg"></param>
        public void Send_All(
            UserDataModel targetExcept
            , ChatCommandType typeChatCmd
            , string sMsg)
        {
            string sSendData
                = GlobalStatic.ChatCmd
                    .ChatCommandString(typeChatCmd, sMsg);
            this.UserList.SendMsg_All(targetExcept, sSendData);
        }

        /// <summary>
        /// 접속 완료된 모든 유저에게 지정된 명령을 보낸다.
        /// </summary>
        /// <param name="typeChatCmd"></param>
        /// <param name="sMsg"></param>
        public void Send_All(ChatCommandType typeChatCmd, string sMsg)
        {
            string sSendData
                = GlobalStatic.ChatCmd
                    .ChatCommandString(typeChatCmd, sMsg);
            this.UserList.SendMsg_All(sSendData);
        }

        /// <summary>
        /// 접속 완료된 모든 유저에게 메시지를 보낸다.
        /// </summary>
        /// <param name="sMsg"></param>
        public void SendMsg_All(string sMsg)
        {
            this.Send_All(ChatCommandType.Msg, sMsg);
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
