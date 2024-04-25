using ChatGlobal;
using SocketClient4Test.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient4Test
{
    public partial class ClientForm : Form
    {
        /// <summary>
        /// 나의 상태
        /// </summary>
        public enum typeState
        {
            /// <summary>
            /// 없음
            /// </summary>
            None = 0,
            /// <summary>
            /// 연결중
            /// </summary>
            Connecting,
            /// <summary>
            /// 연결 완료
            /// </summary>
            Connect,

            /// <summary>
            /// 연결 끊김
            /// </summary>
            Disconnect,
        }

        /// <summary>
        /// 나의 상태
        /// </summary>
        private typeState m_typeState = typeState.None;

        public ClientForm()
        {
            InitializeComponent();

            GlobalStatic.MainForm = this;
            this.Text = string.Format("Socket Client Test({0})"
                                        , ChatSetting.SiteTitle);
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != GlobalStatic.MainClient)
            {
                GlobalStatic.MainClient.Disconnect();
            }
        }

        /// <summary>
        /// 로그인/메시지 보내기 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            switch (m_typeState)
            {
                case typeState.None://기본
                    if ("" == txtMsg.Text)
                    {
                        //입력값이 없으면 리턴
                        MessageBox.Show("아이디를 넣고 시도해 주세요");
                        return;
                    }
                    else
                    {
                        //아이디가 있으면 로그인 시작

                        //유아이를 세팅하고
                        this.UI_Setting(typeState.Connecting);

                        string sIP = this.txtIp.Text;
                        int nPort = Convert.ToInt32(txtPort.Text);

                        //서버연결 처리 개체 
                        GlobalStatic.MainClient
                            .ConnectStart(
                                this.txtIp.Text
                                , nPort
                                , this.txtMsg.Text);
                    }
                    break;

                case typeState.Connect://접속 상태
                                       //이상태에서는 메시지를 보낸다.
                    GlobalStatic.MainClient.SendMsg(ChatCommandType.Msg, txtMsg.Text);
                    this.txtMsg.Text = "";
                    break;
            }
        }



        /// <summary>
        /// UI 세팅
        /// </summary>
        /// <param name="typeSet"></param>
        public void UI_Setting(typeState typeSet)
        {
            //들어온 값을 세팅하고
            m_typeState = typeSet;

            switch (typeSet)
            {
                case typeState.None://기본
                case typeState.Disconnect: //끊김
                    GlobalStatic.CrossThread_Winfom(this
                        , new Action(delegate ()
                        {
                            txtMsg.Enabled = true;
                            btnSend.Text = "로그인";
                            btnSend.Enabled = true;

                            //유저 리스트 비우기
                            this.UserList_Clear();
                        }));

                    //처음으로 돌리기위해 typeState.None로 초기화 한다.
                    m_typeState = typeState.None;
                    break;

                case typeState.Connecting:  //연결중
                    txtMsg.Enabled = false;
                    btnSend.Text = "연결중";
                    btnSend.Enabled = false;
                    break;
                case typeState.Connect: //연결완료
                    GlobalStatic.CrossThread_Winfom(this
                        , new Action(delegate ()
                        {
                            txtMsg.Enabled = true;
                            this.txtMsg.Text = "";

                            btnSend.Text = "보내기";
                            btnSend.Enabled = true;
                        }));
                    break;
            }
        }


        /// <summary>
        /// 받아온 메시지를 출력 한다.
        /// </summary>
        /// <param name="nMessage"></param>
        public void DisplayMsg(string nMessage)
        {
            StringBuilder sb = new StringBuilder();

            //시간 추가
            sb.Append(
                String.Format("[{0}] "
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            //출력할 메시지 완성
            sb.Append(nMessage);

            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        this.listMsg.Items.Add(sb.ToString());
                    }));
        }

        #region 유저 리스트 관련
        /// <summary>
        /// 유저 리스트에 ID 추가
        /// </summary>
        /// <param name="sId"></param>
        public void UserList_Add(string sId)
        {
            if(string.Empty != sId)
            {
                GlobalStatic.CrossThread_Winfom(this
                    , new Action(
                        delegate ()
                        {
                            this.listUser.Items.Add(sId);
                        }));
            }

        }

        /// <summary>
        /// 유저 리스트를 다시 넣는다.
        /// </summary>
        /// <remarks>기존 리스트를 비우고 전달받은 리스트를 다시 추가한다.</remarks>
        /// <param name="sUserList"></param>
        public void UserList_Add_List(string sUserList)
        {
            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        //리스트를 비우고
                        listUser.Items.Clear();

                        //리스트를 다시 채워준다.
                        string[] sList = sUserList.Split(',');
                        for (int i = 0; i < sList.Length; ++i)
                        {
                            if(string.Empty != sList[i])
                            {
                                listUser.Items.Add(sList[i]);
                            }
                        }
                    }));
        }

        /// <summary>
        /// 유저 리스트에 ID 제거
        /// </summary>
        /// <param name="sId"></param>
        public void UserList_Remove(string sId)
        {
            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        this.listUser
                            .Items
                            .RemoveAt(this.listUser.FindString(sId));
                    }));
        }

        /// <summary>
        /// 유저 리스트 비우기
        /// </summary>
        public void UserList_Clear()
        {
            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        this.listUser.Items.Clear();
                    }));
        }
        #endregion

        public void DisplayLog(string nMessage)
        {
            StringBuilder sb = new StringBuilder();

            //시간 추가
            sb.Append(
                String.Format("[{0}] "
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            //출력할 메시지 완성
            sb.Append(nMessage);

            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        this.listLog.Items.Add(sb.ToString());

                        this.listLog.SelectedIndex = listLog.Items.Count - 1;
                        this.listLog.SelectedIndex = -1;
                    }));
        }

        private void tsmiUserListCall_Click(object sender, EventArgs e)
        {
            //유저 리스트 갱신 요청
            GlobalStatic.MainClient
                .SendMsg(ChatCommandType.User_List_Get, "");
        }

        private void tsmiSendMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
