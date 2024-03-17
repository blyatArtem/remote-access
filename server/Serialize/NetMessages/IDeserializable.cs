using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetMessages
{
    internal interface IDeserializable
    {
        public abstract void Deserialize(NMReader reader);
    }
}
