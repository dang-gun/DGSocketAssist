using System;

namespace DG_SocketAssist4.Global.Faculty;

/// <summary>
/// 버퍼 데이터모델
/// </summary>
public class BufferDataModel
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
                nLength = Buffer.Length;
            }

            return nLength;
        }
    }

    /// <summary>
    /// 버퍼를 최대 크기로 생성하고 개체를 초기화 한다.
    /// </summary>
    public BufferDataModel()
    {
        this.Buffer = new byte[SettingData.BufferFullSize];
    }

    /// <summary>
    /// 들어온 데이터 만큼 버퍼를 확보하여 들어온 데이터를 저장한다.
    /// </summary>
    /// <param name="byteData"></param>
    public BufferDataModel(byte[] byteData)
    {
        this.Buffer = byteData;
    }

    /// <summary>
    /// 버퍼의 내용을 초기화 시킨다.
    /// </summary>
    public void Clear()
    {
        Array.Clear(this.Buffer, 0, this.Buffer.Length);
    }
}
