using server.Serialize;
using server.Serialize.NetCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Connection
    {
        public Connection(TcpClient client, int index = -1)
        {
            this.Client = client;
            this.Index = index;
        }

        public void Send(INetCommand command)
        {
            CommandWriter writer = new CommandWriter();
            writer.WriteInt32(command.ID);
            command.Serialize(writer);

            var buffer = writer.buffer;

            Console.WriteLine(string.Join(" ", buffer));

            var stream = Client.GetStream();
            stream.Write(buffer);
            stream.Flush();
        }

        public TcpClient Client
        {
            get; private set;
        }

        public int Index
        {
            get; private set;
        }
    }
}
