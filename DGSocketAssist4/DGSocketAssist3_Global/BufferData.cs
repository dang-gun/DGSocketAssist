using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGSocketAssist3_Global
{
	/// <summary>
	/// 서버와 클라이언트가 주고/받는 바이트 어레이 데이터를 처리하기위한 클래스<br />
	/// 이 개체를 전달하지말고 'Buffer'만 전달하여 사용한다.
	/// <para>이 클래스는 오로지 바이트 어레이 데이터만 처리한다.<br />
	/// 문자열과 같은 다른 데이터 형식으로 바꾸려면 다른 곳에서 하도록 구현한다.</para>
	/// <para>이 개체를 주고받아도 문제는 없지만 효율상 'Buffer'만 전달하는게 좋아보인다.</para>
	/// </summary>
	public class BufferData
	{
		/// <summary>
		/// 확보된 버퍼<br />
		/// 데이터가 들어 있는 영역이다.
		/// </summary>
		public byte[] Buffer { get; set; }

		/// <summary>
		/// 확보된 버퍼의 크기
		/// </summary>
		public int Length 
		{
			get
			{
				int nLength = 0;
				if (null != Buffer)
				{//버퍼가 생성되어 있다.

					//버퍼 크기 리턴
					nLength = Buffer.Length; ;
				}

				return nLength;
			}
		}	

		/// <summary>
		/// 버퍼를 최대 크기로 생성하고 개체를 초기화 한다.
		/// </summary>
		public BufferData()
		{
			this.Buffer = new byte[SettingData.BufferFullSize];
		}

		/// <summary>
		/// 들어온 데이터 만큼 버퍼를 확보하여 들어온 데이터를 저장한다.
		/// </summary>
		/// <param name="byteData"></param>
		public BufferData(byte[] byteData)
		{
			this.Buffer = byteData;
		}
	}
}
