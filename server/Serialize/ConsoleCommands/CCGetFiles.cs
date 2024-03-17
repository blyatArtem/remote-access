using server.Serialize.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.ConsoleCommands
{
    internal class CCGetFiles : ConsoleCommand
    {
        public CCGetFiles() : base("dir_ls", [new CCArgumentString()])
        {
        }

        public override void Execute()
        {
            var getFiles = new NMGetFiles((string)Arguments[0].Value);
            Program.SendNetCommand(getFiles);
        }
    }
}
