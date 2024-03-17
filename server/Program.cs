using server.Serialize.ConsoleCommands;
using server.Serialize.NetMessages;
using server.Serialize.NetMessages.Callbacks;
using System.Diagnostics;
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

            InitializeServer();

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

        static void InitializeServer()
        {

            Server.Initialize(PORT, IPAddress.Any);
            Server.Current.IsRunning = true;

            Server.Current.clientConnected += (sender, connection) => Console.Title = $"Connections: {sender.Connections.Count}";
            Server.Current.clientDisconnected += (sender) => Console.Title = $"Connections: {sender.Connections.Count}";

            Server.Current.RunThread();
        }

        static string ParseCommand(string input, out List<string> arguments)
        {
            MatchCollection matches = Regex.Matches(input, @"(\"".+?\"")|\w+");
            arguments = new List<string>();

            for (int i = 1; i < matches.Count; i++)
                arguments.Add(matches[i].Value.Replace("\"", ""));

            return matches[0].Value.Replace("\"", "").ToLower();
        }

        static void RunCommand(string commandName, in List<string> arguments)
        {
            foreach (var command in Commands)
            {
                if ((command.ShortName is not null && command.ShortName == commandName) || command.Name == commandName)
                {
                    if (command.ParseArguments(arguments))
                    {
                        command.Execute();
                        return;
                    }
                }
            }
            Print("Unknown command", ConsoleColor.Red, false);
        }

        public static void SendNetCommand(INetCommand command)
        {
            if (selectedConnection == null)
            {
                Print("The connection must be selected", ConsoleColor.Red);
                return;
            }
            INetResult? callback = selectedConnection.Send(command);
            if (callback is ConnectionClosed)
            {
                Server.Current.RemoveConnection(selectedConnection);
                selectedConnection.Dispose();
                selectedConnection = null;
                return;
            }
            else if (callback == null)
            {
                Print("Failed to deserialize callback.", ConsoleColor.Yellow);
                return;
            }
            callback?.Invoke();
        }

        public static void Print(string text, ConsoleColor color = ConsoleColor.Gray, bool displayConnection = true)
        {
            string connectionStr = selectedConnection == null ?
                "NULL" :
                selectedConnection.Index.ToString();

            Console.ForegroundColor = color;
            Console.WriteLine(displayConnection ? $"{connectionStr} | {text}" : text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static Connection? selectedConnection = null;

        public static List<ConsoleCommand> Commands = ConsoleCommand.GetCommands().OrderBy(x => x.Name).ToList();

        private const int PORT = 228;

    }
}
