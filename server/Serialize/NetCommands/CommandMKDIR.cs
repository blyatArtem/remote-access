﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Serialize.NetCommands
{
    internal class CommandMKDIR : INetCommand, ISerializable
    {
        public CommandMKDIR(string path = "")
        {
            this.path = path;
            _id = 1;
        }

        public void Serialize(CommandWriter writer)
        {
            writer.WriteString(path);
        }

        public int ID => _id;
        public string path;

        protected int _id;

    }
}
