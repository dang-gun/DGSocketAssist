using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGSocketAssist2_Global
{
	/// <summary>
	/// 서버와 클라이언트가 주고/받는 데이터 구조체
	/// </summary>
	public class SettingData
	{
		/// <summary>
		/// 버퍼의 정보가 들어있는 헤더의 크기
		/// <para>헤더의 크기는 byte의 자리수와 같다.<br />
		/// <list type="table">
		///		<listheader>
		///			<term>크기(byte)</term>
		///			<description>표현 범위</description>
		///		</listheader>
		///		<item>
		///			<term>2</term>
		///			<description>Int16</description>
		///		</item>
		///		<item>
		///			<term>4</term>
		///			<description>Int32</description>
		///		</item>
		///		<item>
		///			<term>8</term>
		///			<description>Int64</description>
		///		</item>
		///</list>
		/// </para>
		/// </summary>
		public static int BufferHeaderSize = 4;

		/// <summary>
		/// 소켓이 한번에 받을 수 있는 최대 버퍼 크기.<br />
		/// SocketAsyncEventArgs를 생성할때 사용되는 버퍼 크기이다.
		/// </summary>
		public static int BufferFullSize = 8192;
	}
}
