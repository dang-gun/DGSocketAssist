using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DG_SocketAssist4.Global.Faculty
{
    /// <summary>
    /// 소켓 관련 정보를 처리해준다.
    /// </summary>
    public class SocketInfo
    {
        public string ToIp(Socket socket)
        {
            string sReturn = string.Empty;

            if (null != socket
                && null != socket.RemoteEndPoint)
            {
                sReturn = ((IPEndPoint)socket.RemoteEndPoint).ToString();
            }

            return sReturn;
        }
    }
}


