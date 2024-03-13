using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands
{
    internal class CommandRMDIR : CommandMKDIR
    {
        public CommandRMDIR()
        {
            _id = 2;
        }

        public CommandRMDIR(string path) : base(path)
        {
            _id = 2;
        }
    }
}
