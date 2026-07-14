namespace UserService.Application.Models;

public sealed record JwtTokenResult(
    string AccessToken,
    DateTime ExpiresAt);