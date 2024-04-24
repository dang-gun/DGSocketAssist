
using System;
using System.Collections.Generic;

namespace DG_SocketAssist4.Global
{
    /// <summary>
    /// 리시브 데이터를 임시로 저장해둘 버퍼
    /// </summary>
    public class ReceiveBufferTemp
    {
        /// <summary>
        /// 임시 버퍼
        /// </summary>
        public List<byte> BufferTemp = new List<byte>();

        public ReceiveBufferTemp() 
        { 

        }

        /// <summary>
        /// 임시 버퍼의 맨뒤에 데이터를 추가한다.
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="nSize">잘라서 넣을 데이터 크기</param>
        public void Add(byte[] byteData, int nSize)
        {
            //데이터를 잘라서 임시로 저장할 공간
            byte[] byteSlice = new byte[nSize];
            //지정한 크기 만큼 저장
            Array.Copy(byteData, 0, byteSlice, 0, nSize);

            //데이터 추가
            BufferTemp.AddRange(byteSlice);
        }

        /// <summary>
        /// SettingData.BufferHeaderSize에 맞게 크기 잘라서 리턴한다.
        /// </summary>
        /// <remarks>
        /// * 무결성 검사 하지 않음 *
        /// </remarks>
        /// <returns></returns>
        private byte[] HeaderSizeGet()
        {
            byte[] byteHeaderSize = new byte[SettingData.BufferHeaderSize];
            Array.Copy(this.BufferTemp.ToArray(), 0
                        , byteHeaderSize, 0, SettingData.BufferHeaderSize);

            return byteHeaderSize;
        }

        /// <summary>
        /// 버퍼 비우기
        /// </summary>
        public void Clear()
        {
            this.BufferTemp.Clear();
        }

        /// <summary>
        /// SettingData.BufferHeaderSize사이즈만큼 잘라서 숫자로 바꾼다.
        /// <para>Byte ~ Int 까지 사용하는 함수</para>
        /// </summary>
        /// <returns>계산된 크기. -1 = 버퍼에 최소한의 데이터도 쌓이지 않았다.</returns>
        public int HeaderToInt()
        {
            int nReturn = -1;

            if(SettingData.BufferHeaderSize <= BufferTemp.Count)
            {//최소 헤더 크기를 채웠다.

                byte[] byteHeaderSize = this.HeaderSizeGet();

                //패킷 크기 받기
                switch(SettingData.BufferHeaderSizeType)
                {
                    case HeaderSizeType.Byte:
                        nReturn = byteHeaderSize[0];
                        break;

                    case HeaderSizeType.Short:
                        nReturn = BitConverter.ToInt16(byteHeaderSize, 0);
                        break;

                    case HeaderSizeType.Int:
                        nReturn = BitConverter.ToInt32(byteHeaderSize, 0);
                        break;
                }
                
            }

            return nReturn;
        }

        /// <summary>
        /// SettingData.BufferHeaderSize사이즈만큼 잘라서 숫자로 바꾼다.
        /// <para>Long에서 사용하는 함수</para>
        /// </summary>
        /// <returns>계산된 크기. -1 = 버퍼에 최소한의 데이터도 쌓이지 않았다.</returns>
        public long HeaderToLong()
        {
            long nReturn = -1;

            if (SettingData.BufferHeaderSize <= BufferTemp.Count)
            {//최소 헤더 크기를 채웠다.

                byte[] byteHeaderSize = this.HeaderSizeGet();

                //패킷 크기 받기
                nReturn = BitConverter.ToInt64(byteHeaderSize, 0);
            }


            return nReturn;
        }


        /// <summary>
        /// 맨 앞의 헤더가 지정한 크기 이상의 데이터가 쌓여있는지 여부
        /// <para>SettingData.BufferHeaderSize를 기준으로 헤더를 읽은 다음 
        /// 헤더가 지정한 크기 이상으로 데이터가 쌓여있는지 여부를 리턴한다.</para>
        /// </summary>
        /// <returns></returns>
        public bool FirstSizeLength()
        {
            bool bReturn = false;

            if(this.BufferTemp.Count >= (SettingData.BufferHeaderSize + this.HeaderToInt()))
            {//충분히 쌓임
                bReturn = true;
            }

            return bReturn;
        }

        /// <summary>
        /// 맨 앞의 헤더가 지정한 이상의 데이터가 쌓여있으면 
        /// 헤더가 지정한 만큼 데이터를 잘라 리턴한다.
        /// <para>데이터가 모자르거나 문제가 있으면 byte[0]이 리턴된다.</para>
        /// </summary>
        /// <returns>헤더가 제거된 데이터 영역(지정된 크기 만큼의 바이트 개수)</returns>
        public byte[] FirstSizeData_Int()
        {
            byte[] byteReturn = new byte[0];


            //효율을 위해서 FirstSizeLength를 호출하지 않고 다시 계산한다.
            int nSize = HeaderToInt();

            if (this.BufferTemp.Count >= (SettingData.BufferHeaderSize + nSize))
            {//데이터 최소 크기는 채움

                //크기를 다시 설정하고
                byteReturn = new byte[nSize];

                //데이터 복사
                Array.Copy(
                    BufferTemp.ToArray(), SettingData.BufferHeaderSize
                    , byteReturn, 0
                    , nSize);

                //사용한 버퍼 제거
                this.BufferTemp.RemoveRange(0, SettingData.BufferHeaderSize + nSize);
            }

            return byteReturn;
        }
    }
}
