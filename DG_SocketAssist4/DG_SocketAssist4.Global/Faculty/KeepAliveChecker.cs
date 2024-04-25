using System;
using System.Timers;

namespace DG_SocketAssist4.Global.Faculty
{
    /// <summary>
    /// 접속이 유지되고 있는지 확인하기위한 동작
    /// </summary>
    /// <remarks>
    /// KeepAlive로 인한 끊김도 직접 send/receive를 하지 않으면 알 수 없다<br />
    /// 그러므로 일정시간동안 send/receive가 없으면 send를 하는 동작을 해주는 모델이다.
    /// </remarks>
    public class KeepAliveChecker
    {
        private Timer timer;

        private Action ActionSend;

        public KeepAliveChecker(Action action)
        {
            this.ActionSend = action;

            this.timer = new Timer();
            this.timer.Interval = 5000;

            this.timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
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
            Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);
            //keepalive 확인 시간
            Buffer.BlockCopy(BitConverter.GetBytes(SettingData.TcpKeepAliveTime), 0, keepAlive, 4, 4);
            //keepalive 확인 간격
            Buffer.BlockCopy(BitConverter.GetBytes(SettingData.TcpKeepAliveInterval), 0, keepAlive, 8, 4);

            return keepAlive;
        }
    }
}
