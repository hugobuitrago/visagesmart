namespace VisageSmart.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "VisageSmart";
    public string Audience { get; set; } = "VisageSmart.Client";
    public string Key { get; set; } = "change-this-development-key-with-at-least-32-chars";
    public int ExpirationInMinutes { get; set; } = 120;
}
