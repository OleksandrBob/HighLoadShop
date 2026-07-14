namespace UserService.Application.Models;

public sealed record RegisterUserInput(
    string Email,
    string Password,
    string? FirstName,
    string? LastName);