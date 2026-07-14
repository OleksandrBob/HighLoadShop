using UserService.Application.Models;

namespace UserService.Application.Interfaces;

public interface IUserApplicationService
{
    Task<UserProfile> RegisterAsync(RegisterUserInput request, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginAsync(LoginUserInput request, CancellationToken cancellationToken = default);
}