using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetMessages.Callbacks
{
    internal class NMResult : INetCommand, INetResult
    {
        public NMResult()
        {
            Message = "";
        }

        public int ID => 0;

        public void Deserialize(NMReader reader)
        {
            Success = reader.ReadBool();
            Message = reader.ReadString();
        }

        public void Invoke()
        {
            Program.Print($"Received: {Message}", Success ? ConsoleColor.Green : ConsoleColor.Red);
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
