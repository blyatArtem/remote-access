using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands
{
    internal interface INetCommand
    {
        public abstract void Serialize(CommandWriter writer);

        public abstract void Deserialize(CommandReader reader);

        public abstract void Execute();

        public int ID
        {
            get;
        }
    }
}
