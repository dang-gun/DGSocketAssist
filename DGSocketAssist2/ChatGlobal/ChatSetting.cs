using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGlobal
{
	/// <summary>
	/// 서버/클라이언트간 같이 사용할 데이터
	/// </summary>
	public class ChatSetting
	{
		/// <summary>
		/// 제목
		/// </summary>
		public static string SiteTitle = "Socket - SocketAsyncEventArgs";

		/// <summary>
		/// 명령어 구분용 문자
		/// </summary>
		public static char Delimeter1 = '▦';

		/// <summary>
		/// 바이너리 데이터를 문자열로 바꾼다.
		/// </summary>
		/// <param name="byteData">바꿀 데이터</param>
		/// <returns></returns>
		public static string ByteArrayToString(byte[] byteData)
		{
			return Encoding.UTF8.GetString(byteData);
		}
		/// <summary>
		/// 문자열을 바이너리 데이터로 바꾼다.
		/// </summary>
		/// <param name="sData">바꿀 데이터</param>
		/// <returns></returns>
		public static byte[] StringToByteArray(string sData)
		{
			return Encoding.UTF8.GetBytes(sData);
		}
	}
}
