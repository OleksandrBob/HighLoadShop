namespace UserService.Application.Options;

public sealed class Auth0Options
{
    public const string SectionName = "Auth0";

    public string Domain { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Connection { get; set; } = string.Empty;
    public string? Audience { get; set; }
    public string IdTokenSigningAlgorithm { get; set; } = "RS256";

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Domain) &&
        !string.IsNullOrWhiteSpace(ClientId) &&
        !string.IsNullOrWhiteSpace(Connection);

    public string Authority => $"https://{Domain.Trim().TrimEnd('/')}/";
}