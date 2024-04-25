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


public class KeepAliveSetting
{
    /// <summary>
    /// KeepAlive설정값을 리턴한다.
    /// </summary>
    /// <returns></returns>
    public byte[] KeepAliveSetting_Btye()
    {
        //KeepAlive 설정
        byte[] keepAlive = new byte[12];

        //keepalive 기능 켜기
        Buffer.BlockCopy(
            BitConverter.GetBytes((uint)1), 0
                                    , keepAlive, 0, 4);
        //keepalive 확인 시간
        Buffer.BlockCopy(
            BitConverter.GetBytes(SettingData.TcpKeepAliveTime), 0
                                , keepAlive, 4, 4);
        //keepalive 확인 간격
        Buffer.BlockCopy(
            BitConverter.GetBytes(SettingData.TcpKeepAliveInterval), 0
                                , keepAlive, 8, 4);
        return keepAlive;
    }

    /// <summary>
    /// 소켓을 전달받아 KeepAlive 세팅을 해준다.
    /// </summary>
    /// <remarks>
    /// .NET 6 이상에서만 사용
    /// </remarks>
    /// <param name="socket"></param>
    public void KeepAliveSetting_Net6(Socket socket)
    {
        //닷넷을 통한 KeepAlive설정은 .NET Core 3.0이상에서만 지원한다.
        //https://learn.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketoptionname?view=netcore-3.0#system-net-sockets-socketoptionname-tcpkeepalivetime
        socket.SetSocketOption(
            SocketOptionLevel.Socket
            , SocketOptionName.KeepAlive
            , true);
        socket.SetSocketOption(
            SocketOptionLevel.Tcp
            , SocketOptionName.TcpKeepAliveTime
            , SettingData.TcpKeepAliveTime);
        socket.SetSocketOption(
            SocketOptionLevel.Tcp
            , SocketOptionName.TcpKeepAliveInterval
            , SettingData.TcpKeepAliveInterval);
        
        //Windows 10 version 1703 or later
        //https://learn.microsoft.com/en-us/windows/win32/winsock/ipproto-tcp-socket-options?WT.mc_id=DT-MVP-4038148
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            && Environment.OSVersion.Version >= new Version(10, 0, 15063))
        {//윈도우10, 1703 이후 버전(windows server 2019)

            //TcpKeepAliveRetryCount를 사용할 수 있다.
            socket.SetSocketOption(
                SocketOptionLevel.Tcp
                , SocketOptionName.TcpKeepAliveRetryCount
                , SettingData.TcpKeepAliveRetryCount);

        }
    }
}