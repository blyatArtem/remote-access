using System.Net;
using System.Net.Sockets;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener listner = new(IPAddress.Any, PORT);
            listner.Start();
            var client = listner.AcceptTcpClient();

            byte[] buffer = new byte[BUFFER_SIZE];
            client.GetStream().Read(buffer, 0, buffer.Length);

            Console.WriteLine(string.Join(' ', buffer));
        }

        private const int BUFFER_SIZE = 64;
        private const int PORT = 228;
    }
}
