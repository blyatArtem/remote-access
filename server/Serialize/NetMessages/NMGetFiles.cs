using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetMessages
{
    internal class NMGetFiles : INetCommand, ISerializable
    {
        public NMGetFiles()
        {
            path = "";
        }

        public NMGetFiles(string path)
        {
            this.path = path;
        }

        public int ID => 3;

        public void Serialize(NMWriter writer)
        {
            writer.WriteString(path);
        }

        public string path;

    }
}
