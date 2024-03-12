using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands
{
    internal class CommandMKDIR : INetCommand
    {
        public CommandMKDIR(string path = "")
        {
            this.path = path;
        }

        public void Execute()
        {
            Console.WriteLine("MKDIR.Execute");
        }

        public void Serialize(CommandWriter writer)
        {
            writer.WriteString(path);
        }

        public void Deserialize(CommandReader reader)
        {

        }

        public int ID => 1;

        public string path;

    }
}
