using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.ConsoleCommands
{
    internal class CCHelp : ConsoleCommand
    {
        public CCHelp() : base("help")
        {

        }

        public override void Execute()
        {
            foreach(var c in Program.Commands)
            {
                string displayName = c.Name.ToUpper() + " ";
                Console.ForegroundColor = ConsoleColor.White;
                if (c.ShortName is not null)
                    displayName += $"({c.ShortName})";

                Console.Write(displayName);
                Tab(displayName);
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach(var a in c.Arguments)
                {
                    Console.Write(a.ToString() + " ");
                }
                Console.Write(Environment.NewLine);
            }
        }

        private void Tab(string text)
        {
            string tab = "";
            for (int i = 0; i < Math.Abs(3 - text.Length / 8); i++)
            {
                tab += "\t";
            }
            Console.Write(tab);
        }
    }
}
