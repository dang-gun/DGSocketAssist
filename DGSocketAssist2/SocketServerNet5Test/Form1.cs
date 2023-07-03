using ChatGlobal;
using DGSocketAssist2_Server;
using SocketServerTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServerNet5Test
{

    /// <summary>
    /// 서버 UI
    /// </summary>
    public partial class ServerForm : Form
    {
        #region 접속자 리스트 관련
        /// <summary>
        /// 접속한 유저 리스트(로그인 완료전 포함)
        /// </summary>
        private List<User> m_listUser = null;
        /// <summary>
        /// 접속 허가된 유저를 리스트에서 추가한다.
        /// </summary>
        /// <param name="sender"></param>
        public void UserList_Add(ClientListener sender)
        {
            if (null != sender)
            {
                User newUser = new User(sender);
                newUser.OnUserReady += NewUser_OnUserReady;
                newUser.OnMessaged += NewUser_OnMessaged;
                newUser.OnLoginComplet += NewUser_OnLoginComplet;
                newUser.OnDisconnectCompleted += NewUser_OnDisconnectCompleted;
                this.m_listUser.Add(newUser);
            }
        }


        /// <summary>
        /// 리스너로 유저를 찾아 리스트에서 제거한다.
        /// </summary>
        /// <param name="sender"></param>
        public void UserList_Remove(ClientListener sender)
        {
            if (null != sender)
            {
                //같은 리스너를 가진 유저를 찾는다.
                User findUser = UserList_FindUser(sender);
                if (null != findUser)
                {//유저를 찾았다
                 //리스트에서 지운다.
                    this.UserList_Remove(findUser);
                }
            }
        }

        /// <summary>
        /// 유저를 리스트에서 제거한다.
        /// </summary>
        /// <param name="sender"></param>
        public void UserList_Remove(User sender)
        {
            if (null != sender)
            {
                //리스트에서 지운다.
                this.m_listUser.Remove(sender);
            }
        }

        /// <summary>
        /// 지정한 클라이언트 리스너와 동일한 유저를 찾아 리턴한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public User UserList_FindUser(ClientListener sender)
        {
            //같은 리스너를 가진 유저를 찾는다.
            User findUser
                = this.m_listUser
                    .Where(w => w.ClientListenerMe == sender)
                    .FirstOrDefault();

            return findUser;
        }
        #endregion

        public ServerForm()
        {
            InitializeComponent();

            GloblaStatic.MainForm = this;

            this.Text = string.Format("Socket Server Test({0})"
                                        , ChatSetting.SiteTitle);
        }

        /// <summary>
        /// 폼이 닫치기 전에 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != GloblaStatic.Server)
            {
                GloblaStatic.Server.Stop();
            }
        }

        #region UI 관련

        /// <summary>
        /// 서버 시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            //버튼 표시
            BtnDisplay(false);

            int nPort = Convert.ToInt32(txtPort.Text);
            GloblaStatic.Server = new Server(nPort);
            GloblaStatic.Server.OnStart += Server_OnStart;
            GloblaStatic.Server.OnStop += Server_OnStop;

            GloblaStatic.Server.OnConnected += Server_OnConnected;
            GloblaStatic.Server.OnValidationComplete += Server_ValidationComplete;
            GloblaStatic.Server.OnDisconnect += Server_OnDisconnected;
            GloblaStatic.Server.OnMessaged += Server_OnMessaged;
            GloblaStatic.Server.Start();

            //유저 리스트 초기화
            this.m_listUser = new List<User>();
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            //서버 중지
            GloblaStatic.Server.Stop();
            BtnDisplay(true);
        }

        /// <summary>
        /// 접속중인 모든 클라이언트에게 메시지를 전달함.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] byteCmdMsg
                = GloblaStatic.ChatCmd
                    .ChatString(
                        ChatCommandType.MsgSend
                        , "Server : " + this.txtSendMsg.Text);

            //전체로 메시지 전송
            this.AllUser_Send(byteCmdMsg, null);
        }

        private void btnImageSend_Click(object sender, EventArgs e)
        {
            //명령어 만들기
            byte[] byteTemp = File.ReadAllBytes(txtDir.Text);
            this.AllUser_Send(ChatCommandType.FileSend, byteTemp, null);
        }


        /// <summary>
        /// 버튼을 화면에 표시하거나 가린다.
        /// </summary>
        /// <param name="bView"></param>
        private void BtnDisplay(bool bView)
        {
            if (true == bView)
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            else
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }

        }


        /// <summary>
        /// 받아온 메시지를 출력 한다.
        /// </summary>
        /// <param name="nMessage"></param>
        public void DisplayLog(String nMessage)
        {
            StringBuilder buffer = new StringBuilder();

            //시간 추가
            buffer.Append(
                String.Format("[{0}] "
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            //출력할 메시지 완성
            buffer.Append(nMessage);

            //출력
            this.Invoke(new Action(
                        delegate ()
                        {
                            listLog.Items.Add(buffer.ToString());

                            listLog.SelectedIndex = listLog.Items.Count - 1;
                            listLog.SelectedIndex = -1;
                        }));

        }

        #endregion

        #region 서버 이벤트 콜백
        /// <summary>
        /// 서버 이벤트 콜백 - 서버가 시작됨
        /// </summary>
        private void Server_OnStart()
        {
            DisplayLog("서버가 시작됨");
        }

        /// <summary>
        /// 서버 이벤트 콜백 - 서버가 멈춤
        /// </summary>
        private void Server_OnStop()
        {
            DisplayLog("서버가 멈춤");
        }

        /// <summary>
        /// 서버 이벤트 콜백 - 클라이언트 연결됨
        /// </summary>
        /// <param name="sender"></param>
        private void Server_OnConnected(ClientListener sender)
        {
            DisplayLog("클라이언트 연결됨 : " + sender.SocketMe.RemoteEndPoint);
        }

        /// <summary>
        /// 서버 이벤트 콜백 - 클라이언트 허가됨
        /// </summary>
        /// <param name="sender"></param>
        private void Server_ValidationComplete(ClientListener sender)
        {

            DisplayLog("클라이언트 허가됨 : " + sender.SocketMe.RemoteEndPoint);

            //클라이언트가 허가되면 리스트에 추가한다.
            this.UserList_Add(sender);
        }

        /// <summary>
        /// 서버 이벤트 콜백 - 클라이언트 끊김
        /// </summary>
        /// <param name="sender"></param>
        private void Server_OnDisconnected(ClientListener sender)
        {
            DisplayLog("클라이언트 끊김 : " + sender.SocketMe.RemoteEndPoint);
        }

        /// <summary>
        /// 서버 이벤트 콜백 - 클라이언트 메시지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="byteMessage"></param>
        private void Server_OnMessaged(
            ClientListener sender
            , byte[] byteMessage)
        {
            DisplayLog("클라이언트 메시지 : " + sender.SocketMe.RemoteEndPoint);
            Debug.WriteLine("클라이언트 메시지 : " + byteMessage);
            //DisplayLog(ChatSetting.ByteArrayToString(byteMessage));

            //서버폼에서는 이것을 명령어가 왔음으로 처리하고 마무리한다.
            //명령어 처리는 유저 클래스에서 하고
            //서버 폼이 처리해야할 내용은 별도의 메시지 콜백으로 처리한다.
        }
        #endregion

        #region 유저 이벤트 콜백 처리 관련

        /// <summary>
        /// 유저 개체가 준비됨
        /// </summary>
        /// <param name="sender"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void NewUser_OnUserReady(User sender)
        {
            //유저 개체가 준비되었을때 필요한 동작을 여기서 함.
        }

        /// <summary>
        /// 유저가 서버에 알리는 메시지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void NewUser_OnMessaged(User sender, MessageEventArgs e)
        {
            StringBuilder sbMsg = new StringBuilder();

            switch (e.m_typeCommand)
            {
                case ChatCommandType.MsgSend:    //메시지
                    sbMsg.Append(sender.UserID);
                    sbMsg.Append(" : ");
                    sbMsg.Append(e.DataString);
                    //화면에 출력
                    DisplayLog(sbMsg.ToString());

                    Commd_SendMsg(sbMsg.ToString(), sender);
                    break;

                case ChatCommandType.FileSend://파일
                    this.Commd_FileSend(sender, e.DataByte);
                    break;

                case ChatCommandType.ID_Check:   //id체크
                    Commd_IDCheck(sender, e.DataString);
                    break;
                case ChatCommandType.User_List_Get:  //유저 리스트 갱신 요청
                    Commd_User_List_Get(sender);
                    break;
            }
        }

        /// <summary>
        /// 유저 로그인 완료.
        /// 로그인 완료된 유저를 UI에 표시하고,
        /// 기존 접속자들에게 다른 유저 접속 알린다.
        /// </summary>
        /// <param name="sender"></param>
        private void NewUser_OnLoginComplet(User sender)
        {
            //로그인이 완료된 유저에게 유저 리스트를 보낸다.
            this.Commd_User_List_Get(sender);

            //전체 유저에게 접속자를 알린다.
            byte[] byteSendData
                = GloblaStatic.ChatCmd
                    .ChatString(
                        ChatCommandType.User_Connect
                        , sender.UserID);


            //전체 유저에게 메시지 전송(지금 로그인 한 접속자는 제외)
            AllUser_Send(byteSendData, sender);

            //로그 유저 리스트에 추가
            this.Invoke(new Action(
                delegate ()
                {
                    listUser.Items.Add(sender.UserID);
                }));

            //로그 남기기
            DisplayLog(string.Format("*** 접속자 : {0} ***", sender.UserID));
        }

        /// <summary>
        /// 유저의 끊김 처리가 시작되었다.
        /// </summary>
        /// <param name="sender"></param>
        private void NewUser_OnDisconnectCompleted(User sender)
        {
            //로그 유저 리스트에서 제거
            this.Invoke(new Action(
                delegate ()
                {
                    if (null != sender.UserID)
                    {
                        listUser.Items.RemoveAt(listUser.FindString(sender.UserID));
                    }
                }));

            if (null != sender.UserID)
            {
                //다른 유저들에게 이 유저가 끊겼음을 알린다.
                byte[] byteCmdMsg
                    = GloblaStatic.ChatCmd
                        .ChatString(
                            ChatCommandType.User_Disonnect
                            , sender.UserID);
                this.AllUser_Send(byteCmdMsg, sender);
            }

            //클라이언트의 접속 끊김 처리가 시작되면 리스트에서 제거한다.
            this.UserList_Remove(sender);
        }
        #endregion


        /// <summary>
        /// 명령 처리 - 메시지 보내기
        /// </summary>
        /// <param name="sMsg"></param>
        /// <param name="sender">호출자. 호출자한테도 보내고 싶으면 null로 보낸다</param>
        private void Commd_SendMsg(string sMsg, User sender)
        {
            byte[] byteSendData
                = GloblaStatic.ChatCmd
                    .ChatString(
                        ChatCommandType.MsgSend
                        , sMsg);

            //전체 유저에게 메시지 전송
            //호출자는 제외
            this.AllUser_Send(byteSendData, sender);
        }

        /// <summary>
        /// 파일 보내기 요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="byteData"></param>
        private void Commd_FileSend(User sender, byte[] byteData)
        {
            //미리 보기 표시
            this.pbDownImage.Image = this.ByteToImage(byteData);

            //센더를 제외하고 전송하기
            this.AllUser_Send(ChatCommandType.FileSend, byteData, sender);
        }

        /// <summary>
        /// 명령 처리 - ID체크
        /// </summary>
        /// <param name="insUser"></param>
        /// <param name="sID"></param>
        private void Commd_IDCheck(User insUser, string sID)
        {
            //사용 가능 여부
            bool bReturn = true;

            //모든 유저의 아이디 체크
            foreach (User insUserTemp in m_listUser)
            {
                if (insUserTemp.UserID == sID)
                {
                    //같은 유저가 있다!
                    //같은 유저가 있으면 그만 검사한다.
                    bReturn = false;
                    break;
                }
            }

            if (true == bReturn)
            {//사용 가능

                //아이디를 지정하고
                insUser.UserID = sID;

                insUser.SendMsg_User(ChatCommandType.ID_Check_Ok, new byte[0]);

                //유저가 접속 했음을 직접 알리지 말고 'ID_Check_Ok'를 받은
                //클라이언트가 직접 요청한다.
            }
            else
            {
                //검사 실패를 알린다.
                insUser.SendMsg_User(ChatCommandType.ID_Check_Fail, new byte[0]);
            }
        }

        /// <summary>
        /// 명령 처리 - 유저 리스트 갱신 요청
        /// </summary>
        /// <param name="insUser"></param>
        private void Commd_User_List_Get(User insUser)
        {
            StringBuilder sbList = new StringBuilder();

            //리스트 만들기
            foreach (User insUser_Temp in this.m_listUser)
            {
                sbList.Append(insUser_Temp.UserID);
                sbList.Append(",");
            }

            //요청에 응답해준다.
            insUser.SendMsg_User(ChatCommandType.User_List, sbList.ToString());
        }

        /// <summary>
        /// 접속중인 모든 유저에게 메시지를 보낸다
        /// </summary>
        /// <param name="typeChatCommand"></param>
        /// <param name="byteData"></param>
        /// <param name="user">제외할 유저. 없으면 null</param>
        private void AllUser_Send(
            ChatCommandType typeChatCommand
            , byte[] byteData
            , User user)
        {
            //명령 만들기
            byte[] byteSendData
                = GloblaStatic.ChatCmd
                    .ChatString(
                        typeChatCommand
                        , byteData);

            //모든 유저에게 메시지를 전송 한다.
            this.AllUser_Send(byteSendData, user);
        }

        /// <summary>
        /// 접속중인 모든 유저에게 메시지를 보낸다<br />
        /// 이미 체팅명령 헤더가 붙은 경우 이것으로 메시지를 보낸다.
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="user">제외할 유저. 없으면 null</param>
        private void AllUser_Send(
            byte[] byteData
            , User user)
        {
            //모든 유저에게 메시지를 전송 한다.
            foreach (User itemUser in m_listUser)
            {
                //제외 유저
                if ((null == user)
                    || (itemUser.UserID != user.UserID))
                {//제외할 유저가 없거나.
                 //제외할 유저가 아니라면

                    //메시지를 보낸다.
                    itemUser.SendMsg_User(byteData);
                }

            }
        }


        private void btnDir_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDir.Text = openFileDialog.FileName;
                }
            }//end using openFileDialog
        }

        /// <summary>
        /// 바이너리를 이미지로 바꾼다.
        /// https://stackoverflow.com/questions/9576868/how-to-put-image-in-a-picture-box-from-a-byte-in-c-sharp
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        public Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }


    }
}