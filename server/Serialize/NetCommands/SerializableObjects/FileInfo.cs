using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands.SerializableObjects
{
    internal class FileInfo
    {
        public FileInfo()
        {
            fileName = "";
            isDir = false;
        }

        public string fileName;
        public bool isDir;
    }
}
