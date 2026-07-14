using Microsoft.AspNetCore.Mvc;
using UserService.Api.Filters;
using UserService.Api.Models;
using UserService.Application.Interfaces;
using UserService.Application.Models;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Produces("application/json")]
public class UsersController(IUserApplicationService userApplicationService) : ControllerBase
{
    [HttpPost("register")]
    [TypeFilter(typeof(ValidationActionFilter<RegisterUserRequest>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userApplicationService.RegisterAsync(
            new RegisterUserInput(request.Email, request.Password, request.FirstName, request.LastName),
            cancellationToken);

        return Ok(Map(result));
    }

    [HttpPost("login")]
    [TypeFilter(typeof(ValidationActionFilter<LoginUserRequest>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userApplicationService.LoginAsync(new LoginUserInput(request.Email, request.Password), cancellationToken);
        return Ok(Map(result));
    }

    private static UserResponse Map(UserProfile profile)
    {
        return new UserResponse(
            profile.Id,
            profile.Auth0Subject,
            profile.Email,
            profile.FirstName,
            profile.LastName,
            profile.CreatedAt,
            profile.UpdatedAt,
            profile.LastLoginAt);
    }

    private static LoginResponse Map(LoginResult result)
    {
        return new LoginResponse(
            Map(result.User),
            new JwtTokenResponse(result.Token.AccessToken, result.Token.ExpiresAt));
    }
}