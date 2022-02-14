using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ClientTestConsole
{
	internal class Program
	{
        static Client client = null;

        static void Main(string[] args)
        {
            //서버 ip 및 포트
            IPEndPoint ipServer
                = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7000);
            client = new Client(ipServer);
            client.Connect();

            

            

            Task.Delay(1000)
                .ContinueWith((task) => 
                {
                    byte[] send = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    client.Send(send);
                });

            Console.WriteLine("Press any key to terminate the client process...");
            Console.Read();
        }
        

    }
}
