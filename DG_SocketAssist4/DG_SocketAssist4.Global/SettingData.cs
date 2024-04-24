using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG_SocketAssist4.Global
{
	/// <summary>
	/// 서버와 클라이언트가 주고/받는 데이터 구조체
	/// </summary>
	public class SettingData
	{
        /// <summary>
        /// 버퍼의 정보가 들어있는 헤더의 크기를 타입으로 저장
        /// </summary>
        /// <remarks>
        /// 빠른 처리를 위해 타입과 숫자를 따로 저장한다.
        /// </remarks>
        public readonly static HeaderSizeType BufferHeaderSizeType = HeaderSizeType.Short;

        /// <summary>
        /// 버퍼의 정보가 들어있는 헤더의 크기
        /// <para>한개의 리시브에 필수로 사용되는 헤더의 크기</para>
        /// <para>2의 배수로 사용한다.</para>
        /// </summary>
        /// <remarks>
        /// 데이터를 전달받을때 최소한으로 필요한 데이터의 크기이다.
        /// <para>이 프로젝트에서는 2자리를 int로 바꾸어 리시브의 크기로 사용한다.</para>
        /// <para>여기에 표시된 크기는 헤더를 포함하지 않은 크기이다.</para>
        /// </remarks>
        public readonly static int BufferHeaderSize = BufferHeaderSizeType.GetHashCode();
        

		/// <summary>
		/// 소켓이 한번에 받을 수 있는 최대 버퍼 크기.<br />
		/// SocketAsyncEventArgs를 생성할때 사용되는 버퍼 크기이다.
		/// </summary>
		public readonly static int BufferFullSize = 8192;
        //public readonly static int BufferFullSize = 1024;

        /// <summary>
        /// 연결 유지 확인 시간
        /// </summary>
        public readonly static uint TcpKeepAliveTimeMilliseconds = 1000;
        /// <summary>
        /// 연결 유지 확인 간격
        /// </summary>
        public readonly static uint TcpKeepAliveIntervalMilliseconds = 1000;

        /// <summary>
        /// KeepAlive설정값을 리턴한다.
        /// </summary>
        /// <returns></returns>
        public static byte[] KeepAliveSetting()
        {
            //KeepAlive 설정
            byte[] keepAlive = new byte[12];

            //keepalive 기능 켜기
            Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);
            //keepalive 확인 시간
            Buffer.BlockCopy(BitConverter.GetBytes(SettingData.TcpKeepAliveTimeMilliseconds), 0, keepAlive, 4, 4);
            //keepalive 확인 간격
            Buffer.BlockCopy(BitConverter.GetBytes(SettingData.TcpKeepAliveIntervalMilliseconds), 0, keepAlive, 8, 4);


            //KeepAlive 설정
            //닷넷을 통한 KeepAlive설정은 .NET Core 3.0이상에서만 지원한다.
            //https://learn.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketoptionname?view=netcore-3.0#system-net-sockets-socketoptionname-tcpkeepalivetime
            //윈도우의 경우 IOControl를 통해서 적용할 수 있다.
            //this.socketServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            //this.socketServer.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 10000);
            //this.socketServer.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 5000);
            // Windows 10 version 1703 or later
            //https://learn.microsoft.com/en-us/windows/win32/winsock/ipproto-tcp-socket-options?WT.mc_id=DT-MVP-4038148
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            //    && Environment.OSVersion.Version >= new Version(10, 0, 15063))
            //{//윈도우10, 1703 이후 버전(windows server 2019)
            //    this.socketServer.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);
            //}

            return keepAlive;
        }
    }

    /// <summary>
    /// 헤더의 크기로 사용될 타입
    /// </summary>
    public enum HeaderSizeType
    {
        /// <summary>
        /// 1바이트
        /// <para>0~255</para>
        /// </summary>
        Byte = 1,

        /// <summary>
        /// 2바이트
        /// <para>-32,768 ~ 32,767</para>
        /// </summary>
        Short = 2,

        /// <summary>
        /// 4바이트
        /// <para>-2,147,483,648 ~ 2,147,483,647</para>
        /// </summary>
        Int = 4,

        /// <summary>
        /// 8바이트
        /// <para>-9,223,372,036,854,775,808 ~ 9,223,372,036,854,775,807</para>
        /// </summary>
        Long = 8,
    }
}
