using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestConsole
{
    /// <summary>
    /// 이 클래스는 SocketAsyncEventArgs.UserToken 속성에 할당할 개체로 
    /// 사용하도록 설계되었습니다.
    /// <para>최신버전 슈퍼소켓에는 이 것을 사용하지 않고 StreamAsyncResult를 전달한다.<br />
    /// 이 프로젝트에서는 MSDN의 내용을 가능한 그대로 따라가기 위해 예전 코드를 그대로 사용한다.
    /// </para>
    /// <see href="https://github.com/mdavid/SuperSocket/blob/master/v1.4/SocketEngine/AsyncSocket/AsyncUserToken.cs">SuperSocket/v1.4/SocketEngine/AsyncSocket/AsyncUserToken.cs</see>
    /// </summary>
    class AsyncUserToken
    {
        Socket m_socket;

        public AsyncUserToken() : this(null) { }

        public AsyncUserToken(Socket socket)
        {
            m_socket = socket;
        }

        public Socket Socket
        {
            get { return m_socket; }
            set { m_socket = value; }
        }
    }
}
