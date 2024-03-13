using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize
{
    internal class CommandReader : CommandSerializer
    {
        public CommandReader() : base()
        {

        }

        public bool ReadBool()
        {
            byte flag = buffer[position];
            position += 1;
            return flag != 0 ? true : false;
        }

        public int ReadInt32()
        {
            byte[] bufferValue = new byte[4];
            bufferValue[0] = buffer[position];
            bufferValue[1] = buffer[position + 1];
            bufferValue[2] = buffer[position + 2];
            bufferValue[3] = buffer[position + 3];
            int value = BitConverter.ToInt32(bufferValue);
            position += 4;
            return value;
        }

        public string ReadString()
        {
            int lenght = ReadInt32();
            byte[] data = ReadBytes(lenght);
            return Encoding.UTF8.GetString(data, 0, lenght);
        }
    }
}
