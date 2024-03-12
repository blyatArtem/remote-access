using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Server : IDisposable
    {
        private Server()
        {
            this._block = new object();
            this.Connections = new List<Connection>();
        }

        private Server(uint port, IPAddress address) : this()
        {
            this.Port = port;
            this._address = address;
            this._listner = new TcpListener(address, (int)port);
        }

        ~Server()
        {
            if (!_disposed)
                Dispose();
        }

        internal static void Initialize(uint port, IPAddress address) => Current = new Server(port, address);

        internal void RunThread(bool sync = true)
        {
            _thr = new Thread(() =>
            {
                while (true)
                {
                    var connection = new Connection(_listner.AcceptTcpClient(), _connectionСounter);
                    _connectionСounter++;
                    lock (_block)
                        Connections.Add(connection);
                }
            });
            _thr.Start();
            if (sync)
                _thr.Join();
        }

        internal void StopThread()
        {
            if (_thr.IsAlive)
                _thr.Abort();
        }

        internal void Start() => _listner.Start();

        internal void Stop() => _listner.Stop();

        public void Dispose()
        {
            if (!_disposed)
                Dispose();
        }

        public void RemoveConnection(Connection connection)
        {
            lock (_block)
                Connections.Remove(connection);
        }

        public bool IsRunning
        {
            get => _running;
            set
            {
                if (value)
                    Start();
                else
                    Stop();
                _running = value;
            }
        }

        internal static Server Current
        {
            get; private set;
        }

        internal static Server? Second
        {
            get; private set;
        } = null!;

        internal uint Port
        {
            get; private set;
        }

        public List<Connection> Connections
        {
            get; set;
        }

        private readonly TcpListener _listner;
        private readonly IPAddress _address;
        private Thread _thr;

        private readonly object _block;

        private bool _running;
        private bool _disposed;
        private int _connectionСounter = 0;

        public const int BUFFER_SIZE = 32;
    }
}
