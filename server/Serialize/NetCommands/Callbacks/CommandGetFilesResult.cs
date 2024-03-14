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
            string directoriesStr = "";
            string filesStr = "";

            data.files.Where(x => x.isDir).OrderBy(x => x.fileName).ToList().ForEach(x => directoriesStr += $"{data.path}\\{x.fileName}{Environment.NewLine}");
            data.files.Where(x => !x.isDir).OrderBy(x => x.fileName).ToList().ForEach(x => filesStr += $"{data.path}\\{x.fileName}{Environment.NewLine}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(directoriesStr);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(filesStr);
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
