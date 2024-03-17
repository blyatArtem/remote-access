using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetMessages.Callbacks
{
    internal class ConnectionClosed : INetResult
    {
        public int ID => -1;

        public void Invoke()
        {
        }
    }
}
