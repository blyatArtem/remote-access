using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize
{
    internal class CommandWriter : CommandSerializer
    {
        public CommandWriter() : base()
        {

        }

        public void WriteBool(bool flag)
        {
            Resize(1);
            buffer[position] = flag ? (byte)1 : (byte)0;
            position += 1;
        }

        public void WriteInt32(int value)
        {
            Resize(4);
            var data = BitConverter.GetBytes(value);
            for (int i = 0; i < data.Length; i++)
            {
                buffer[position + i] = data[i];
            }
            position += 4;
        }

        public void WriteString(string text)
        {
            var data = Encoding.UTF8.GetBytes(text);
            WriteInt32(data.Length);
            WriteBytes(data);
        }
    }
}
