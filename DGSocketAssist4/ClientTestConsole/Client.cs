using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientTestConsole
{
	/// <summary>
	/// 서버연결에 사용하는 클라이언트.
	/// </summary>
	public class Client
	{
		private int bufferSize = 60000;
		private const int MessageHeaderSize = 4;

		private Socket clientSocket;
		private IPEndPoint hostEndPoint;

		private SocketAsyncEventArgs sendEventArgs;
		private SocketAsyncEventArgs receiveEventArgs;

        /// <summary>
        /// 개체를 생성한다.
        /// </summary>
        /// <param name="hostEndPoint"></param>
        public Client(IPEndPoint hostEndPoint)
        {
            //ip 저장
            this.hostEndPoint = hostEndPoint;

            //접속용 소켓 생성
            this.clientSocket 
                = new Socket(
                    this.hostEndPoint.AddressFamily
                    , SocketType.Stream
                    , ProtocolType.Tcp);

            //전송용 SocketAsyncEventArgs 세팅
            this.sendEventArgs = new SocketAsyncEventArgs();
            //this.sendEventArgs.UserToken = this.clientSocket;
            this.sendEventArgs.UserToken = new AsyncUserToken(clientSocket);
            this.sendEventArgs.RemoteEndPoint = this.hostEndPoint;
            this.sendEventArgs.Completed 
                += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            //수신용 SocketAsyncEventArgs 세팅
            this.receiveEventArgs = new SocketAsyncEventArgs();
            this.receiveEventArgs.UserToken = new AsyncUserToken(clientSocket);
            this.receiveEventArgs.RemoteEndPoint = this.hostEndPoint;
            this.receiveEventArgs.SetBuffer(new Byte[bufferSize], 0, bufferSize);
            this.receiveEventArgs.Completed 
                += new EventHandler<SocketAsyncEventArgs>(OnReceiveCompleted);
        }

        /// <summary>
        /// 서버에 접속 시도
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public void Connect()
        {
            //접속용 SocketAsyncEventArgs를 생성
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            //생성한 소켓을 연결하고 
            connectArgs.UserToken = this.clientSocket;
            connectArgs.RemoteEndPoint = this.hostEndPoint;
            connectArgs.Completed 
                += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);

            //연결 시작
            clientSocket.ConnectAsync(connectArgs);
            

            SocketError errorCode = connectArgs.SocketError;
            if (errorCode != SocketError.Success)
            {
                throw new SocketException((Int32)errorCode);
            }


            //수신 대기
            if (!clientSocket.ReceiveAsync(receiveEventArgs))
            {//접속 후 수신 성공
                ProcessReceive(receiveEventArgs);
            }
        }

        public void Disconnect()
        {
            clientSocket.Disconnect(false);
        }

        public void Send(byte[] message)
        {
            sendEventArgs.SetBuffer(message, 0, message.Length);
            clientSocket.SendAsync(sendEventArgs);
        }

        /// <summary>
        /// 연결되었다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("접속 성공");
        }

        /// <summary>
        /// 메시지 전송 완료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("전송 성공");
        }

        /// <summary>
        /// 메시지 수신 완료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                AsyncUserToken token = e.UserToken as AsyncUserToken;

                //수신 데이터 처리
                 

                //다음 수신 데이터의 시작 위치와 수신 데이터의 버퍼에서 수신할 수 있는
                //최대 데이터 길이 업데이트
                e.SetBuffer(e.Buffer, 0, e.Buffer.Length);

                //후속 데이터 수신
                if (!token.Socket.ReceiveAsync(e))
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                //ProcessError(e);
            }
        }
    }
}
