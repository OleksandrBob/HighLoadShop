using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Persistence.Repositories;

public class UserRepository(UserDbContext context) : IUserRepository
{
    public Task<User?> GetByAuth0SubjectAsync(string auth0Subject, CancellationToken cancellationToken = default)
    {
        return context.Users.FirstOrDefaultAsync(user => user.Auth0Subject == auth0Subject, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return context.Users.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}