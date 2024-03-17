using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Server
    {
        private Server()
        {
            this._block = new object();
            this._connections = new List<Connection>();
        }

        private Server(uint port, IPAddress address) : this()
        {
            this.Port = port;
            this._address = address;
            this._listner = new TcpListener(address, (int)port);
        }

        internal static void Initialize(uint port, IPAddress address) => Current = new Server(port, address);

        internal void RunThread()
        {
            _thr = new Thread(() =>
            {
                while (true)
                {
                    var connection = new Connection(_listner.AcceptTcpClient(), _connectionsIndex);
                    _connectionsIndex++;
                    lock (_block)
                        _connections.Add(connection);
                    clientConnected?.Invoke(this, connection);
                }
            });
            _thr.Start();
        }

        internal void StopThread()
        {
            if (_thr.IsAlive)
                _thr.Abort();
        }

        internal void Start() => _listner.Start();

        internal void Stop() => _listner.Stop();

        public void RemoveConnection(Connection connection)
        {
            lock (_block)
            {
                _connections.Remove(connection);
                clientDisconnected?.Invoke(this);
            }
        }

        public Connection? GetConnection(int index)
        {
            foreach (var c in Connections)
            {
                if (c.Index == index)
                    return c;
            }
            return null;
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
        } = null!;

        internal static Server? Second
        {
            get; private set;
        } = null!;

        internal uint Port
        {
            get; private set;
        }

        internal IReadOnlyCollection<Connection> Connections
        {
            get => new ReadOnlyCollection<Connection>(_connections);
        }

        private List<Connection> _connections;

        public Action<Server, Connection>? clientConnected;
        public Action<Server>? clientDisconnected;

        private readonly TcpListener _listner;
        private readonly IPAddress _address;
        private Thread _thr;

        private readonly object _block;

        private bool _running;
        private int _connectionsIndex = 0;

        public const int BUFFER_SIZE = 32;
    }
}
