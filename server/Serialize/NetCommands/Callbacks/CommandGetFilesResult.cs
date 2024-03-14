using server.Serialize.NetCommands.SerializableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands.Callbacks
{
    internal class CommandGetFilesResult : INetResult, IDeserializable
    {
        public CommandGetFilesResult()
        {
            data = new DirInfo();
        }

        public void Invoke()
        {
            string result = "";
            data.files.ForEach(x => result += $"{data.path}\\{x.fileName}{Environment.NewLine}");
            Console.WriteLine(result);
        }   

        public void Deserialize(CommandReader reader)
        {
            data.path = reader.ReadString();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                SerializableObjects.FileInfo fileInfo = new SerializableObjects.FileInfo();
                fileInfo.fileName = reader.ReadString();
                fileInfo.isDir = reader.ReadBool();
                data.files.Add(fileInfo);
            }
        }

        public int ID => 4;

        public DirInfo data;

    }
}
