using DGSocketAssist3_Server;
using System;
using System.Net;

namespace ServerTestConsole
{
	internal class Program
	{
		public static Server serverThis = null;

		static void Main(string[] args)
		{
			//서버 개체 생성
			serverThis = new Server(10, 1024);
			//초기화
			serverThis.Init();

			//서버 ip 및 포트
			IPEndPoint ipServer
				= new IPEndPoint(IPAddress.Any, 7000);
			//서버 시작
			serverThis.Start(ipServer);
		}
	}
}
