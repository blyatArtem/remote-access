using server.Serialize;
using server.Serialize.NetMessages;
using server.Serialize.NetMessages.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Connection : IDisposable
    {
        public Connection(TcpClient client, int index = -1)
        {
            this.Client = client;
            this.Index = index;
        }

        ~Connection()
        {
            Dispose();
        }

        public INetResult? Send(INetCommand command)
        {
            NMWriter writer = new NMWriter();
            writer.WriteInt32(command.ID);
            (command as ISerializable)!.Serialize(writer);

            var buffer = writer.buffer;

            Program.Print($"sent {buffer.Length} bytes", ConsoleColor.DarkGray);

            var stream = Client.GetStream();
            try
            {
                stream.Write(buffer);
            }
            catch(IOException _)
            {
                Program.Print($"Connection was forcibly closed by the remote host", ConsoleColor.Red);
                return new ConnectionClosed();
            }
            catch(Exception exc)
            {
                Program.Print(exc.ToString(), ConsoleColor.Red);
                return new ConnectionClosed();
            }
            stream.Flush();

            return Receive();
        }

        public INetResult? Receive()
        {
            List<byte> buffer = new List<byte>();
            for (; ; )
            {
                byte[] readBuffer = new byte[Server.BUFFER_SIZE];
                Client.GetStream().Read(readBuffer, 0, readBuffer.Length);
                buffer = buffer.Concat(readBuffer.ToList()).ToList();

                if (!ContinueRead(readBuffer))
                {
                    break;
                }
            }

            Program.Print($"received {buffer.Count} bytes", ConsoleColor.DarkGray);

            NMReader reader = new NMReader();
            reader.buffer = buffer.ToArray();
            int commandId = reader.ReadInt32();
            return GetCommandFromId(commandId, reader);
        }

        private INetResult? GetCommandFromId(int id, NMReader reader)
        {
            if (id == 0)
            {
                NMResult result = new NMResult();
                result.Deserialize(reader);
                return result;
            }
            else if (id == 4)
            {
                NMGetFilesResult result = new NMGetFilesResult();
                result.Deserialize(reader);
                return result;
            }
            Program.Print("Failed to parse ID. Received value: " + id, ConsoleColor.Red);
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


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Client.Dispose();

                // null

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TcpClient Client
        {
            get; private set;
        }

        public int Index
        {
            get; private set;
        }

        private bool _disposed;
    }
}
