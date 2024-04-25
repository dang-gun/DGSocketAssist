using System;

using DG_SocketAssist4.Global.Faculty;

namespace DG_SocketAssist4.Global.SendAssists
{
    /// <summary>
    /// 전송할 데이터를 큐로 관리
    /// </summary>
    public class SendAssist
    {
        /// <summary>
        /// 바이트 처리 지원
        /// </summary>
        private BtyeAssist BtyeAssist = new BtyeAssist();
        /// <summary>
        /// 보내기 큐 관리 지원
        /// </summary>
        private SendQueue SendQueue = new SendQueue();


        public SendAssist() { }

        /// <summary>
        /// SendCheck가 선공하면 동작할 액션 대리자
        /// </summary>
        /// <param name="byteMsg_Send">전송할 데이터(헤더까지 붙어있음)</param>
        public delegate void SendCheckDelegate(byte[] byteMsg_Send);
        /// <summary>
        /// 전달된 데이터에 헤더를 추가하여 큐에 추가한 다음 
        /// 대기중인 큐의 맨앞의 데이터를 action에 전달한다.
        /// <para>큐에 얼마만큼의 데이터가 남아있던지 딱 한번만 작동한다.</para>
        /// </summary>
        /// <remarks>
        /// byteData가 비어있으면 큐에 추가하지 않음
        /// <para>큐가 비어 있으면 action가 호출되지 않는다.</para>
        /// <para>보내기 action에 대한 완료처리를 밖에서 해야 한다.</para>
        /// <para>byteData를 빈값(null 이나 new byte[0])으로 보내면 큐에 추가 하지 않고 
        /// 다음 데이터를 추출하여 진행한다.</para>
        /// </remarks>
        /// <param name="byteData"></param>
        /// <param name="action"></param>
        public void SendCheck(
            byte[] byteData
            , SendCheckDelegate action)
        {

            if (null != byteData
                && 0 < byteData.Length)
            {//전달할 데이터가 있다.

                //데이터에 헤더를 붙이고
                byte[] byteHeader = this.BtyeAssist.SizeAddData(byteData);
                //전송 시도
                this.SendQueue.Add(byteHeader);
            }


            //여기서부터는 큐를 가지고 동작한다.
            if (false == this.SendQueue.Used
                && 0 < this.SendQueue.Count)
            {
                //사용중임을 알리고
                this.SendQueue.Used = true;

                //맨 앞에 있는 데이터를 읽는다.
                byte[] sMsg_Send = this.SendQueue.Get();

                if (0 < sMsg_Send.Length)
                {//값이 있으면 처리 시작

                    //액션 호출
                    action(sMsg_Send);
                }
                else
                {//값이 없다.

                    //값이 없을리가 없으므로 강제로 에러를 만든다.
                    throw new Exception("SendQueue에서 받은 데이터가 비어있다.");
                }
            }
        }

        /// <summary>
        /// 큐 사용이 완료되었음을 알림
        /// </summary>
        public void SendCheckCompleted()
        {
            //큐 사용이 끝남을 알림
            this.SendQueue.Used = false;
        }



    }
}
