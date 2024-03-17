using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize
{
    internal abstract class NMSerializer
    {
        public NMSerializer()
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

        protected byte[] ReadBytes(int lenght)
        {
            byte[] data = new byte[lenght];
            for (int i = 0; i < lenght; i++)
            {
                data[i] = buffer[position + i];
            }
            position += lenght;
            return data;
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
