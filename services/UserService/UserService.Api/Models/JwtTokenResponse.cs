namespace UserService.Api.Models;

public sealed record JwtTokenResponse(
    string AccessToken,
    DateTime ExpiresAt);