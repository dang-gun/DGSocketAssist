﻿using System.Net;
using System.Net.Sockets;

using DG_SocketAssist6.Global;
using DG_SocketAssist6.Global.Faculty;

namespace DG_SocketAssist6.Server;

/// <summary>
/// 클라이언트의 접속을 기다리는 서버
/// </summary>
/// <remarks>
/// 클라이언트의 접속과 끊김은 여기서만 처리한다.
/// </remarks>
public class ServerSocket
{
    #region 서버 이벤트
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
    /// 서버가 시작됨
    /// </summary>
    public event EmptyDelegate? OnStart;
    /// <summary>
    /// 서버 시작됨을 외부에 알림
    /// </summary>
    private void OnStartCall()
    {
        if (null != this.OnStart)
        {
            this.OnStart();
        }
    }

    /// <summary>
    /// 서버가 멈춤
    /// </summary>
    public event EmptyDelegate? OnStop;
    /// <summary>
    /// 서버 멈춤을 외부에 알림
    /// </summary>
    private void OnStopCall()
    {
        if (null != this.OnStop)
        {
            this.OnStop();
        }
    }
    #endregion

    #region 클라이언트 이벤트
    /// <summary>
    /// 클라이언트 접속 성공
    /// </summary>
    /// <param name="sender"></param>
    public delegate void OnConnectedDelegate(ClientModel sender);
    /// <summary>
    /// 클라이언트 접속함.
    /// <para>무결성 검사가 아직 진행되지 않은 접속.</para>
    /// </summary>
    public event OnConnectedDelegate? OnConnected;
    /// <summary>
    /// 클라이언트 접속을 외부에 알림
    /// </summary>
    /// <param name="sender"></param>
    private void ConnectedCall(ClientModel sender)
    {
        if (null != this.OnConnected)
        {
            this.OnConnected(sender);
        }
    }


    /// <summary>
    /// 클라이언트가 끊김 처리가 시작됨
    /// </summary>
    /// <param name="sender"></param>
    public delegate void DisconnectDelegate(ClientModel sender);
    /// <summary>
    /// 클라이언트가 끊김 처리가 시작되었다.
    /// </summary>
    public event DisconnectDelegate? OnDisconnect;
    /// <summary>
    /// 클라이언트가 끊김 처리가 시작되었음을 외부에 알림
    /// </summary>
    /// <param name="sender"></param>
    private void OnDisconnectCall(ClientModel sender)
    {
        if (null != this.OnDisconnect)
        {
            this.OnDisconnect(sender);
        }
    }

    /// <summary>
    /// 클라이언트 끊김 처리 완료
    /// </summary>
    /// <param name="nClientIndex">호출한 개체가 가지고 있던 인덱스</param>
    public delegate void DisconnectCompletedDelegate(long nClientIndex);
    /// <summary>
    /// 클라이언트가 끊김처리가 완료 되었다.
    /// <para>끊김 처리가 완료되면 호출했던 ClientModel모델은 제거</para>
    /// </summary>
    public event DisconnectCompletedDelegate? OnDisconnectCompleted;
    /// <summary>
    /// 클라이언트 끊김 처리 완료되었음을 외부에 알림
    /// </summary>
    /// <param name="nClientIndex">호출한 개체가 가지고 있던 인덱스</param>
    private void OnDisconnectCompletedCall(long nClientIndex)
    {
        if (null != OnDisconnectCompleted)
        {
            this.OnDisconnectCompleted(nClientIndex);
        }
    }
    #endregion


    #region 클라이언트 유효성(validation) 검사용 함수 정의
    /// <summary>
    /// 접속된 클라이언트의 유효성(블랙리스트에 있는지 등등)을 검사하는 함수 대리자
    /// </summary>
    /// <param name="e">접속된 클라이언트의 소켓 이벤트 개체</param>
    /// <returns></returns>
    public delegate bool ClientConnectCheckDelegate(SocketAsyncEventArgs e);
    /// <summary>
    /// 유효성 검사에 사용할 함수
    /// </summary>
    public ClientConnectCheckDelegate? ClientConnectCheckFunc = null;
    /// <summary>
    /// 유효성 검사를 한다.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private bool ClientConnectCheckCall(SocketAsyncEventArgs e)
    {
        bool bReturn = false;

        if (null == this.ClientConnectCheckFunc)
        {//검사 함수가 없다.

            //무조건 통과
            bReturn = true;
        }
        else
        {
            bReturn = this.ClientConnectCheckFunc(e);
        }

        return bReturn;
    }
    #endregion

    /// <summary>
    /// 접속한 클라이언트 리스트
    /// </summary>
    public List<ClientModel> ClientList { get; private set; }

    /// <summary>
    /// 서버 소켓
    /// </summary>
    private Socket socketServer;

