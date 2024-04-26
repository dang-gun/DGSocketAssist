using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Timers;

namespace DG_SocketAssist6.Global.Faculty;

/// <summary>
/// 접속이 유지되고 있는지 확인하기위한 동작
/// </summary>
/// <remarks>
/// KeepAlive로 인한 끊김도 직접 send/receive를 하지 않으면 알 수 없다<br />
/// 그러므로 일정시간동안 send/receive가 없으면 send를 하는 동작을 해주는 모델이다.
/// </remarks>
public class KeepAliveChecker
{
    private System.Timers.Timer timer;

    private Action ActionSend;

    public KeepAliveChecker(Action action)
    {
        this.ActionSend = action;

        this.timer = new System.Timers.Timer();
        this.timer.Interval = 5000;

        this.timer.Elapsed += Timer_Elapsed;
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if(null != this.ActionSend)
        {
            this.ActionSend();
        }
    }

    /// <summary>
    /// 타이머 다시 시작
    /// </summary>
    public void TimerReset()
    {
        this.timer.Stop();
        this.timer.Start();
    }
}

/// <summary>
/// KeepAlive에 사용할 기본값
/// </summary>
/// <remarks>
/// 한번만 쓰고 버릴 데이터라 전역 변수(static)로 선언하지 않으려고 이렇게 구현함
/// </remarks>
public class KeepAlive_DefultValue
{
    //연결 유지 확인 시간
    public readonly uint TcpKeepAliveTime = 1;
    /// <summary>
    /// 연결 유지 확인 시간(ms)
    /// </summary>
    public readonly uint TcpKeepAliveTime_ms;
    /// <summary>
    /// 연결 유지 확인 시간(ms) - int
    /// </summary>
    public readonly int TcpKeepAliveTime_ms_int;


    /// <summary>
    /// 연결 유지 확인 간격
    /// </summary>
    public readonly uint TcpKeepAliveInterval = 1;
    /// <summary>
    /// 연결 유지 확인 간격(ms)
    /// </summary>
    public readonly uint TcpKeepAliveInterval_ms;
    /// <summary>
    /// 연결 유지 확인 간격(ms) - int
    /// </summary>
    public readonly int TcpKeepAliveInterval_ms_int;

    /// <summary>
    /// 연결 유지 실패시 반복 확인 횟수
    /// <para>끊김시 이 횟수만큼 반복해서 확인하고 연결이 복원되지 않으면 끊김처리가 된다.</para>
    /// </summary>
    public readonly int TcpKeepAliveRetryCount = 5;

    public KeepAlive_DefultValue()
    {
        this.TcpKeepAliveTime_ms = TcpKeepAliveTime * 1000;
        this.TcpKeepAliveTime_ms_int = Convert.ToInt32(TcpKeepAliveTime) * 1000;

        this.TcpKeepAliveInterval_ms = TcpKeepAliveInterval * 1000;
        this.TcpKeepAliveInterval_ms_int = Convert.ToInt32(TcpKeepAliveInterval) * 1000;
    }
}


/// <summary>
/// KeepAlive Setting 지원
/// </summary>
/// <remarks>
/// 이 프로젝트는 .NET 6 이상에서 사용한다는 가정을 하고 작성되어 있다.
/// <para>구현은 윈도우를 기준으로 되어 있다.</para>
/// </remarks>
public class KeepAliveSetting
{
    public void KeepAliveSetting_All(Socket socket)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            && Environment.OSVersion.Version < new Version(10, 0, 15063))
        {//윈도우 10, 1709이하에서만 사용

            this.KeepAliveSetting_Btye(socket);
        }
        else
        {
            this.KeepAliveSetting_Net6(socket);
        }

    }

    /// <summary>
    /// KeepAlive설정값을 설정한다.
    /// <para>윈도우 전용</para>
    /// <para>윈도우 10, 1709이하에서만 사용</para>
    /// <para>.NET Core 3.0이하에서만 사용</para>
    /// </summary>
    /// <returns></returns>
    public void KeepAliveSetting_Btye(Socket socket)
    {


        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            && Environment.OSVersion.Version < new Version(10, 0, 15063))
        {
            KeepAlive_DefultValue defultValue = new KeepAlive_DefultValue();

            //KeepAlive 설정
            byte[] keepAlive = new byte[12];

            //keepalive 기능 켜기
            Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);
            //keepalive 확인 시간
            Buffer.BlockCopy(BitConverter.GetBytes(defultValue.TcpKeepAliveTime_ms), 0, keepAlive, 4, 4);
            //keepalive 확인 간격
            Buffer.BlockCopy(BitConverter.GetBytes(defultValue.TcpKeepAliveInterval_ms), 0, keepAlive, 8, 4);

            //keepalive설정 적용
            socket.IOControl(
                IOControlCode.KeepAliveValues
                , keepAlive
                , null);

            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, keepAlive);
        }

    }

    /// <summary>
    /// 소켓을 전달받아 KeepAlive 세팅을 해준다.
    /// </summary>
    /// <remarks>
    /// .NET 6 이상에서만 사용
    /// <para>https://learn.microsoft.com/ko-kr/windows/win32/winsock/ipproto-tcp-socket-options#windows-support-for-ipproto_tcp-options</para>
    /// </remarks>
    /// <param name="socket"></param>
    public void KeepAliveSetting_Net6(Socket socket)
    {
        KeepAlive_DefultValue defultValue = new KeepAlive_DefultValue();

        //닷넷을 통한 KeepAlive설정은 .NET Core 3.0이상에서만 지원한다.
        //https://learn.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketoptionname?view=netcore-3.0#system-net-sockets-socketoptionname-tcpkeepalivetime
        socket.SetSocketOption(
            SocketOptionLevel.Socket
            , SocketOptionName.KeepAlive
            , true);

        //TCP_KEEPALIVE or TCP_KEEPIDLE
        socket.SetSocketOption(
            SocketOptionLevel.Tcp
            , SocketOptionName.TcpKeepAliveTime
            , defultValue.TcpKeepAliveTime_ms_int);
        //TCP_KEEPINTVL
        socket.SetSocketOption(
            SocketOptionLevel.Tcp
            , SocketOptionName.TcpKeepAliveInterval
            , defultValue.TcpKeepAliveInterval_ms_int);
        
        //Windows 10 version 1703 or later
        //https://learn.microsoft.com/en-us/windows/win32/winsock/ipproto-tcp-socket-options?WT.mc_id=DT-MVP-4038148
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            && Environment.OSVersion.Version >= new Version(10, 0, 15063))
        {//윈도우10, 1703 이후 버전(windows server 2019)

            //TcpKeepAliveRetryCount를 사용할 수 있다.
            //TCP_KEEPCNT
            socket.SetSocketOption(
                SocketOptionLevel.Tcp
                , SocketOptionName.TcpKeepAliveRetryCount
                , defultValue.TcpKeepAliveRetryCount);

        }
    }
}