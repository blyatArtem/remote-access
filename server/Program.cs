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
                Console.ReadKey();
                INetCommand? result = Server.Current.Connections[0].Send(new CommandRMDIR("C:\\asd"));
                if (result == null)
                {
                    continue;
                }
                CommandResult callback = result! as CommandResult;
                Console.WriteLine($"returned: success: {callback.Success}, message: {callback.Message}");
            }
        }

        private const int PORT = 228;

    }
}
