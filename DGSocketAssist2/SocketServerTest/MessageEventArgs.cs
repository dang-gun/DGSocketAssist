using ChatGlobal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerTest
{
	/// <summary>
	/// 메시지 이벤트용 형식입니다.
	/// </summary>
	public class MessageEventArgs : EventArgs
	{
		/// <summary>
		/// 문자열
		/// </summary>
		public readonly string DataString = "";

		/// <summary>
		/// 바이트 데이터
		/// </summary>
		public readonly byte[] DataByte;

		/// <summary>
		/// 메시지 타입
		/// </summary>
		public ChatCommandType m_typeCommand = ChatCommandType.None;

		/// <summary>
		/// 메시지 설정
		/// </summary>
		/// <param name="strMsg"></param>
		/// <param name="typeCommand"></param>
		public MessageEventArgs(string strMsg, ChatCommandType typeCommand)
		{
			this.DataString = strMsg;
			this.m_typeCommand = typeCommand;
		}

		/// <summary>
		/// 바이너리 데이터 설정
		/// </summary>
		/// <param name="byteData"></param>
		/// <param name="typeCommand"></param>
		public MessageEventArgs(byte[] byteData, ChatCommandType typeCommand)
		{
			this.DataByte = byteData;
			this.m_typeCommand = typeCommand;
		}
	}
}
