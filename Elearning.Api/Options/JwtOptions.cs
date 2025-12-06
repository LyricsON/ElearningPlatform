namespace Elearning.Api.Options;

/// <summary>
/// JWT settings loaded from configuration.
/// </summary>
public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "ElearningPlatform";
    public string Audience { get; set; } = "ElearningPlatform";
    public string Key { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 120;
}
