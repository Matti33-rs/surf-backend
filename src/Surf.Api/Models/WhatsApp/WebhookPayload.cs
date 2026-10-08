using System.Text.Json.Serialization;

namespace Surf.Api.Models.WhatsApp;

// Shapes of the Meta WhatsApp Cloud API webhook payload.
// https://developers.facebook.com/docs/whatsapp/cloud-api/webhooks/components

public class WebhookPayload
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("entry")]
    public List<WebhookEntry> Entry { get; set; } = [];
}

public class WebhookEntry
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("changes")]
    public List<WebhookChange> Changes { get; set; } = [];
}

public class WebhookChange
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public WebhookValue Value { get; set; } = new();
}

public class WebhookValue
{
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public WebhookMetadata? Metadata { get; set; }

    [JsonPropertyName("contacts")]
    public List<WebhookContact> Contacts { get; set; } = [];

    [JsonPropertyName("messages")]
    public List<WebhookMessage> Messages { get; set; } = [];

    [JsonPropertyName("statuses")]
    public List<WebhookStatus> Statuses { get; set; } = [];
}

public class WebhookMetadata
{
    [JsonPropertyName("display_phone_number")]
    public string DisplayPhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("phone_number_id")]
    public string PhoneNumberId { get; set; } = string.Empty;
}

public class WebhookContact
{
    [JsonPropertyName("wa_id")]
    public string WaId { get; set; } = string.Empty;

    [JsonPropertyName("profile")]
    public WebhookProfile? Profile { get; set; }
}

public class WebhookProfile
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class WebhookMessage
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public WebhookText? Text { get; set; }
}

public class WebhookText
{
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;
}

public class WebhookStatus
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [JsonPropertyName("recipient_id")]
    public string RecipientId { get; set; } = string.Empty;
}
