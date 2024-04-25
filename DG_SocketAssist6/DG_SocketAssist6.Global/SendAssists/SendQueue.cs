
namespace DG_SocketAssist4.Global.SendAssists;

/// <summary>
/// 보내기 큐 구현
/// <para>SocketAsyncEventArgs가 사용중이라면 대기할 요청</para>
/// </summary>
internal class SendQueue
{
    /// <summary>
	/// 샌드 요청이 끝났는지 여부
	/// </summary>
	/// <remarks>
	/// 외부용이다.<br />
	/// 샌드 요청이 시작될때 true,
	/// 끝날때 false로 넣어준다.
	/// </remarks>
	internal bool Used { get; set; } = false;

    /// <summary>
    /// 다음 요청이 들어있는 큐
    /// </summary>
    private Queue<byte[]> Send = new Queue<byte[]>();

    /// <summary>
    /// 큐에 남아있는 데이터 수
    /// </summary>
    internal int Count
    {
        get
        {
            return Send.Count;
        }
    }

    /// <summary>
    /// 큐에 추가할 패턴. 완성된 패턴을 넣는다.
    /// </summary>
    /// <param name="byteSendData"></param>
    internal void Add(byte[] byteSendData)
    {
        if (0 < byteSendData.Length)
        {//데이터가 있다.
            this.Send.Enqueue(byteSendData);
        }
    }

    /// <summary>
    /// 큐의 맨앞에 잇는 데이터를 추출한다. 없으면 byte[0]가 리턴됨
    /// </summary>
    /// <returns></returns>
    internal byte[] Get()
    {
        byte[] byteReturn = new byte[0];

        if (0 < Send.Count)
        {
            byteReturn = this.Send.Dequeue();
        }

        return byteReturn;
    }
}

