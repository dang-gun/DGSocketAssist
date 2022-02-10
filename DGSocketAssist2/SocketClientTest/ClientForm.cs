using ChatGlobal;
using DGSocketAssist2_Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClientTest
{
	/// <summary>
	/// 유저 UI
	/// </summary>
	public partial class ClientForm : Form
	{
		/// <summary>
		/// 나의 상태
		/// </summary>
		enum typeState
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

		/// <summary>
		/// 유저 UI 생성
		/// </summary>
		public ClientForm()
		{
			InitializeComponent();

			GloblaStatic.MainForm = this;
			this.Text = string.Format("Socket Client Test({0})"
										, ChatSetting.SiteTitle);
		}

		#region 메뉴 - 서버
		/// <summary>
		/// 메뉴 - 서버 - 접속 준비
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsmiConnectReady_Click(object sender, EventArgs e)
		{
			string nIP = "127.0.0.1";
			int nPort = Convert.ToInt32(txtPort.Text);

			//클라이언트 개체 생성
			GloblaStatic.Client = new Client(nIP, nPort);
			GloblaStatic.Client.OnConnectionComplete += ServerTestClient_OnConnectionComplete;
			GloblaStatic.Client.OnDisconnect += ServerTestClient_OnDisconnect;
			GloblaStatic.Client.OnDisconnectCompleted += ServerTestClient_OnDisconnectCompleted;
			GloblaStatic.Client.OnReceiveReady += ServerTestClient_OnReceiveReady;
			GloblaStatic.Client.OnMessaged += ServerTestClient_OnMessaged;

			DisplayMsg("서버 준비 완료");
		}

		#region 서버 테스트용 이벤트 콜백
		/// <summary>
		/// 서버 접속 완료
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체</param>
		private void ServerTestClient_OnConnectionComplete(Client sender)
		{
			DisplayMsg("서버 접속 완료");
		}
		/// <summary>
		/// 서버 끊김 처리가 시작됨
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체</param>
		private void ServerTestClient_OnDisconnect(Client sender)
		{
			DisplayMsg("서버 끊김 : " + sender.SocketMe.RemoteEndPoint);
		}
		/// <summary>
		/// 서버 끊김 처리가 완료됨
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체</param>
		private void ServerTestClient_OnDisconnectCompleted(Client sender)
		{
			//끊김 처리 완료
		}
		/// <summary>
		/// 메시지 수신 준비가 완료됨.
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체</param>
		private void ServerTestClient_OnReceiveReady(Client sender)
		{
			DisplayMsg("메시지 수신 준비");
		}
		/// <summary>
		/// 메시지가 수신됨.
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체</param>
		/// <param name="byteMessage"></param>
		private void ServerTestClient_OnMessaged(Client sender, byte[] byteMessage)
		{
			DisplayMsg("메시지 : " + ChatSetting.ByteArrayToString(byteMessage));
		}
		#endregion

		/// <summary>
		/// 메뉴 - 서버 - 접속
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsmiConnect_Click(object sender, EventArgs e)
		{
			GloblaStatic.Client.ConnectServer();
		}
		/// <summary>
		/// 메뉴 - 서버 - 메시지 보내기
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsmiSendMessage_Click(object sender, EventArgs e)
		{
			GloblaStatic.Client.Send(ChatSetting.StringToByteArray(txtMsg.Text));
		}
		#endregion

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
						UI_Setting(typeState.Connecting);

						string nIP = "127.0.0.1";
						int nPort = Convert.ToInt32(txtPort.Text);

						//클라이언트 개체 생성
						GloblaStatic.Client = new Client(nIP, nPort);
						GloblaStatic.Client.OnConnectionComplete += Client_OnConnectionComplete;
						GloblaStatic.Client.OnDisconnect += Client_OnDisconnect;
						GloblaStatic.Client.OnDisconnectCompleted += Client_OnDisconnectCompleted;
						GloblaStatic.Client.OnReceiveReady += Client_OnReceiveReady;
						GloblaStatic.Client.OnMessaged += Client_OnMessaged;

						DisplayMsg("서버 준비 완료");

						//서버 접속 시작
						GloblaStatic.Client.ConnectServer();
					}
					break;

				case typeState.Connect://접속 상태
									   //이상태에서는 메시지를 보낸다.
					this.SendMsg(ChatCommandType.Msg, txtMsg.Text);
					this.txtMsg.Text = "";
					break;
			}
		}

		#region 클라이언트 이벤트 콜백
		/// <summary>
		/// 서버 연결 완료
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체</param>
		private void Client_OnConnectionComplete(Client sender)
		{
			DisplayMsg("*** 서버 연결 성공 ***");

			//서버에서 유저가 준비될때까지 로그인을 할수 없다.
			//일단 대기
		}

		/// <summary>
		/// 서버와 끊김처리가 시작됨
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체</param>
		private void Client_OnDisconnect(Client sender)
		{
			//접속 끊김
			GloblaStatic.Client = null;

			//유아이를 세팅하고
			UI_Setting(typeState.None);

			DisplayMsg("*** 서버 연결 끊김 ***");
		}
		/// <summary>
		/// 서버와 끊김 처리가 완료됨
		/// </summary>
		/// <param name="sender">이 이벤트를 발생시킨 개체(개체가 null일수 있다.)</param>
		private void Client_OnDisconnectCompleted(Client sender)
		{
			//끊김 처리 완료
		}

		/// <summary>
		/// 메시지 수신 준비가 완료됨
		/// </summary>
		/// <param name="sender"></param>
		private void Client_OnReceiveReady(Client sender)
		{
			//DisplayMsg("메시지 수신 준비");
		}

		/// <summary>
		/// 서버로 부터 메시지가 수신됨.
		/// <para>서버로 부터 온 메시지를 해석하여 처리한다.</para>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		private void Client_OnMessaged(Client sender, byte[] byteMessage)
		{
			//구분자로 명령을 구분 한다.
			string[] sData = GloblaStatic.ChatCmd.ChatCommandCut(byteMessage);

			//데이터 개수 확인
			if ((1 <= sData.Length))
			{
				//0이면 빈메시지이기 때문에 별도의 처리는 없다.

				//넘어온 명령
				ChatCommandType typeCommand
					= GloblaStatic.ChatCmd.StrIntToType(sData[0]);

				switch (typeCommand)
				{
					case ChatCommandType.None:   //없다
						break;
					case ChatCommandType.Msg:    //메시지인 경우
						Command_Msg(sData[1]);
						break;
					case ChatCommandType.ID_Check:    //아이디 체크 시도
													  //로그인 요청
						this.Login();
						break;
					case ChatCommandType.ID_Check_Ok:    //아이디 성공
						SendMeg_IDCheck_Ok();
						break;
					case ChatCommandType.ID_Check_Fail:  //아이디 실패
						SendMeg_IDCheck_Fail();
						break;
					case ChatCommandType.User_Connect:   //다른 유저가 접속 했다.
						SendMeg_User_Connect(sData[1]);
						break;
					case ChatCommandType.User_Disonnect: //다른 유저가 접속을 끊었다.
						SendMeg_User_Disconnect(sData[1]);
						break;
					case ChatCommandType.User_List:  //유저 리스트 갱신
						SendMeg_User_List(sData[1]);
						break;
				}
			}
		}
		#endregion

		#region 전달받은 명령 처리
		/// <summary>
		/// 메시지 출력
		/// </summary>
		/// <param name="sMsg"></param>
		private void Command_Msg(string sMsg)
		{
			DisplayMsg(sMsg);
		}

		/// <summary>
		/// 아이디 성공
		/// </summary>
		private void SendMeg_IDCheck_Ok()
		{
			this.Invoke(new Action(
				delegate ()
				{
					labID.Text = txtMsg.Text;
					txtMsg.Text = "";
				}));

			//UI갱신
			UI_Setting(typeState.Connect);

			//아이디확인이 되었으면 서버에 로그인 요청을 하여 로그인을 끝낸다.
			SendMsg(ChatCommandType.Login, "");
		}

		/// <summary>
		/// 아이디체크 실패
		/// </summary>
		private void SendMeg_IDCheck_Fail()
		{
			DisplayMsg("로그인 실패 : 다른 아이디를 이용해 주세요.");
			//연결 끊기
			GloblaStatic.Client.Disconnect(true);
		}

		/// <summary>
		/// 접속한 유저가 있다.
		/// </summary>
		private void SendMeg_User_Connect(string sUserID)
		{
			this.Invoke(new Action(
				delegate ()
				{
					this.DisplayMsg(String.Format("*** 다른 유저 접속 : {0} ***", sUserID));
					listUser.Items.Add(sUserID);
				}));
		}

		/// <summary>
		/// 접속을 끊은 유저가 있다.
		/// </summary>
		/// <param name="sUserID"></param>
		private void SendMeg_User_Disconnect(string sUserID)
		{
			this.Invoke(new Action(
				delegate ()
				{
					this.DisplayMsg(String.Format("*** 다른 유저 끊김 : {0} ***", sUserID));
					listUser.Items.RemoveAt(listUser.FindString(sUserID));
				}));
		}

		/// <summary>
		/// 유저리스트 
		/// </summary>
		/// <param name="sUserList"></param>
		private void SendMeg_User_List(string sUserList)
		{
			this.Invoke(new Action(
				delegate ()
				{
					//리스트를 비우고
					listUser.Items.Clear();

					//리스트를 다시 채워준다.
					string[] sList = sUserList.Split(',');
					for (int i = 0; i < sList.Length; ++i)
					{
						listUser.Items.Add(sList[i]);
					}
				}));
		}

		#endregion


		/// <summary>
		/// 아이디 체크 요청
		/// </summary>
		private void Login()
		{
			this.SendMsg(ChatCommandType.ID_Check, txtMsg.Text);
		}

		/// <summary>
		/// 연결 끊기 요청
		/// </summary>
		private void Logout()
		{
			SendMsg(ChatCommandType.Logout, "");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <param name="sMessage"></param>
		public void SendMsg(
			ChatCommandType typeChatCommand
			, string sMessage)
		{
			string sToss
				= GloblaStatic.ChatCmd
					.ChatCommandString(
						typeChatCommand
						, sMessage);

			GloblaStatic.Client.Send(ChatSetting.StringToByteArray(sToss));
		}


		/// <summary>
		/// UI 세팅
		/// </summary>
		/// <param name="typeSet"></param>
		private void UI_Setting(typeState typeSet)
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
		private void DisplayMsg(string nMessage)
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
							listMsg.Items.Add(buffer.ToString());
						}));

		}

		/// <summary>
		/// 폼이 닫치기 전에 발생
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (null != GloblaStatic.Client)
			{
				GloblaStatic.Client.Disconnect(true);
			}
		}
	}
}
