using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    internal class Program
    {
        static bool init = false;
        static TcpListener listner = new(IPAddress.Any, PORT);

        static void Main()
        {
            Console.WriteLine("waiting");
            if (!init)
            {
                listner.Start();
                init = true;
            }

            var client = listner.AcceptTcpClient();

            byte[] buffer = new byte[BUFFER_SIZE];

            var valueBuffer1 = BitConverter.GetBytes(1337);
            var valueBuffer2 = BitConverter.GetBytes(11);  
            var valueBuffer3 = BitConverter.GetBytes(228); 
            for (int i = 0; i < 4; i++)
            {
                buffer[i] = valueBuffer1[i];
            }
            for (int i = 4; i < 8; i++)
            {
                buffer[i] = valueBuffer2[i - 4];
            }
            int pos = 8;

            var textBuffer = Encoding.UTF8.GetBytes("hello world");

            foreach(byte b in textBuffer)
            {
                buffer[pos] = b;
                    pos++;
            }
            int j = 0;
            for (int i = pos; i < pos + 4; i++)
            {
                buffer[i] = valueBuffer3[j];
                j++;
            }

            Console.WriteLine("connected");

            client.GetStream().Write(buffer, 0, buffer.Length);

            Console.WriteLine("sent");

            Main();
        }

        private const int BUFFER_SIZE = 1024;
        private const int PORT = 228;
    }
}
