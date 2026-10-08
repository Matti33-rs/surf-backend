using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Surf.Api.Filters;
using Surf.Api.Models.WhatsApp;
using Surf.Api.Options;
using Surf.Api.Services;

namespace Surf.Api.Controllers;

[ApiController]
[Route("api/hooks/whatsapp")]
public class WhatsAppWebhookController(
    WhatsAppWebhookService webhookService,
    IOptions<WhatsAppOptions> options,
    ILogger<WhatsAppWebhookController> logger) : ControllerBase
{
    private readonly WhatsAppOptions _options = options.Value;

    /// <summary>Meta webhook verification handshake.</summary>
    [HttpGet]
    public IActionResult Verify(
        [FromQuery(Name = "hub.mode")] string? mode,
        [FromQuery(Name = "hub.verify_token")] string? verifyToken,
        [FromQuery(Name = "hub.challenge")] string? challenge)
    {
        if (mode == "subscribe"
            && !string.IsNullOrEmpty(_options.VerifyToken)
            && verifyToken == _options.VerifyToken)
        {
            return Content(challenge ?? string.Empty, "text/plain");
        }

        logger.LogWarning("WhatsApp webhook verification failed (mode: {Mode})", mode);
        return StatusCode(StatusCodes.Status403Forbidden);
    }

    /// <summary>Receives WhatsApp events (messages, statuses) from Meta.</summary>
    [HttpPost]
    [WhatsAppSignature]
    public async Task<IActionResult> Receive([FromBody] WebhookPayload payload, CancellationToken cancellationToken)
    {
        await webhookService.EnqueueAsync(payload, cancellationToken);
        return Ok();
    }
}
