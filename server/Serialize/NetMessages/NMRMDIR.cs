using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetMessages
{
    internal class NMRMDIR : NMMKDIR
    {
        public NMRMDIR()
        {
            _id = 2;
        }

        public NMRMDIR(string path) : base(path)
        {
            _id = 2;
        }
    }
}
