using System.Threading.Channels;
using Surf.Api.Models.WhatsApp;

namespace Surf.Api.Queues;

public class Queue
{
    private readonly Channel<WebhookMessage> _queue = Channel.CreateUnbounded<WebhookMessage>();


    public async Task EnqueueAsync(WebhookMessage message)
    {
        await _queue.Writer.WriteAsync(message);
    }

    public async Task<WebhookMessage?> Dequeue()
    {
        if (_queue.Reader.Count > 0)
        {
            var message = await _queue.Reader.ReadAsync();

            return message;
        }

        return null;
    }
}