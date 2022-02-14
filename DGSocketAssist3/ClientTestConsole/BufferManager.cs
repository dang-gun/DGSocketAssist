using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestConsole
{
	/// <summary>
	/// 이 클래스는 분할할 수 있는 단일 대형 버퍼를 생성하고 각 버퍼와 함께 사용하기 위해 SocketAsyncEventArgs 개체에 할당합니다.<br />
	/// 소켓 I/O 작업.<br />
	/// 이렇게 하면 버퍼를 쉽게 재사용할 수 있고 힙 메모리 조각화를 방지할 수 있습니다.
	/// <see href="https://docs.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketasynceventargs.setbuffer?view=net-6.0">SocketAsyncEventArgs.SetBuffer 메서드</see>
	/// </summary>
	public class BufferManager
	{
		/// <summary>
		/// 버퍼 풀이 제어하는 총 바이트 수
		/// </summary>
		int m_numBytes;
		/// <summary>
		/// 버퍼 관리자가 유지 관리하는 기본 바이트 배열
		/// </summary>
		byte[] m_buffer;
		Stack<int> m_freeIndexPool;
		int m_currentIndex;
		int m_bufferSize;

		public BufferManager(int totalBytes, int bufferSize)
		{
			m_numBytes = totalBytes;
			m_currentIndex = 0;
			m_bufferSize = bufferSize;
			m_freeIndexPool = new Stack<int>();
		}

        /// <summary>
        /// 버퍼 풀에서 사용하는 버퍼 공간 할당
        /// </summary>
        public void InitBuffer()
        {
            //하나의 큰 큰 버퍼를 만들고 이를 각 SocketAsyncEventArg 개체로 나눕니다.
            m_buffer = new byte[m_numBytes];
        }

        /// <summary>
        /// 버퍼 풀의 버퍼를 지정된 SocketAsyncEventArgs 개체에 할당합니다.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>버퍼가 성공적으로 설정되면 true, 그렇지 않으면 false</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {

            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        /// <summary>
        /// SocketAsyncEventArg 개체에서 버퍼를 제거합니다.<br />
        /// 이것은 버퍼를 버퍼 풀로 다시 해제합니다.
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
