using System.Net;
using System.Net.Sockets;

using DG_SocketAssist6.Global;
using DG_SocketAssist6.Global.Faculty;
using DG_SocketAssist6.Global.ReceiveAssists;
using DG_SocketAssist6.Global.SendAssists;

namespace DG_SocketAssist6.Client;

/// <summary>
/// 서버연결에 사용하는 클라이언트.
/// <para>클라이언트 입장에서 서버에 접속하기위한 클라이언트</para>
/// </summary>
public class ClientSocket
{
    #region 클라이언트 이벤트
    /// <summary>
    /// 로그용 대리자
    /// </summary>
    /// <param name="nLogType">로그 성격</param>
    /// <param name="sMessage"></param>
    public delegate void LogDelegate(int nLogType, string sMessage);
    /// <summary>
    /// 로그 발생 이벤트
    /// </summary>
    public event LogDelegate? OnLog;
    /// <summary>
    /// 로그 발생 이벤트 호출
    /// </summary>
    /// <param name="nLogType"></param>
    /// <param name="sMessage"></param>
    private void OnLogCall(int nLogType, string sMessage)
    {
        if (null != this.OnLog)
        {
            this.OnLog(nLogType, sMessage);
        }
    }

    /// <summary>
    /// 서버 접속 완료
    /// </summary>
    /// <param name="sender"></param>
    public delegate void ConnectionCompleteDelegate(ClientSocket sender);
    /// <summary>
    /// 서버 접속 완료
    /// </summary>
    /// <remarks>
    /// 로그인 처리와 같은 무결성 검사가 끝나서 서버에 진입한(혹은 가능한) 
    /// 상태가 되면 발생한다.
    /// </remarks>
    public event ConnectionCompleteDelegate? OnConnectionComplete;
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
    public delegate void DisconnectDelegate(ClientSocket sender);
    /// <summary>
    /// 클라이언 끊김 처리가 시작되었음을 알린다.
    /// <para>클라이언트가 어떤 사유에서든 끊겼음을 의미한다.</para>
    /// <para>정상 종료라면 서버에서 먼저 메시지를 보내 직접 끊는 것이 좋다.</para>
    /// </summary>
    public event DisconnectDelegate? OnDisconnect;
    /// <summary>
    /// 클라이언트 끊김 처리가 시작되었음을 외부에 알림
    /// </summary>
    private void DisconnectCall()
    {
        this.DisconnectEventIs = true;

        if (null != OnDisconnect)
        {
            this.OnDisconnect(this);
        }
    }

    /// <summary>
    /// 클라이언트 끊김 처리 완료
    /// </summary>
    /// <param name="sender"></param>
    public delegate void DisconnectCompletedDelegate(ClientSocket sender);
    /// <summary>
    /// 클라이언트가 끊김처리가 완료 되었다.
    /// </summary>
    public event DisconnectCompletedDelegate? OnDisconnectCompleted;
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
    public delegate void ReceiveReadyDelegate(ClientSocket sender);
    /// <summary>
    /// 데이터 수신 준비 완료.
    /// </summary>
    /// <remarks>
    /// <para>서버와 정상 연결되고 메시지 수신이 준비되면 발생한다.</para>
    /// <para>서버의 메시지를 한번 다받고 나면 다시 수신 준비상태가 되므로.</para>
    /// <para>이때도 발생한다.</para>
    /// </remarks>
    public event ReceiveReadyDelegate? OnReceiveReady;
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
    /// <param name="byteData"></param>
    public delegate void MessagedDelegate(ClientSocket sender, byte[] byteData);
    /// <summary>
    /// 메시지가 수신됨
    /// </summary>
    public event MessagedDelegate? OnMessaged;
    /// <summary>
    /// 메시지가 수신 되었음을 외부에 알림
    /// </summary>
    private void MessagedCall(byte[] byteData)
    {
        if (null != OnMessaged)
        {
            this.OnMessaged(this, byteData);
        }
    }
    #endregion

