using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Services;

public sealed class UserApplicationService(
    IUserRepository userRepository,
    IAuth0IdentityService auth0IdentityService,
    IJwtTokenService jwtTokenService) : IUserApplicationService
{
    public async Task<UserProfile> RegisterAsync(RegisterUserInput request, CancellationToken cancellationToken = default)
    {
        var profile = await auth0IdentityService.RegisterAsync(request, cancellationToken);
        var user = await userRepository.GetByAuth0SubjectAsync(profile.Subject, cancellationToken)
            ?? await userRepository.GetByEmailAsync(profile.Email, cancellationToken);

        if (user is null)
        {
            user = User.Create(profile.Subject, profile.Email, profile.FirstName, profile.LastName);
            userRepository.Add(user);
        }
        else
        {
            user.UpdateMetadata(profile.Email, profile.FirstName, profile.LastName);
        }

        await userRepository.SaveChangesAsync(cancellationToken);

        return MapUser(user);
    }

    public async Task<LoginResult> LoginAsync(LoginUserInput request, CancellationToken cancellationToken = default)
    {
        var profile = await auth0IdentityService.LoginAsync(request, cancellationToken);

        var user = await userRepository.GetByAuth0SubjectAsync(profile.Subject, cancellationToken)
            ?? await userRepository.GetByEmailAsync(profile.Email, cancellationToken);

        if (user is null)
        {
            user = User.Create(profile.Subject, profile.Email, profile.FirstName, profile.LastName);
            userRepository.Add(user);
        }
        else
        {
            user.UpdateMetadata(profile.Email, profile.FirstName, profile.LastName);
        }

        user.MarkLoggedIn();
        await userRepository.SaveChangesAsync(cancellationToken);

        var token = jwtTokenService.GenerateToken(user);

        return new LoginResult(MapUser(user), token);
    }

    private static UserProfile MapUser(User user)
    {
        return new UserProfile(
            user.Id,
            user.Auth0Subject,
            user.Email,
            user.FirstName,
            user.LastName,
            user.CreatedAt,
            user.UpdatedAt,
            user.LastLoginAt);
    }
}