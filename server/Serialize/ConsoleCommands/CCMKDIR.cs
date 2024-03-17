using server.Serialize.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.ConsoleCommands
{
    internal class CCMKDIR : ConsoleCommand
    {
        public CCMKDIR() : base("dir_create", [new CCArgumentString()])
        {
            ShortName = "mkdir";
        }

        public override void Execute()
        {
            var getFiles = new NMMKDIR((string)Arguments[0].Value);
            Program.SendNetCommand(getFiles);
        }
    }

    internal class CCRMDIR : ConsoleCommand
    {
        public CCRMDIR() : base("dir_delete", [new CCArgumentString()])
        {
            ShortName = "rmdir";
        }

        public override void Execute()
        {
            var getFiles = new NMRMDIR((string)Arguments[0].Value);
            Program.SendNetCommand(getFiles);
        }
    }
}