    /// <summary>
    /// 이 클라이언트가 연결된 Socket
    /// </summary>
    public Socket SocketMe { get; private set; }
    /// <summary>
    /// 연결된 SocketMe의 IP
    /// </summary>
    public string Ip
    {
        get
        {
            return (new SocketInfo()).ToIp(this.SocketMe);
        }
    }

    /// <summary>
    /// 서버로 전송용 SocketAsyncEventArgs
    /// </summary>
    private SocketAsyncEventArgs m_saeaSend;
    /// <summary>
    /// 서버 전송시 m_saeaSend가 사용중일때 처리해주는 큐
    /// </summary>
    private SendAssist m_SendAssist = new SendAssist();

    /// <summary>
    /// 수신용 SocketAsyncEventArgs
    /// </summary>
    private SocketAsyncEventArgs m_saeaReceive;
    /// <summary>
    /// 리시브 데이터 임시보관할 버퍼
    /// </summary>
    private ReceiveAssist m_ReceiveBuffer = new ReceiveAssist();

    /// <summary>
    /// 서버 주소
    /// </summary>
    public IPEndPoint ServerIP { get; private set; }

    /// <summary>
    /// 클라이언트 끊어짐 이벤트가 발생했었는지 여부
    /// </summary>
    /// <remarks>
    /// 이 클래스는 클라이언트가 끊어지면 제거되므로 OnDisconnect이벤트는 한번만 발생해야 한다.
    /// <para>OnDisconnect이벤트가 여러번 발생하면 외부에서는 제거된 개체를 제거하려는 문제가 생길 수 있다.</para>
    /// <para>이 문제를 방지하기위한 변수로 OnDisconnect가 한번이라도 발생하면 true로 변경된다. </para>
    /// </remarks>
    private bool DisconnectEventIs = false;

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
    /// <summary>
    /// 서버와 연결할 클라이언트 생성. 
    /// </summary>
    /// <param name="sIP">서버 ip</param>
    /// <param name="nPort">서버 포트</param>

    public ClientSocket(string sIP, int nPort)
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
    public ClientSocket(IPEndPoint address)
    {
        this.SocketSetting(address);
    }
#pragma warning restore CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.

    /// <summary>
    /// 이 개체가 사용할 소켓을 생성한다.
    /// </summary>
    /// <param name="ip">서버 주소</param>
    private void SocketSetting(IPEndPoint ip)
    {
        //소켓 생성
        this.SocketMe
            = new Socket(AddressFamily.InterNetwork
                , SocketType.Stream
                , ProtocolType.Tcp);
        this.ServerIP = ip;

        //keepalive설정 적용
        //(new KeepAliveSetting()).KeepAliveSetting_Net6(this.SocketMe);
        (new KeepAliveSetting()).KeepAliveSetting_All(this.SocketMe);

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

        this.OnLogCall(0, "서버 연결 중");
        //서버 메시지 대기
        if (false == this.SocketMe.ConnectAsync(saeaConnect))
        {
            this.SaeaConnect_Completed(this.SocketMe, saeaConnect);
        };
    }

