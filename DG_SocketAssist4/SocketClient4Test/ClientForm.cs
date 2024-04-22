﻿using ChatGlobal;
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
                case typeState.None:    //기본
                    if (true == InvokeRequired)
                    {
                        this.Invoke(new Action(
                        delegate ()
                        {
                            txtMsg.Enabled = true;
                            btnSend.Text = "로그인";
                            btnSend.Enabled = true;
                        }));
                    }
                    else
                    {
                        txtMsg.Enabled = true;
                        btnSend.Text = "로그인";

                        btnSend.Enabled = true;
                    }

                    break;
                case typeState.Connecting:  //연결중
                    txtMsg.Enabled = false;
                    btnSend.Text = "연결중";
                    btnSend.Enabled = false;
                    break;
                case typeState.Connect: //연결완료
                    this.Invoke(new Action(
                        delegate ()
                        {
                            txtMsg.Enabled = true;
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
                    }));
        }
    }
}