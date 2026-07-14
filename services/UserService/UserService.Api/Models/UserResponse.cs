namespace UserService.Api.Models;

public sealed record UserResponse(
    Guid Id,
    string Auth0Subject,
    string Email,
    string? FirstName,
    string? LastName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? LastLoginAt);