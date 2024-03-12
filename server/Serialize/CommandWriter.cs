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

        public void WriteInt32(int value)
        {
            Resize(4);
            var valueBuf = BitConverter.GetBytes(value);
            for (int i = 0; i < valueBuf.Length; i++)
            {
                buffer[position + i] = valueBuf[i];
            }
            position += 4;
        }

        public void WriteString(string text)
        {
            var valueBuf = Encoding.UTF8.GetBytes(text);
            WriteInt32(valueBuf.Length);
            WriteBytes(valueBuf);
        }
    }
}
