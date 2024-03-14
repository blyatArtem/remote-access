using server.Serialize.NetCommands;
using server.Serialize.NetCommands.Callbacks;
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
            Console.WriteLine("rdy");
            for (; ; )
            {
                var key = Console.ReadKey().Key;

                INetCommand command = null!;

                if (key == ConsoleKey.D1)
                {
                    command = new CommandGetFiles(@"C:\Users\myzer\OneDrive\Desktop");
                }
                else if (key == ConsoleKey.D2)
                {
                    command = new CommandMKDIR(@"C:\Users\myzer\OneDrive\Desktop\test_rust");
                }
                else
                    continue;

                INetResult? callback = Server.Current.Connections[0].Send(command);
                if (callback == null)
                {
                    Console.WriteLine("result is null");
                    continue;
                }
                callback?.Invoke();
            }
        }

        private const int PORT = 228;

    }
}
