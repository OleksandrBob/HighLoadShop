namespace UserService.Api.Models;

public sealed record LoginUserRequest(string Email, string Password);