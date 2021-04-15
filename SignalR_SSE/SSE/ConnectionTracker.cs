using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR_SSE.SSE
{
    public class ConnectionTracker
    {
        private readonly static Lazy<ConnectionTracker> instance =
            new Lazy<ConnectionTracker>(() => new ConnectionTracker());
        public static ConnectionTracker Instance { get { return instance.Value; } }
        private List<int> connections;

        public ConnectionTracker()
        {
            connections = new List<int>();
        }

        public void AddConnection(int id)
        {
            connections.Add(id);
        }

        public int ConnectionCount => connections.Count;

        internal void RemoveConnection(int? myConnectionId)
        {
            connections.Remove(myConnectionId.Value);
        }
    }
}