namespace Surf.Api.Options;

public class WhatsAppOptions
{
    public const string SectionName = "WhatsApp";

    /// <summary>Token configured in the Meta app dashboard to verify the webhook subscription.</summary>
    public string VerifyToken { get; set; } = string.Empty;

    /// <summary>Meta app secret, used to validate the X-Hub-Signature-256 header.</summary>
    public string AppSecret { get; set; } = string.Empty;

    /// <summary>Access token for calling the WhatsApp Cloud API (sending messages).</summary>
    public string AccessToken { get; set; } = string.Empty;

    public string PhoneNumberId { get; set; } = string.Empty;

    public string ApiVersion { get; set; } = "v23.0";
}
