using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize
{
    internal abstract class CommandSerializer
    {
        public CommandSerializer()
        {
            buffer = new byte[Server.BUFFER_SIZE];
            position = 0;
        }
        protected void WriteBytes(byte[] bytes)
        {
            int size = bytes.Length;
            Resize(size);
            for (int i = 0; i < bytes.Length; i++)
            {
                buffer[position + i] = bytes[i];
            }
            position += bytes.Length;
        }

        protected void Resize(int needBytes)
        {
            while (position + needBytes > buffer.Length)
                Array.Resize(ref buffer, buffer.Length + Server.BUFFER_SIZE);
        }

        public byte[] buffer;
        protected int position;
    }
}
