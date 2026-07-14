namespace UserService.Api.Models;

public sealed record LoginResponse(
    UserResponse User,
    JwtTokenResponse Token);