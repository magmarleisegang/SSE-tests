using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;

namespace SignalR_SSE.SSE
{
    public interface IMessageQueue
    {
        void Register(string id);
        void Unregister(string id);
        Task<string> DequeueAsync(string id, CancellationToken cancelToken);
        Task EnqueueAsync(string id, string message, CancellationToken cancelToken);

    }
    public class MessageQueue : IMessageQueue
    {
        private ConcurrentDictionary<string, Channel<string>> clientToChannelMap;
        public MessageQueue()
        {
            clientToChannelMap = new ConcurrentDictionary<string, Channel<string>>();
        }

        public async Task<string> DequeueAsync(string id, CancellationToken cancelToken)
        {
            if (clientToChannelMap.TryGetValue(id, out Channel<string> channel))
            {
                return await channel.Reader.ReadAsync(cancelToken);
            }
            else
            {
                throw new ArgumentException($"Id {id} isn't registered");
            }
        }

        public async Task EnqueueAsync(string id, string message, CancellationToken cancelToken)
        {
            if (clientToChannelMap.TryGetValue(id, out Channel<string> channel))
            {
                await channel.Writer.WriteAsync(message, cancelToken);
            }
        }

        public void Register(string id)
        {
            if (!clientToChannelMap.TryAdd(id, Channel.CreateUnbounded<string>()))
            {
                throw new ArgumentException($"Id {id} is already registered");
            }
        }

        public void Unregister(string id)
        {
            clientToChannelMap.TryRemove(id, out _);
        }

        private Channel<string> CreateChannel()
        {
            return Channel.CreateUnbounded<string>();
        }
    }
}