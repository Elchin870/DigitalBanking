using DigitalBanking.Core.Dtos;
using DigitalBanking.Core.Entities;
using DigitalBanking.Core.Helpers;
using DigitalBanking.Core.Interfaces.Services;
using DigitalBanking.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalBanking.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly DigitalBankingDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, DigitalBankingDbContext context, IConfiguration configuration, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
        _signInManager = signInManager;
    }

    public async Task<(bool IsSuccess, string Message)> CreateAdminAsync()
    {
        var isExistAdmin = await _userManager.FindByNameAsync("admin");
        if (isExistAdmin != null)
        {
            return (false, "Admin is already exist");
        }

        var admin = new AppUser()
        {
            UserName = "admin",
            Email = "admin@gmail.com",
            Firstname = "admin",
            Lastname = "admin",
            Address = "admin",
            Age = 18
        };

        var result = await _userManager.CreateAsync(admin, "Admin123!");

        if (!result.Succeeded)
            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new IdentityRole("Admin"));

        await _userManager.AddToRoleAsync(admin, "Admin");

        return (true, "Admin created successfully");

    }

    public JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    public async Task<(bool IsSuccess, string Token, DateTime Expiration)> Login(LoginDto dto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserName == dto.Username);

        if (user == null)
            return (false, null, DateTime.MinValue);

        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValid)
            return (false, null, DateTime.MinValue);

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = GetToken(authClaims);

        return (
            true,
            new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo
        );
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<(bool IsSuccess, string Message)> Register(RegisterDto dto, string? avatarUrl)
    {
        var user = new AppUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            Address = dto.Address,
            Age = dto.Age,
            ProfilePhoto = avatarUrl
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!await _roleManager.RoleExistsAsync("User"))
            await _roleManager.CreateAsync(new IdentityRole("User"));

        await _userManager.AddToRoleAsync(user, "User");

        var account = new Account
        {
            UserId = user.Id,
            Balance = 0,
            CreatedAt = DateTime.UtcNow
        };
        _context.Accounts.Add(account);

        var card = new VirtualCard
        {
            CardNumber = CardGenerator.GenerateCardNumber(),
            Account = account
        };
        _context.VirtualCards.Add(card);

        await _context.SaveChangesAsync();

        return (true, "User created successfully");
    }
}
