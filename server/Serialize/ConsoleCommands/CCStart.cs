using server.Serialize.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.ConsoleCommands
{
    internal class CCStart : ConsoleCommand
    {
        public CCStart() : base("start", [new CCArgumentString()])
        {

        }

        public override void Execute()
        {
            var mes = new NMStart((string)Arguments[0].Value);
            Program.SendNetCommand(mes);
        }
    }

    internal class CCStartArg : ConsoleCommand
    {
        public CCStartArg() : base("start_arg", [new CCArgumentString(), new CCArgumentString()])
        {
        }

        public override void Execute()
        {
            var mes = new NMStart((string)Arguments[0].Value, (string)Arguments[1].Value);
            Program.SendNetCommand(mes);
        }
    }
}