    /// <summary>
    /// 서버 생성
    /// </summary>
    /// <param name="nPort">사용할 포트</param>
    public ServerSocket(int nPort)
    {
        this.ClientList = new List<ClientModel>();

        //서버 세팅
        this.socketServer
        = new Socket(AddressFamily.InterNetwork
                    , SocketType.Stream
                    , ProtocolType.Tcp);


        //서버 ip 및 포트
        IPEndPoint ipServer
        = new IPEndPoint(IPAddress.Any, nPort);
        socketServer.Bind(ipServer);
    }

    /// <summary>
    /// 서버 시작.
    /// tcp 요청을 대기한다.
    /// </summary>
    public void Start()
    {
        this.OnLogCall(0, "서버 시작...");

        //수신 대기 시작
        //매개변수는 연결 대기 숫자.
        //.NET 5 이상에서는 자동으로 설정가능하다.
        //https://docs.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socket.listen?view=net-6.0
        //socketServer.Listen(40);
        socketServer.Listen();

        //keepalive설정 적용
        (new KeepAliveSetting()).KeepAliveSetting_Net6(this.socketServer);

        this.OnStartCall();
        this.OnLogCall(0, "클라이언트 접속 대기 : 시작");

        //클라이언트 연결시 사용될 SocketAsyncEventArgs
        SocketAsyncEventArgs saeaUser = new SocketAsyncEventArgs();
        //클라이언트가 연결되었을때 이벤트
        saeaUser.Completed -= this.ClientConnect_Completed;
        saeaUser.Completed += this.ClientConnect_Completed;

        //클라이언트 접속 대기 시작
        //첫 클라이언트가 접속되기 전까지 여기서 대기를 하게 된다.
        if (false == this.socketServer.AcceptAsync(saeaUser))
        {
            this.ClientConnect_Completed(this.socketServer, saeaUser);

        }

    }

    /// <summary>
    /// 클라이언트 접속 완료.
    /// <para>하나의 클라이언트가 접속했음을 처리한다.</para>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ClientConnect_Completed(object? sender, SocketAsyncEventArgs e)
    {
        this.OnLogCall(0, $"클라이언트 접속됨 : {(new SocketInfo()).ToIp(e.AcceptSocket)}");

        if (true == this.ClientConnectCheckCall(e)
            && null != e.AcceptSocket)
        {//성공

            //클라이언트 리스너 개체를 만든다.
            //여기서 하는 클라이언트 리스너의 처리는 접속과 끊김 뿐임으로
            //다른 기능은 외부에서 연결하여 사용해야 한다.
            ClientListener newUser = new ClientListener(e.AcceptSocket);

            //클라이언트 모델을 생성한다
            ClientModel newCM = new ClientModel(newUser);
            newCM.OnLog += NewCM_OnLog;

            newCM.OnDisconnect += NewCM_OnDisconnect;
            newCM.OnDisconnectCompleted += NewCM_OnDisconnectCompleted;

            //리스트에 클라이언트 추가
            this.ClientList.Add(newCM);

            //첫 메시지 대기
            newCM.FirstListening();

            //클라이언트 접속을 알림.
            this.ConnectedCall(newCM);
        }


        //다시 클라이언트 접속 대기 시작
        this.OnLogCall(0, "다음 클라이언트 접속 대기");

        //이렇게 구성하는 이유는 'Start'에서 무한 루프 없이
        //클라이언트 대기를 구현하기 위해서이다.
        //Socket socketServer = (Socket)sender;
        e.AcceptSocket = null;
        if (false == this.socketServer.AcceptAsync(e))
        {
            this.ClientConnect_Completed(this.socketServer, e);
        }
    }

    private void NewCM_OnLog(int nLogType, string sMessage)
    {
        this.OnLogCall(nLogType, "[ServerSocket]" + sMessage);
    }


    /// <summary>
    /// 클라이언트가 끊김 처리가 시작되었다.
    /// </summary>
    /// <param name="sender"></param>
    private void NewCM_OnDisconnect(ClientModel sender)
    {
        this.OnDisconnectCall(sender);
    }
    /// <summary>
    /// 클라이언트 끊김 처리가 완료됨
    /// </summary>
    /// <param name="sender"></param>
    private void NewCM_OnDisconnectCompleted(ClientModel sender)
    {
        //끊기전에 인덱스를 저장해 둔다.
        long nClientIndex = sender.ClientIndex;

        //연결이 끊긴 클라이언트를 제거한다.
        this.ClientList.Remove(sender);

        this.OnDisconnectCompletedCall(nClientIndex);
    }


    /// <summary>
    /// 서버를 정지 시킨다.
    /// </summary>
    public void Stop()
    {
        try
        {
            this.socketServer.Close();
        }
        catch (Exception ex)
        {
            this.OnLogCall(0, "서버 종료중 오류 : " + ex.Message);
        }

        this.OnStopCall();
        this.OnLogCall(0, "서버를 종료합니다.");
    }
}
