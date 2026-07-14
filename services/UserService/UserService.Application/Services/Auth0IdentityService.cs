using System.IdentityModel.Tokens.Jwt;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Extensions.Options;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Application.Options;

namespace UserService.Application.Services;

public sealed class Auth0IdentityService(IOptions<Auth0Options> options) : IAuth0IdentityService
{
    private readonly Auth0Options _options = options.Value;

    public async Task<Auth0UserProfile> RegisterAsync(RegisterUserInput request, CancellationToken cancellationToken = default)
    {
        EnsureConfigured();

        using var client = new AuthenticationApiClient(_options.Domain);
        var response = await client.SignupUserAsync(new SignupUserRequest
        {
            ClientId = _options.ClientId,
            Connection = _options.Connection,
            Email = request.Email,
            Password = request.Password,
            GivenName = request.FirstName ?? string.Empty,
            FamilyName = request.LastName ?? string.Empty,
            Name = BuildFullName(request.FirstName, request.LastName),
            Nickname = request.FirstName ?? request.Email,
        }, cancellationToken);

        return new Auth0UserProfile(
            response.Id ?? string.Empty,
            response.Email ?? request.Email,
            response.GivenName,
            response.FamilyName,
            response.Name);
    }

    public async Task<Auth0UserProfile> LoginAsync(LoginUserInput request, CancellationToken cancellationToken = default)
    {
        EnsureConfigured();

        using var client = new AuthenticationApiClient(_options.Domain);
        var tokenResponse = await client.GetTokenAsync(new ResourceOwnerTokenRequest
        {
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            Realm = _options.Connection,
            Username = request.Email,
            Password = request.Password,
            Scope = "openid profile email",
            Audience = _options.Audience,
            SigningAlgorithm = ParseSigningAlgorithm(_options.IdTokenSigningAlgorithm)
        }, cancellationToken);

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(tokenResponse.IdToken);
        var subject = jwt.Subject ?? jwt.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value
            ?? throw new InvalidOperationException("Auth0 ID token is missing the subject claim.");
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value ?? request.Email;
        string? firstName = jwt.Claims.FirstOrDefault(claim => claim.Type == "given_name")?.Value;
        string? lastName = jwt.Claims.FirstOrDefault(claim => claim.Type == "family_name")?.Value;
        string? fullName = jwt.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;

        return new Auth0UserProfile(subject, email, firstName, lastName, fullName);
    }

    private void EnsureConfigured()
    {
        if (!_options.IsConfigured)
        {
            throw new InvalidOperationException("Auth0 is not configured.");
        }
    }

    private static string BuildFullName(string? firstName, string? lastName)
    {
        return string.Join(" ", new[] { firstName, lastName }.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    private static JwtSignatureAlgorithm ParseSigningAlgorithm(string value)
    {
        return Enum.TryParse<JwtSignatureAlgorithm>(value, ignoreCase: true, out var parsed)
            ? parsed
            : JwtSignatureAlgorithm.RS256;
    }
}