using Surf.Api.Data;
using Surf.Api.Models.WhatsApp;

namespace Surf.Api.Services;

public class WhatsAppWebhookService(AppDbContext db, ILogger<WhatsAppWebhookService> logger)
{
    public Task EnqueueAsync(WebhookPayload payload, CancellationToken cancellationToken)
    {
        foreach (var change in payload.Entry.SelectMany(e => e.Changes))
        {
            foreach (var message in change.Value.Messages)
            {
                logger.LogInformation(
                    "WhatsApp message {MessageId} from {From} ({Type}): {Body}",
                    message.Id, message.From, message.Type, message.Text?.Body);
            }

            foreach (var status in change.Value.Statuses)
            {
                logger.LogInformation(
                    "WhatsApp status {Status} for message {MessageId} to {RecipientId}",
                    status.Status, status.Id, status.RecipientId);
            }
        }

        // TODO: persist messages via `db` once entities are defined.
        _ = db;
        return Task.CompletedTask;
    }
}
