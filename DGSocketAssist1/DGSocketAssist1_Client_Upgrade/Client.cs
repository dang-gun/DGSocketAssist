﻿using DGSocketAssist1_Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DGSocketAssist1_Client
{
    /// <summary>
    /// 서버연결에 사용하는 클라이언트.
    /// </summary>
    public class Client
    {
        #region 클라이언트 이벤트
        /// <summary>
        /// 서버 접속 완료
        /// </summary>
        /// <param name="sender"></param>
        public delegate void ConnectionCompleteDelegate(Client sender);
        /// <summary>
        /// 서버 접속 완료
        /// </summary>
        /// <remarks>
        /// 로그인 처리와 같은 무결성 검사가 끝나서 서버에 진입한(혹은 가능한) 
        /// 상태가 되면 발생한다.
        /// </remarks>
        public event ConnectionCompleteDelegate OnConnectionComplete;
        /// <summary>
        /// 서버 접속 완료를 외부에 알림
        /// </summary>
        private void ConnectionCompleteCall()
        {
            if (null != OnConnectionComplete)
            {
                this.OnConnectionComplete(this);
            }
        }

        /// <summary>
        /// 클라이언트 끊김 처리가 시작되었음을 알린다.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void DisconnectDelegate(Client sender);
        /// <summary>
        /// 클라이언 끊김 처리가 시작되었음을 알린다.
        /// <para>클라이언트가 어떤 사유에서든 끊겼음을 의미한다.</para>
        /// <para>정상 종료라면 서버에서 먼저 메시지를 보내 직접 끊는 것이 좋다.</para>
        /// </summary>
        public event DisconnectDelegate OnDisconnect;
        /// <summary>
        /// 클라이언트 끊김 처리가 시작되었음을 외부에 알림
        /// </summary>
        private void DisconnectCall()
        {
            if (null != OnDisconnect)
            {
                this.OnDisconnect(this);
            }
        }

        /// <summary>
        /// 클라이언트 끊김 처리 완료
        /// </summary>
        /// <param name="sender"></param>
        public delegate void DisconnectCompletedDelegate(Client sender);
        /// <summary>
        /// 클라이언트가 끊김처리가 완료 되었다.
        /// </summary>
        public event DisconnectCompletedDelegate OnDisconnectCompleted;
        /// <summary>
        /// 클라이언트 끊김 처리 완료되었음을 외부에 알림
        /// </summary>
        private void DisconnectCompletedCall()
        {
            if (null != OnDisconnectCompleted)
            {
                this.OnDisconnectCompleted(this);
            }
        }

        /// <summary>
        /// 데이터 수신 준비 완료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public delegate void ReceiveReadyDelegate(Client sender);
        /// <summary>
        /// 데이터 수신 준비 완료.
        /// </summary>
        /// <remarks>
        /// <para>서버와 정상 연결되고 메시지 수신이 준비되면 발생한다.</para>
        /// <para>서버의 메시지를 한번 다받고 나면 다시 수신 준비상태가 되므로.</para>
        /// <para>이때도 발생한다.</para>
        /// </remarks>
        public event ReceiveReadyDelegate OnReceiveReady;
        /// <summary>
        /// 데이터 수신 준비 완료되었음을 외부에 알림
        /// </summary>
        private void ReceiveReadyCall()
        {
            if (null != OnReceiveReady)
            {
                this.OnReceiveReady(this);
            }
        }

        /// <summary>
        /// 메시지가 수신
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public delegate void MessagedDelegate(Client sender, string message);
        /// <summary>
        /// 메시지가 수신됨
        /// </summary>
        public event MessagedDelegate OnMessaged;
        /// <summary>
        /// 메시지가 수신 되었음을 외부에 알림
        /// </summary>
        private void MessagedCall(string message)
        {
            if (null != OnMessaged)
            {
                this.OnMessaged(this, message);
            }
        }
        #endregion

        /// <summary>
        /// 이 클라이언트가 연결된 Socket
        /// </summary>
        public Socket SocketMe { get; private set; }

        /// <summary>
        /// 서버로 전송용 SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs m_saeaSend = null;
        /// <summary>
        /// 수신용 SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs m_saeaReceive = null;

        /// <summary>
        /// 서버 주소
        /// </summary>
        public IPEndPoint ServerIP { get; private set; }

        /// <summary>
        /// 서버와 연결할 클라이언트 생성. 
        /// </summary>
        /// <param name="sIP">서버 ip</param>
        /// <param name="nPort">서버 포트</param>
        public Client(string sIP, int nPort)
        {
            this.SocketSetting(
                new IPEndPoint(
                    IPAddress.Parse(sIP)
                    , nPort));
        }
        /// <summary>
        /// 서버와 연결할 클라이언트 생성. 
        /// </summary>
        /// <param name="address">서버 주소</param>
        public Client(IPEndPoint address)
        {
            this.SocketSetting(address);
        }

        /// <summary>
        /// 이 개체가 사용할 소켓을 생성한다.
        /// </summary>
        /// <param name="ip">서버 주소</param>
        private void SocketSetting(IPEndPoint ip)
        {
            //소켓 생성
            SocketMe
                = new Socket(AddressFamily.InterNetwork
                    , SocketType.Stream
                    , ProtocolType.Tcp);
            this.ServerIP = ip;

            //전송용 SocketAsyncEventArgs 세팅
            this.m_saeaSend = new SocketAsyncEventArgs();
            this.m_saeaSend.RemoteEndPoint = this.ServerIP;
            this.m_saeaSend.Completed -= SaeaSend_Completed;
            this.m_saeaSend.Completed += SaeaSend_Completed;

            //수신용 SocketAsyncEventArgs 세팅
            this.m_saeaReceive = new SocketAsyncEventArgs();
            this.m_saeaReceive.RemoteEndPoint = this.ServerIP;
            this.m_saeaReceive.SetBuffer(new Byte[SettingData.BufferFullSize], 0, SettingData.BufferFullSize);
            this.m_saeaReceive.Completed -= SaeaReceive_Completed;
            this.m_saeaReceive.Completed += SaeaReceive_Completed;
        }

        /// <summary>
        /// 서버에 연결시도를 한다.
        /// </summary>
        public void ConnectServer()
        {
            //접속용 SocketAsyncEventArgs를 생성
            SocketAsyncEventArgs saeaConnect = new SocketAsyncEventArgs();
            saeaConnect.RemoteEndPoint = this.ServerIP;
            //연결 완료 이벤트 연결
            saeaConnect.Completed -= SaeaConnect_Completed;
            saeaConnect.Completed += SaeaConnect_Completed;

            Debug.WriteLine("서버 연결 중");
            //서버 메시지 대기
            this.SocketMe.ConnectAsync(saeaConnect);
        }

        /// <summary>
        /// 연결 완료 이벤트에 연결됨
        /// <para>서버에 연결되었음에만 사용하는 이벤트이다.</para>
        /// </summary>
        /// <param name="sender">호출한 개체</param>
        /// <param name="e">SocketAsync 이벤트</param>
        private void SaeaConnect_Completed(object sender, SocketAsyncEventArgs e)
        {
            this.SocketMe = (Socket)sender;

            if (true == this.SocketMe.Connected)
            {
                MessageData mdReceiveMsg = new MessageData();

                //서버에 수신대기할 개체를 설정한다.
                //보낼 데이터를 설정하고
                this.m_saeaReceive.UserToken = mdReceiveMsg;


                //.NET5 부터는 ReceiveAsync 상황에 따라서 동기/비동기로 돌아간다.
                //이 부분은 별도의 스래드를 만들어 사용했더니 가끔 데이터를 받지 못하는현상이 일어났다.
                //그래서 별도 스래드 처리를 제거하였다.
                //첫 메시지 받기 준비 
                if (false == this.SocketMe.ReceiveAsync(this.m_saeaReceive))
                {
                    this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
                }
                this.ReceiveReadyCall();


                Debug.WriteLine("서버 연결 성공");
                //서버 연결 성공을 알림
                this.ConnectionCompleteCall();
            }
            else
            {
                //접속 끊김을 알린다.
                Disconnect(true);
            }
        }


        /// <summary>
        /// 수신 완료 이벤트 연결됨
        /// <para>실제 데이터를 수신받는 이벤트이다.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaeaReceive_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket socketClient = (Socket)sender;
            MessageData mdRecieveMsg = (MessageData)e.UserToken;
            mdRecieveMsg.SetLength(e.Buffer);
            mdRecieveMsg.InitData();

            if (true == socketClient.Connected)
            {
                //연결이 되어 있다.

                //데이터 수신
                socketClient.Receive(
                    mdRecieveMsg.Data
                    , mdRecieveMsg.DataLength
                    , SocketFlags.None);

                //메시지 수신을 알림
                this.MessagedCall(mdRecieveMsg.GetData());

                Debug.WriteLine("다음 데이터 받을 준비 ");

                //다음 메시지를 받을 준비를 한다.
                if (false == socketClient.ReceiveAsync(e))
                {
                    this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
                }
                this.ReceiveReadyCall();
            }
            else
            {
                //접속 끊김을 알린다.
                Disconnect(true);
            }
        }


        /// <summary>
        /// 연결된 이 서버로 메시지를 전송 한다.
        /// </summary>
        /// <param name="sMsg"></param>
        public void Send(string sMsg)
        {
            MessageData mdSendMsg = new MessageData();

            //데이터를 넣고
            mdSendMsg.SetData(sMsg);



            //데이터 길이 세팅
            this.m_saeaSend.SetBuffer(BitConverter.GetBytes(mdSendMsg.DataLength), 0, 4);
            //보낼 데이터 설정
            this.m_saeaSend.UserToken = mdSendMsg;



            //.NET5 부터는 SendAsync가 상황에 따라서 동기/비동기로 돌아간다.
            if (false == this.SocketMe.SendAsync(this.m_saeaSend))
            {
                this.SaeaSend_Completed(this.SocketMe, this.m_saeaSend);
            }

        }

        /// <summary>
        /// 전송 완료 이벤트 연결됨.
        /// <para>'Send'에서 전송이 시작되면 이 곳에서 마무리 한다.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaeaSend_Completed(object sender, SocketAsyncEventArgs e)
        {
            Socket socketSend = (Socket)sender;
            MessageData mdMsg = (MessageData)e.UserToken;
            //데이터 보내기 마무리
            socketSend.Send(mdMsg.Data);
        }

        /// <summary>
        /// 연결을 끊는다.
        /// <para>bEvent를 true로 사용하는 경우 무한루프에 빠질수 있으니 조심해야 한다.</para>
        /// </summary>
        /// <param name="bEvent">연결끊김 이벤트 발생 여부.</param>
        public void Disconnect(bool bEvent)
        {
            if (true == bEvent)
            {
                this.DisconnectCall();
            }

            if (null != this.SocketMe)
            {
                this.SocketMe.Close();
                this.SocketMe = null;
            }

            if (true == bEvent)
            {
                this.DisconnectCompletedCall();
            }
        }

        /// <summary>
        /// 연결을 끊는다.
        /// <para>외부용 - 이벤트 발생 안함.</para>
        /// </summary>
        public void Disconnect()
        {
            this.Disconnect(false);
        }

    }
}