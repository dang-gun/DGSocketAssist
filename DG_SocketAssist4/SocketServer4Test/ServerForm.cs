using DG_SocketAssist4.Server;
using SocketServer4Test.Faculty;
using SocketServer4Test.Faculty.User;
using SocketServer4Test.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServer4Test
{
    public partial class ServerForm : Form
    {

        public ServerForm()
        {
            InitializeComponent();

            GlobalStatic.MainForm = this;
        }

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
            GlobalStatic.MainServer.Start(nPort);
        }

        /// <summary>
        /// 서버 중지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            //서버 중지
            GlobalStatic.MainServer.Stop();
            this.BtnDisplay(true);
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

        #region 유저 리스트 관련
        /// <summary>
        /// 유저 리스트에 ID 추가
        /// </summary>
        /// <param name="sId"></param>
        public void UserList_Add(string sId)
        {
            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        this.listUser.Items.Add(sId);
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

        /// <summary>
        /// 받아온 메시지를 출력 한다.
        /// </summary>
        /// <param name="nMessage"></param>
        public void DisplayMsg(string nMessage)
        {
            StringBuilder buffer = new StringBuilder();

            //시간 추가
            buffer.Append(
                String.Format("[{0}] "
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            //출력할 메시지 완성
            buffer.Append(nMessage);

            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                    delegate ()
                    {
                        this.listMsg.Items.Add(buffer.ToString());

                        this.listMsg.SelectedIndex = listMsg.Items.Count - 1;
                        this.listMsg.SelectedIndex = -1;
                    }));
        }

        /// <summary>
		/// 받아온 메시지를 출력 한다.
		/// </summary>
		/// <param name="nMessage"></param>
		public void DisplayLog(string nMessage)
        {
            StringBuilder buffer = new StringBuilder();

            //시간 추가
            buffer.Append(
                String.Format("[{0}] "
                    , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            //출력할 메시지 완성
            buffer.Append(nMessage);

            GlobalStatic.CrossThread_Winfom(this
                , new Action(
                        delegate ()
                        {
                            this.listLog.Items.Add(buffer.ToString());

                            this.listLog.SelectedIndex = listLog.Items.Count - 1;
                            this.listLog.SelectedIndex = -1;
                        }));
        }

        /// <summary>
        /// 모든 접속자에게 메시지를 보낸다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            string sTossMsg = string.Format("server : {0}", this.txtSendMsg.Text);
            GlobalStatic.MainServer.SendMsg_All(sTossMsg);
        }
    }
}
