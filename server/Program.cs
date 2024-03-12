using server.Serialize.NetCommands;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    internal class Program
    {
        static void Main()
        {
            Server.Initialize(PORT, IPAddress.Any);
            Server.Current.IsRunning = true;
            Server.Current.RunThread(false);

            for (; ; )
            {
                Console.ReadKey();
                Server.Current.Connections[0].Send(new CommandMKDIR("hello world!"));
            }
        }

        private const int PORT = 228;

    }
}
