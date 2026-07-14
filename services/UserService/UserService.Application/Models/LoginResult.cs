namespace UserService.Application.Models;

public sealed record LoginResult(
    UserProfile User,
    JwtTokenResult Token);