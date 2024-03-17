using server.Serialize.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.ConsoleCommands
{
    internal class CCSelectConnection : ConsoleCommand
    {
        public CCSelectConnection() : base("c_select", [new CCArgumentInt32()])
        {
        }

        public override void Execute()
        {
            Connection? c = Server.Current.GetConnection((int)Arguments[0].Value);
            if (c is not null)
            {
                Program.selectedConnection = c;
            }
            else
            {
                Program.Print($"Connection not found", ConsoleColor.Red, false);
            }
        }
    }

    internal class CCConnectionList : ConsoleCommand
    {
        public CCConnectionList() : base("c_ls")
        {

        }

        public override void Execute()
        {
            foreach(var c in Server.Current.Connections)
            {
                Program.Print($"{c.Index} {c.Client.Available} {c.Client.Connected}");
            }
        }
    }
}
