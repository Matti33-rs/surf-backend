namespace Surf.Api.Options;

public class RedisOptions
{
    public const string SectionName = "Redis";

    /// <summary>Prefix applied to every key written by this service.</summary>
    public string InstanceName { get; set; } = "surf:";
}
