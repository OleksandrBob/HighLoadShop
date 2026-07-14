using UserService.Application.Models;

namespace UserService.Application.Interfaces;

public interface IAuth0IdentityService
{
    Task<Auth0UserProfile> RegisterAsync(RegisterUserInput request, CancellationToken cancellationToken = default);
    Task<Auth0UserProfile> LoginAsync(LoginUserInput request, CancellationToken cancellationToken = default);
}