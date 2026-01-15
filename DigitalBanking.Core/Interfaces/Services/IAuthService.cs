using DigitalBanking.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DigitalBanking.Core.Interfaces.Services;

public interface IAuthService
{
    Task<(bool IsSuccess, string Message)> Register([FromBody] RegisterDto dto, string? avatarUrl);
    Task<(bool IsSuccess, string Token, DateTime Expiration)> Login(LoginDto dto);
    JwtSecurityToken GetToken(List<Claim> authClaims);
    Task<(bool IsSuccess, string Message)> CreateAdminAsync();
    Task LogoutAsync();
}
