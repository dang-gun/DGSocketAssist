using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGlobal
{
    /// <summary>
    /// 메시지 이벤트용 형식입니다.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// 메시지
        /// </summary>
        public readonly string Message = "";
        /// <summary>
        /// 메시지 타입
        /// </summary>
        public ChatCommandType CommandType = ChatCommandType.None;

        /// <summary>
        /// 메시지 설정
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="typeCommand"></param>
        public MessageEventArgs(ChatCommandType typeCommand, string strMsg)
        {
            this.Message = strMsg;
            this.CommandType = typeCommand;
        }
    }
}
