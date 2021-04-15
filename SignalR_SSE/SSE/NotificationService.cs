using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalR_SSE.SSE
{
    public class NotificationService
    {
        private readonly static Lazy<NotificationService> instance =
            new Lazy<NotificationService>(() => new NotificationService(
                 GlobalHost.ConnectionManager.GetConnectionContext<NotificationConnection>().Connection));

        public event EventHandler OnNotify;

        private NotificationService(IConnection connection)
        {
            this.Connection = connection;
        }

        public static NotificationService Instance { get { return instance.Value; } }

        private IConnection Connection { get; set; }

        public void Notify(string message, int? id)
        {
            Connection.Broadcast(message);
            EventHandler handler = OnNotify;
            if (handler != null)
            {
                handler(this, new NotifyEventArgs(message, id));
            }
        }
    }
    public class NotificationConnection : PersistentConnection
    {
        private readonly NotificationService notification;

        public NotificationConnection() : this(NotificationService.Instance) { }
        public NotificationConnection(NotificationService notification)
        {
            this.notification = notification;
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            return base.OnConnected(request, connectionId);
        }

        protected override Task OnReconnected(IRequest request, string connectionId)
        {
            return base.OnReconnected(request, connectionId);
        }

        //protected override Task OnDisconnected(IRequest request, string connectionId)
        //{
        //    return base.OnDisconnected(request, connectionId);
        //}

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return base.OnReceived(request, connectionId, data);
        }
    }

   public class NotifyEventArgs : EventArgs
    {
        public string Message { get; set; }
        public int? Id { get; }

        public NotifyEventArgs(string message, int? id)
        {
            Message = message;
            Id = id;
        }
    }
}