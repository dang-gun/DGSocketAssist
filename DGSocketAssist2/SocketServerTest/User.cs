using ChatGlobal;
using DGSocketAssist2_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static SocketServerTest.ServerForm;

namespace SocketServerTest
{
	/// <summary>
	/// 리스트로 관리될 유저 모델
	/// </summary>
	public class User
	{
		#region 외부에 연결될 이벤트
		/// <summary>
		/// 유저 준비
		/// </summary>
		/// <param name="sender"></param>
		public delegate void UserReadyDelegate(User sender);
		/// <summary>
		/// 유저 준비
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
		/// 유저 메시지 요청
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void MessageDelegate(User sender, MessageEventArgs e);
		/// <summary>
		/// 메시지
		/// </summary>
		public event MessageDelegate OnMessaged;

		/// <summary>
		/// 유저 로그인 완료
		/// </summary>
		/// <param name="sender"></param>
		public delegate void LoginCompletDelegate(User sender);
		/// <summary>
		/// 유저 로그인 완료되면 발생함
		/// </summary>
		public event LoginCompletDelegate OnLoginComplet;

		/// <summary>
		/// 클라이언트 끊김 처리 완료
		/// </summary>
		/// <param name="sender"></param>
		public delegate void DisconnectCompletedDelegate(User sender);
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
		/// 이 유저의 아이디
		/// </summary>
		public string UserID { get; set; }


		/// <summary>
		/// 유저 객체를 생성합니다.
		/// </summary>
		/// <param name="newClientListener">접속된 클라이언트의 리스너 개체</param>
		public User(ClientListener newClientListener)
		{
			//소켓 저장
			this.ClientListenerMe = newClientListener;
			
			//메시지 분석 연결
			this.ClientListenerMe.OnMessaged += ClientListenerMe_OnMessaged;
            //끊김 이벤트 연결
            this.ClientListenerMe.OnDisconnectCompleted += ClientListenerMe_OnDisconnectCompleted;

			//유저 준비를 알림
			this.UserReadyCall();

			//아이디 체크를 시작하라고 알림
			this.SendMsg_User(ChatCommandType.ID_Check, new byte[0]);
		}

        private void ClientListenerMe_OnDisconnectCompleted(ClientListener sender)
        {
			DisconnectCompletedCall();
		}

        private void ClientListenerMe_OnMessaged(
			ClientListener sender
			, byte[] byteMessage)
		{
			//명령어 분리
			byte[] byteBodyData = byteMessage;
			ChatCommandType typeCommand
				= GloblaStatic.ChatCmd
					.ChatHeaderToChatCommand(ref byteBodyData);

			switch (typeCommand)
			{
				case ChatCommandType.None://없다
					break;
				
				case ChatCommandType.MsgSend://메시지인 경우
					SendMeg_Main(ChatSetting.ByteArrayToString(byteBodyData), typeCommand);
					break;
				case ChatCommandType.FileSend://파일 전송
					MessageEventArgs e = new MessageEventArgs(byteBodyData, typeCommand);
					this.OnMessaged(this, e);
					break;

				case ChatCommandType.ID_Check://아이디 체크
					SendMeg_Main(ChatSetting.ByteArrayToString(byteBodyData), typeCommand);
					break;

				case ChatCommandType.User_List_Get://유저리스트 갱신 요청
					SendMeg_Main("", typeCommand);
					break;

				case ChatCommandType.Login://로그인 완료
					OnLoginComplet(this);
					break;
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

			OnMessaged(this, e);
		}

		/// <summary>
		/// 이 유저에게 체팅 명령 문자열을 만들어 메시지를 보낸다.
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <param name="byteData"></param>
		public void SendMsg_User(
			ChatCommandType typeChatCommand
			, byte[] byteData)
		{
			byte[] byteToss
				= GloblaStatic.ChatCmd.ChatString(
					typeChatCommand
					, byteData);
			this.ClientListenerMe.Send(byteToss);
		}

		/// <summary>
		/// 이 유저에게 메시지를 보낸다.
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <param name="sMsg"></param>
		public void SendMsg_User(ChatCommandType typeChatCommand, string sMsg)
		{
			//명령 만들기
			byte[] byteSendData
				= GloblaStatic.ChatCmd
					.ChatString(
						typeChatCommand
						, sMsg);

			this.ClientListenerMe.Send(byteSendData);
		}

		/// <summary>
		/// 이 유저에게 메시지를 보낸다.<br />
		/// 이미 체팅명령 헤더가 붙은 경우 이것으로 메시지를 보낸다.
		/// </summary>
		/// <param name="byteData"></param>
		public void SendMsg_User(byte[] byteData)
		{
			this.ClientListenerMe.Send(byteData);
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
