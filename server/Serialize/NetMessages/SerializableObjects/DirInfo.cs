using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetMessages.SerializableObjects
{
    internal class DirInfo
    {
        public DirInfo()
        {
            path = "";
            files = new List<FileInfo>();
        }

        public string path;
        public List<FileInfo> files;
    }
}
