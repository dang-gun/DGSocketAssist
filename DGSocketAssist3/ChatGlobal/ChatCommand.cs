using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGlobal
{
	/// <summary>
	/// 서버/클라이언트간 통신에 사용할 명령어
	/// </summary>
	public enum ChatCommandType
	{
		/// <summary>
		/// 기본 상태
		/// </summary>
		None = 0,

		/// <summary>
		/// 아이디 체크
		/// </summary>
		ID_Check,
		/// <summary>
		/// 아이디 체크 성공
		/// </summary>
		ID_Check_Ok,
		/// <summary>
		/// 아이디 체크 실패
		/// </summary>
		ID_Check_Fail,

		/// <summary>
		/// 접속한 유저가 있다.
		/// </summary>
		User_Connect,
		/// <summary>
		/// 접속을 끊은 유저가 있다.
		/// </summary>
		User_Disonnect,
		/// <summary>
		/// 유저 리스트를 보냅니다.
		/// </summary>
		User_List,
		/// <summary>
		/// 유저 리스트를 갱신을 요청 합니다.
		/// </summary>
		User_List_Get,

		/// <summary>
		/// 아이디 무셜성이 확인 된후 호출함
		/// </summary>
		Login,
		/// <summary>
		/// 서버에서 모든 로그인 과정이 완료 되었다고 클라이언트에게 알림
		/// </summary>
		Login_Complete,
		/// <summary>
		/// 로그아웃
		/// </summary>
		Logout,

		/// <summary>
		/// 메시지 전송
		/// </summary>
		Msg,
	}

	/// <summary>
	/// 체팅 명령어 처리 유틸
	/// </summary>
	public class ChatCommand
	{
		/// <summary>
		/// 숫자 관련 지원
		/// </summary>
		private NumberAssist m_insNumber = new NumberAssist();

		/// <summary>
		/// 문자열로된 숫자를 명령어 타입으로 바꿔줍니다.
		/// 입력된 문자열이 올바르지 않다면 기본상태를 줍니다.
		/// </summary>
		/// <param name="sData"></param>
		/// <returns></returns>
		public ChatCommandType StrIntToType(string sData)
		{
			//넘어온 명령
			ChatCommandType typeCommand = ChatCommandType.None;

			if (true == m_insNumber.IsNumeric(sData))
			{
				//입력된 명령이 숫자라면 명령 타입으로 변환한다.
				//입력된 명령이 숫자가 아니면 명령 없음 처리(기본값)를 한다.
				typeCommand = (ChatCommandType)Convert.ToInt32(sData);
			}

			return typeCommand;
		}

		/// <summary>
		/// 채팅에 사용할 명령어 구조를 문자열로 만들어 리턴한다.
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <param name="sMessage"></param>
		/// <returns></returns>
		public string ChatCommandString(
			ChatCommandType typeChatCommand
			, string sMessage)
		{
			StringBuilder sReturn = new StringBuilder();

			sReturn.Append(typeChatCommand.GetHashCode());
			sReturn.Append(ChatSetting.Delimeter1);
			sReturn.Append(sMessage);

			return sReturn.ToString();
		}

		/// <summary>
		/// 채팅에 사용할 명령어 구조를 바이트로 만들어 리턴한다.
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <param name="sMessage"></param>
		/// <returns></returns>
		public byte[] ChatCommandByte(
			ChatCommandType typeChatCommand
			, string sMessage)
		{
			string sTemp = this.ChatCommandString(typeChatCommand, sMessage);

			return ChatSetting.StringToByteArray(sTemp);
		}


		/// <summary>
		/// 채팅에 사용할 명령어 구조를 구분자로 잘라 리턴한다.
		/// </summary>
		/// <param name="sMessage"></param>
		/// <returns></returns>
		public string[] ChatCommandCut(string sMessage)
		{
			//구분자로 명령을 구분 한다.
			return sMessage.Split(ChatSetting.Delimeter1);
		}

		/// <summary>
		/// 바이너리 정보를 문자열로 바꾼후 채팅에 사용할 명령어 구조로 잘라서 리턴한다.
		/// </summary>
		/// <param name="byteMessage"></param>
		/// <returns></returns>
		public string[] ChatCommandCut(byte[] byteMessage)
		{
			return this.ChatCommandCut(ChatSetting.ByteArrayToString(byteMessage));
		}
	}
}
