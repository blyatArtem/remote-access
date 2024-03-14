using server.Serialize;
using server.Serialize.NetCommands;
using server.Serialize.NetCommands.Callbacks;
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

        public INetResult? Send(INetCommand command)
        {
            CommandWriter writer = new CommandWriter();
            writer.WriteInt32(command.ID);
            (command as ISerializable)!.Serialize(writer);

            var buffer = writer.buffer;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(string.Join(" ", buffer));
            Console.ForegroundColor = ConsoleColor.Gray;

            var stream = Client.GetStream();
            stream.Write(buffer);
            stream.Flush();

            return Receive();
        }

        public INetResult? Receive()
        {
            List<byte> buffer = new List<byte>();
            for(; ; )
            {
                byte[] readBuffer = new byte[Server.BUFFER_SIZE];
                Client.GetStream().Read(readBuffer, 0, readBuffer.Length);
                buffer = buffer.Concat(readBuffer.ToList()).ToList();
                
                if (!ContinueRead(readBuffer))
                {
                    break;
                }
            }

            Console.WriteLine($"received buffer: " + buffer.Count);

            CommandReader reader = new CommandReader();
            reader.buffer = buffer.ToArray();
            int commandId = reader.ReadInt32();
            return GetCommandFromId(commandId, reader);
        }

        private INetResult? GetCommandFromId(int id, CommandReader reader)
        {
            if (id == 0)
            {
                CommandResult result = new CommandResult();
                result.Deserialize(reader);
                return result;
            }
            else if (id == 4)
            {
                CommandGetFilesResult result = new CommandGetFilesResult();
                result.Deserialize(reader);
                return result;
            }
            Console.WriteLine("Command id missed! ID: " + id);
            return null;
        }

        private bool ContinueRead(byte[] readBuffer)
        {
            for (int i = 1; i < 11; i++)
            {
                if (readBuffer[^i] != 0)
                    return true;
            }
            return false;
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
