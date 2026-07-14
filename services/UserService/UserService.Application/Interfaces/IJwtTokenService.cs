using UserService.Application.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IJwtTokenService
{
    JwtTokenResult GenerateToken(User user);
}