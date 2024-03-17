using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.ConsoleCommands
{
    internal abstract class ConsoleCommand
    {
        public ConsoleCommand(string name, CCArgument[] args)
        {
            this.Name = name;
            this.Arguments = args;
            this.ShortName = null;
        }

        public ConsoleCommand(string name) : this(name, []) { }

        public bool ParseArguments(List<string> arguments)
        {
            if (arguments.Count < Arguments.Length)
            {
                Program.Print("Argument missed", ConsoleColor.Red, false);
                return false;
            }

            for (int i = 0; i < Arguments.Length; i++)
            {
                if (!Arguments[i].TryParse(arguments[i]))
                {
                    Program.Print($"format \"{arguments[i]}\" {Arguments[i]}", ConsoleColor.Red, false);
                    return false;
                }
            }
            return true;
        }

        public static List<ConsoleCommand> GetCommands()
        {
            return
                [
                    new CCGetFiles(),
                    new CCSelectConnection(),
                    new CCHelp(),
                    new CCMKDIR(),
                    new CCRMDIR(),
                    new CCConnectionList(),
                ];
        }

        public abstract void Execute();

        public string Name { get; private set; }
        public string? ShortName { get; protected set; }
        public CCArgument[] Arguments { get; private set; }
    }
}
