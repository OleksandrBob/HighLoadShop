namespace UserService.Application.Models;

public sealed record Auth0UserProfile(
    string Subject,
    string Email,
    string? FirstName,
    string? LastName,
    string? FullName);