    /// <summary>
    /// 연결 완료 이벤트에 연결됨
    /// <para>서버에 연결되었음에만 사용하는 이벤트이다.</para>
    /// </summary>
    /// <param name="sender">호출한 개체</param>
    /// <param name="e">SocketAsync 이벤트</param>
    private void SaeaConnect_Completed(object? sender, SocketAsyncEventArgs e)
    {
        if (true == this.SocketMe.Connected)
        {
            BufferDataModel mdReceiveMsg = new BufferDataModel();

            //서버에 수신대기할 개체를 설정한다.
            //보낼 데이터를 설정하고
            this.m_saeaReceive.SetBuffer(mdReceiveMsg.Buffer, 0, mdReceiveMsg.Length);

            //.NET5 부터는 ReceiveAsync 상황에 따라서 동기/비동기로 돌아간다.
            //이 부분은 별도의 스래드를 만들어 사용했더니 가끔 데이터를 받지 못하는현상이 일어났다.
            //그래서 별도 스래드 처리를 제거하였다.
            //첫 메시지 받기 준비 
            if (false == this.SocketMe.ReceiveAsync(this.m_saeaReceive))
            {
                this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
            }
            this.ReceiveReadyCall();

            this.OnLogCall(0, "서버 연결 성공");
            //서버 연결 성공을 알림
            this.ConnectionCompleteCall();
        }
        else
        {
            //접속 끊김을 알린다.
            this.Disconnect();
        }
    }


    /// <summary>
    /// 수신 완료 이벤트 연결됨
    /// <para>실제 데이터를 수신받는 이벤트이다.</para>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaeaReceive_Completed(object? sender, SocketAsyncEventArgs e)
    {

        if (true == this.SocketMe.Connected)
        {//연결이 되어 있다.

            //버퍼에서 데이터가 완성되었는지 확인한다.
            byte[] byteReceiveCompleteData
                = this.m_ReceiveBuffer.ReceiveDataCheck(e);

            if (0 < byteReceiveCompleteData.Length)
            {//완성된 데이터가 있다.

                //수신된 버퍼 만큼 확보
                BufferDataModel bdRecieveMsg
                    = new BufferDataModel(byteReceiveCompleteData);
                //메시지 수신을 알림
                this.MessagedCall(bdRecieveMsg.Buffer);
            }


            this.OnLogCall(0, "다음 데이터 받을 준비 ");
            //다음 메시지를 받을 준비를 한다.
            if (false == this.SocketMe.ReceiveAsync(e))
            {
                this.SaeaReceive_Completed(this.SocketMe, this.m_saeaReceive);
            }
            this.ReceiveReadyCall();
        }
        else
        {
            //접속 끊김을 알린다.
            this.Disconnect();
        }
    }


    /// <summary>
    /// 연결된 이 서버로 메시지를 전송 한다.
    /// </summary>
    /// <param name="byteData"></param>
    public void Send(byte[] byteData)
    {
        this.m_SendAssist.SendCheck(byteData
            , delegate (byte[] byteMsg_Send)
            {
                //버퍼 데이터를 만들고
                BufferDataModel bdSendMsg = new BufferDataModel(byteMsg_Send);

                this.OnLogCall(0, "Send : 메시지 보내기 시작 : " + byteMsg_Send);
                //데이터 넣기
                this.m_saeaSend.SetBuffer(bdSendMsg.Buffer, 0, bdSendMsg.Length);
                //보내기 시작
                if (false == this.SocketMe.SendAsync(this.m_saeaSend))
                {
                    this.SaeaSend_Completed(this.SocketMe, this.m_saeaSend);
                }
            });
    }

    /// <summary>
    /// 전송 완료 이벤트 연결됨.
    /// <para>'Send'에서 전송이 시작되면 이 곳에서 마무리 한다.</para>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SaeaSend_Completed(object? sender, SocketAsyncEventArgs e)
    {

        this.OnLogCall(0, "SaeaSend_Completed : 메시지 보내기 완료");

        //큐 사용이 끝남을 알림
        this.m_SendAssist.SendCheckCompleted();
        //보낸게 완료되었으니 다음 큐를 진행 시킨다.
        this.Send(new byte[0]);
    }

    /// <summary>
    /// 연결을 끊는다.
    /// </summary>
    public void Disconnect()
    {
        if (false == this.DisconnectEventIs)
        {
            this.DisconnectCall();
        }

        if (null != this.SocketMe)
        {
            this.SocketMe.Close();
        }

        if (false == this.DisconnectEventIs)
        {
            this.DisconnectCompletedCall();
        }
    }

}
