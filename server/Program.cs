using server.Serialize.NetCommands;
using server.Serialize.NetCommands.Callbacks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace server
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Connections: 0";

            Server.Initialize(PORT, IPAddress.Any);
            Server.Current.IsRunning = true;
            Server.Current.clientConnected += (sender, connection) => Console.Title = $"Connections: {sender.Connections.Count}";

            Server.Current.RunThread();
            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{(selectedConnection is null ? "NULL" : selectedConnection.Index)}>");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                RunCommand(ParseCommand(input, out List<string> arguments), in arguments);

            }
        }

        static string ParseCommand(string input, out List<string> arguments)
        {
            MatchCollection matches = Regex.Matches(input, @"(\"".+?\"")|\w+");
            arguments = new List<string>();

            for (int i = 1; i < matches.Count; i++)
                arguments.Add(matches[i].Value.Replace("\"", ""));

            return matches[0].Value.Replace("\"", "");
        }

        static void RunCommand(string commandName, in List<string> arguments)
        {
            if (commandName == "dir")
            {
                var getFiles = new CommandGetFiles(arguments[0]);
                SendNetCommand(getFiles);
            }
            else if (commandName == "mkdir")
            {
                var mkdir = new CommandMKDIR(arguments[0]);
                SendNetCommand(mkdir);
            }
            else if (commandName == "rmdir")
            {
                var rmdir = new CommandRMDIR(arguments[0]);
                SendNetCommand(rmdir);
            }
            else if (commandName == "connection")
            {
                int index = int.Parse(arguments[0]);
                selectedConnection = Server.Current.Connections[index];
            }
        }

        static void SendNetCommand(INetCommand command)
        {
            if (selectedConnection == null)
            {
                Print("The connection must be selected", ConsoleColor.Red);
                return;
            }
            INetResult? callback = selectedConnection.Send(command);
            if (callback == null)
            {
                Print("Failed to deserialize callback", ConsoleColor.Yellow);
                return;
            }
            callback?.Invoke();
        }

        public static void Print(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            string connectionStr = selectedConnection == null ?
                "NULL" :
                selectedConnection.Index.ToString();

            Console.ForegroundColor = color;
            Console.WriteLine($"{connectionStr} | {text}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static Connection? selectedConnection = null;

        private const int PORT = 228;

    }
}
