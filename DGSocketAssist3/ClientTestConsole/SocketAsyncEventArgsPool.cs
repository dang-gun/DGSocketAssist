using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestConsole
{
    /// <summary>
    /// 재사용 가능한 SocketAsyncEventArgs 개체 컬렉션을 나타냅니다.
    /// <see href="https://docs.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketasynceventargs.-ctor?view=net-6.0">SocketAsyncEventArgs 생성자</see>
    /// </summary>
	public class SocketAsyncEventArgsPool
	{
        /// <summary>
        /// 이 풀이 가지고 있는 개체 리스트
        /// </summary>
        private Stack<SocketAsyncEventArgs> m_pool;

        /// <summary>
        /// 개체 풀을 지정된 크기로 초기화합니다.<br />
        /// </summary>
        /// <param name="capacity">풀이 보유할 수 있는 최대 SocketAsyncEventArgs 개체 수입니다.</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// SocketAsyncEventArg 인스턴스를 풀에 추가합니다.
        /// </summary>
        /// <param name="item">풀에 추가할 SocketAsyncEventArgs 인스턴스입니다.</param>
        /// <exception cref="ArgumentNullException">SocketAsyncEventArgsPool에 추가된 항목은 null일 수 없습니다.</exception>
        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) 
            { 
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null"); 
            }

            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        /// <summary>
        /// 풀에서 SocketAsyncEventArgs 인스턴스를 제거하고 풀에서 제거된 개체를 반환합니다.
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        /// <summary>
        /// 풀의 SocketAsyncEventArgs 인스턴스 수
        /// </summary>
        public int Count
        {
            get { return m_pool.Count; }
        }
    }
}
