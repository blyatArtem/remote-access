using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands.Callbacks
{
    internal class CommandResult : INetCommand, INetResult
    {
        public CommandResult()
        {
            Message = "";
        }

        public int ID => 0;

        public void Deserialize(CommandReader reader)
        {
            Success = reader.ReadBool();
            Message = reader.ReadString();
        }

        public void Invoke()
        {
            Program.Print($"Received: {Message}", Success ? ConsoleColor.Green : ConsoleColor.Red);
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
