using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Surf.Api.Options;

namespace Surf.Api.Filters;

/// <summary>
/// Validates the Meta X-Hub-Signature-256 header against the raw request body.
/// Implemented as a resource filter so it runs before model binding consumes the body.
/// </summary>
public class WhatsAppSignatureAttribute() : TypeFilterAttribute(typeof(WhatsAppSignatureFilter));

public class WhatsAppSignatureFilter(
    IOptions<WhatsAppOptions> options,
    ILogger<WhatsAppSignatureFilter> logger) : IAsyncResourceFilter
{
    private const string SignatureHeader = "X-Hub-Signature-256";
    private const string SignaturePrefix = "sha256=";

    private readonly WhatsAppOptions _options = options.Value;

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        if (string.IsNullOrEmpty(_options.AppSecret))
        {
            logger.LogWarning("WhatsApp:AppSecret is not configured; skipping signature validation");
            await next();
            return;
        }

        var request = context.HttpContext.Request;
        request.EnableBuffering();

        using var buffer = new MemoryStream();
        await request.Body.CopyToAsync(buffer, context.HttpContext.RequestAborted);
        request.Body.Position = 0;

        if (!IsSignatureValid(buffer.ToArray(), request.Headers[SignatureHeader].ToString()))
        {
            logger.LogWarning("WhatsApp webhook signature validation failed");
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }

    private bool IsSignatureValid(byte[] body, string signatureHeader)
    {
        if (!signatureHeader.StartsWith(SignaturePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        byte[] received;
        try
        {
            received = Convert.FromHexString(signatureHeader[SignaturePrefix.Length..]);
        }
        catch (FormatException)
        {
            return false;
        }

        var expected = HMACSHA256.HashData(Encoding.UTF8.GetBytes(_options.AppSecret), body);
        return CryptographicOperations.FixedTimeEquals(expected, received);
    }
}
