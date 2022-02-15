using DGU.DGU_ByteAssist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGSocketAssist2_Global
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
		/// <para>이 데이터는 원본 데이터로 헤더 여부는 알수 없다.</para>
		/// </summary>
		public byte[] Buffer { get; set; }

		/// <summary>
		/// Buffer에 헤더가 있는지 여부
		/// </summary>
		public bool IsHeader { get; private set; }

		/// <summary>
		/// 헤더로 부터 전달된 버퍼의 크기.<br />
		/// 헤더가 포함안된 크기인다.
		/// </summary>
		public Int32 BufferSize { get; private set; }

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
					nLength = Buffer.Length;
				}

				return nLength;
			}
		}


		/// <summary>
		/// 버퍼를 최대 크기로 생성하고 개체를 초기화 한다.
		/// </summary>
		public BufferData()
		{
			this.IsHeader = false;
			this.Buffer = new byte[SettingData.BufferFullSize];
		}


		/// <summary>
		/// 들어온 데이터 만큼 버퍼를 확보하여 들어온 데이터를 저장한다.
		/// </summary>
		/// <param name="byteData"></param>
		/// <param name="bHeader">헤더가 포함된 데이터인지 여부</param>
		public BufferData(byte[] byteData, bool bHeader)
		{
			this.IsHeader = bHeader;
			this.Buffer = byteData;
		}

		/// <summary>
		/// 가지고 있는 데이터의 크기를 계산해 헤더를 붙여준다.
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		public void AddHeader()
		{
			if (false == this.IsHeader)
			{
				//데이터 길이 저장
				this.BufferSize = this.Buffer.Length;

				//설정된 공간 만큼 생성
				byte[] byteReturn = new byte[SettingData.BufferHeaderSize];

				//길이정보를 바이트로 변환해 설정된 공간에 저장
				ByteArray.Copy_All(
					out byteReturn
					, BitConverter.GetBytes(this.BufferSize));

				//길이정보를 데이터 앞에 붙여준다.
				this.Buffer = ByteArray.Combine(byteReturn, this.Buffer);

				//버퍼에 헤더가 있음을 알린다.
				this.IsHeader = true;
			}
			else
			{
				throw new ArgumentException("헤더가 있을때 헤더를 붙이면 안됩니다.");
			}
		}


		/// <summary>
		/// 가지고 있는 데이터에서 헤더를 잘라내서 헤더 정보는 BufferSize에 저장하고
		/// 헤더를 제거한 데이터는 다시 Buffer에 저장한다.
		/// <para>주의해야할 점은 this.Buffer를 확보하는 시점이 실데이터가 오는 시점이 아니라
		/// 공간만 만드는 시점이기 때문에 데이터를 전달 받고 나서 이 메소드를 호출해야 한다.</para>
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		public void CutHeader()
		{
			if (true == this.IsHeader)
			{
				List<byte[]> listCut
					= ByteArray.Cut_Left(this.Buffer, SettingData.BufferHeaderSize);

				//데이터 사이즈 계산
				this.BufferSize = BitConverter.ToInt32(listCut[0], 0);
				this.Buffer = listCut[1];
			}
			else
			{
				throw new ArgumentException("헤더가 없을때 헤더를 잘라내면 안됩니다.");
			}
		}

		/// <summary>
		/// 가지고 있는 BufferSize의 크기보다 많은 Buffer는 버린다.
		/// </summary>
		public void CutBody()
		{
			this.Buffer = ByteArray.Get_Left(this.Buffer, this.BufferSize);
		}

	}
}
