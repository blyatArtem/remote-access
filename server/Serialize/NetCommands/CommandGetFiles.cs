using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands
{
    internal class CommandGetFiles : INetCommand, ISerializable
    {
        public CommandGetFiles()
        {
            path = "";
        }

        public CommandGetFiles(string path)
        {
            this.path = path;
        }

        public int ID => 3;

        public void Serialize(CommandWriter writer)
        {
            writer.WriteString(path);
        }

        public string path;

    }
}
