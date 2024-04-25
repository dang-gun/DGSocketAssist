using System;
using System.Windows.Forms;

using ChatGlobal;

using SocketServer4Test.Faculty;


namespace SocketServer4Test.Global
{
    /// <summary>
    /// 전역 프로그램 변수
    /// </summary>
    static class GlobalStatic
    {
        /// <summary>
        /// 사용할 메인폼(컨트롤러 + View)
        /// </summary>
        public static ServerForm MainForm = null;

        /// <summary>
        /// 서버 동작을 할 리스너
        /// </summary>
        public static ServerModel MainServer = new ServerModel();

        /// <summary>
        /// 체팅 명령어 처리 유틸
        /// </summary>
        public static readonly ChatCommand ChatCmd = new ChatCommand();

        /// <summary>
        /// 크로스 스레드 체크를 하고 상황에 맞게 처리한다.
        /// </summary>
        /// <param name="controlThis"></param>
        /// <param name="action"></param>
        public static void CrossThread_Winfom(Control controlThis, Action action)
        {
            if (true == controlThis.InvokeRequired)
            {//다른 쓰래드다.
                controlThis.Invoke(new Action(
                    delegate ()
                    {
                        action();
                    }));
            }
            else
            {//같은 쓰래드다.
                action();
            }
        }
    }
}
