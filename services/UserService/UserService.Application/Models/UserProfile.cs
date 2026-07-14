namespace UserService.Application.Models;

public sealed record UserProfile(
    Guid Id,
    string Auth0Subject,
    string Email,
    string? FirstName,
    string? LastName,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? LastLoginAt);