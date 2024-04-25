using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DG_SocketAssist4.Global.ReceiveAssists
{
    public class ReceiveAssist
    {
        /// <summary>
        /// 리시브 데이터 임시보관할 버퍼
        /// </summary>
        private ReceiveBufferTemp m_ReceiveBuffer = new ReceiveBufferTemp();

        /// <summary>
        /// 리시브를 통해 넘어온 데이터를 버퍼에 저장하고 헤더에 작성된 크기만큼 데이터를 잘라 리턴한다.
        /// <para>데이터가 모자르거나 문제가 있으면 byte[0]이 리턴된다.</para>
        /// </summary>
        /// <remarks>
        /// SocketAsyncEventArgs.Completed 안에서 사용해야 한다.
        /// </remarks>
        /// <param name="e"></param>
        /// <returns>헤더가 제거된 데이터 영역(지정된 크기 만큼의 바이트 개수)</returns>
        public byte[] ReceiveDataCheck(SocketAsyncEventArgs e)
        {
            byte[] byteReturn = new byte[0];

            if (1 <= e.BytesTransferred)
            {//데이터가 1이라도 들어왔다.

                //임시 버퍼에 데이터 추가
                this.m_ReceiveBuffer.Add(e.Buffer, e.BytesTransferred);

                byteReturn = this.m_ReceiveBuffer.FirstSizeData_Int();
            }

            return byteReturn;
        }


    }
}
