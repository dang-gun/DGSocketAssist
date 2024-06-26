﻿using System;

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
        /// 연결 유지 확인 시간(ms)
        /// </summary>
        public readonly static uint TcpKeepAliveTime = 1000;
        /// <summary>
        /// 연결 유지 확인 간격(ms)
        /// </summary>
        public readonly static uint TcpKeepAliveInterval = 1000;

        
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
