﻿using ChatGlobal;
using DGSocketAssist1_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientTest
{
	/// <summary>
	/// 프로그램 변수
	/// </summary>
	public static class GlobalStatic
	{
		/// <summary>
		/// 사용할 메인폼
		/// </summary>
		public static ClientForm MainForm = null;

		/// <summary>
		/// 서버 동작을 할 리스너
		/// </summary>
		public static Client Client = null;

		/// <summary>
		/// 체팅 명령어 처리 유틸
		/// </summary>
		public static readonly ChatCommand ChatCmd = new ChatCommand();
	}
}
