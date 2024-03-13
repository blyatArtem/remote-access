using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands.Callbacks
{
    internal class CommandResult : INetCommand, INetMessageResult
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

        public void Serialize(CommandWriter writer) => throw new NotImplementedException("CommandResult.Serialize");

        public void Invoke()
        {
            throw new NotImplementedException();
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
