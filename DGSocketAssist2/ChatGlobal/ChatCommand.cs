using DGU.DGU_ByteAssist;
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
		MsgSend,
		/// <summary>
		/// 파일 전송
		/// </summary>
		FileSend,
	}

	/// <summary>
	/// 체팅 명령어 처리 유틸
	/// </summary>
	public class ChatCommand
	{
		/// <summary>
		/// 체팅 명령을 체팅헤더로 바꾼다.
		/// <para>체팅 명령을 설정된 크기만큼의 byte로 변환하여 리턴한다.</para>
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <returns></returns>
		public byte[] ChatCommandToChatHeader(ChatCommandType typeChatCommand)
		{
			byte[] byteReturn = new byte[ChatSetting.ChatCommandSize];
			ByteArray.Copy_All(
				out byteReturn
				, BitConverter.GetBytes(Convert.ToInt16(typeChatCommand)));

			return byteReturn;
		}

		/// <summary>
		/// 문자열로된 명령을 변환한다.
		/// <para>체팅명령은 체팅 헤더로 변환하고 문자열을 바이트어레이로 변환한다.</para>
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <param name="sData">전달할 문자열</param>
		/// <returns></returns>
		public byte[] ChatString(
			ChatCommandType typeChatCommand
			, string sData)
		{
			//체팅명령은 체팅 헤더로 변환
			byte[] byteChatHeader = this.ChatCommandToChatHeader(typeChatCommand);
			//문자열 데이터를 바이너리로 변환
			byte[] byteData = ChatSetting.StringToByteArray(sData);
			//두개를 합쳐서 리턴
			return ByteArray.Combine(byteChatHeader, byteData);
		}

		/// <summary>
		/// 바이너리로된 명령을 변환 한다.
		/// <para>체팅명령은 체팅 헤더로 변환하고 뒤에 데이터를 붙인다.</para>
		/// </summary>
		/// <param name="typeChatCommand"></param>
		/// <param name="sData">전달할 데이터</param>
		/// <returns></returns>
		public byte[] ChatString(
			ChatCommandType typeChatCommand
			, byte[] sData)
		{
			//체팅명령은 체팅 헤더로 변환
			byte[] byteChatHeader = this.ChatCommandToChatHeader(typeChatCommand);
			//두개를 합쳐서 리턴
			return ByteArray.Combine(byteChatHeader, sData);
		}


		/// <summary>
		/// 데이터에서 체팅 헤더를 잘라내어 체팅명령어로 바꾸고
		/// 남은 데이터를 전달한다.
		/// </summary>
		/// <param name="sData"></param>
		/// <returns></returns>
		public ChatCommandType ChatHeaderToChatCommand(ref byte[] sData)
		{
			ChatCommandType typeReturn = ChatCommandType.None;

			List<byte[]> listCut
				= ByteArray.Cut_Left(sData, ChatSetting.ChatCommandSize);
			sData = listCut[1];

			//명령어 변환
			typeReturn = (ChatCommandType)BitConverter.ToInt16(listCut[0], 0);

			return typeReturn;
		}
	}
}
