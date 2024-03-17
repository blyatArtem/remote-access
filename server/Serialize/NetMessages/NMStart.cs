using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetMessages
{
    internal class NMStart : INetCommand, ISerializable
    {
        public NMStart(string file, string arg)
        {
            this.file = file;
            this.arg = arg;
        }

        public NMStart(string file) : this(file, "")
        {

        }

        public int ID => 5;

        public void Serialize(NMWriter writer)
        {
            writer.WriteString(file);
            writer.WriteString(arg);
        }

        public string file, arg;
    }
}
