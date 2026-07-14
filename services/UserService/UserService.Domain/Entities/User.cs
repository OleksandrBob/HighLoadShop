namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Auth0Subject { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; }

    private User() { }

    private User(string auth0Subject, string email, string? firstName, string? lastName)
    {
        Id = Guid.NewGuid();
        Auth0Subject = Normalize(auth0Subject);
        Email = Normalize(email);
        FirstName = NormalizeOptional(firstName);
        LastName = NormalizeOptional(lastName);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        IsActive = true;
    }

    public static User Create(string auth0Subject, string email, string? firstName, string? lastName)
    {
        return new User(auth0Subject, email, firstName, lastName);
    }

    public void UpdateMetadata(string email, string? firstName, string? lastName)
    {
        Email = Normalize(email);
        FirstName = NormalizeOptional(firstName);
        LastName = NormalizeOptional(lastName);
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkLoggedIn()
    {
        LastLoginAt = DateTime.UtcNow;
        UpdatedAt = LastLoginAt.Value;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string Normalize(string value)
    {
        var normalized = value?.Trim();

        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new ArgumentException("Value cannot be empty.", nameof(value));
        }

        return normalized;
    }

    private static string? NormalizeOptional(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}