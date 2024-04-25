using System;


namespace DG_SocketAssist4.Global.Faculty
{
    /// <summary>
    /// SettingData의 설정에 맞게 전달된 바이트를 처리하는 클래스
    /// </summary>
    internal class BtyeAssist
    {
        internal BtyeAssist() { }

        /// <summary>
        /// 입력된 길이를 BufferHeaderSize에 맞는 byte[]로 변환한다.
        /// </summary>
        /// <param name="nLength"></param>
        /// <returns></returns>
        internal byte[] LengthToByte(int nLength)
        {
            //리턴할 데이터
            byte[] byteReturn = new byte[SettingData.BufferHeaderSize];

            //숫자를 변환한 데이터
            byte[] byteHeader = BitConverter.GetBytes(nLength);

            //작은쪽의 길이
            int nDataLength = 0;
            if(byteReturn.Length > byteHeader.Length)
            {
                nDataLength = byteHeader.Length;
            }
            else
            {
                nDataLength = byteReturn.Length;
            }


            //작은 쪽의 길에 맞춰 데이터를 완성한다.
            Array.Copy(byteHeader, 0, byteReturn, 0, nDataLength);

            return byteReturn;
        }

        /// <summary>
        /// 전달 받은 데이터의 크기를 byte로 바꿔 앞쪽에 붙여 데이터를 완성한다.
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        internal byte[] SizeAddData(byte[] byteData)
        {
            //데이터 길이를 헤더 데이터로 만들다.
            byte[] byteHeader = this.LengthToByte(byteData.Length);


            //헤더를 포함한 데이터의 크기로 공간을 만든다.
            byte[] byteReturn = new byte[byteHeader.Length + byteData.Length];
            //헤더 복사
            Array.Copy(byteHeader, 0, byteReturn, 0, byteHeader.Length);
            //데이터 복사
            Array.Copy(byteData, 0, byteReturn, byteHeader.Length, byteData.Length);

            return byteReturn;
        }
    }
}
