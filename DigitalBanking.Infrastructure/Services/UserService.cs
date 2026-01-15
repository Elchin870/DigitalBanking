using DigitalBanking.Core.Dtos;
using DigitalBanking.Core.Entities;
using DigitalBanking.Core.Interfaces.Services;
using DigitalBanking.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly DigitalBankingDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileService _fileService;
    public UserService(UserManager<AppUser> userManager, IFileService fileService, DigitalBankingDbContext dbContext)
    {
        _userManager = userManager;
        _fileService = fileService;
        _context = dbContext;
    }
    public async Task<decimal> GetBalance(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return 0;
        }
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == userId);
        if (account == null) { return 0; }
        return account.Balance;
    }

    public async Task<List<CardDto>> GetMyCardsAsync(string userId)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
        if (account == null)
            return new List<CardDto>();

        var cards = await _context.VirtualCards
            .Where(c => c.AccountId == account.Id && c.IsActive)
            .Select(c => new CardDto
            {
                Id = c.Id,
                CardNumber = c.CardNumber
            })
            .ToListAsync();

        return cards;
    }

    public async Task<UserProfileDto?> GetProfileInfosAsync(string userId)
    {
        var info = await _context.Users.Include(x => x.Accounts).Where(x => x.Id == userId).Select(x => new UserProfileDto
    {
        Lastname = x.Lastname,
        Firstname = x.Firstname,
        Address = x.Address,
        Age = x.Age,
        Email = x.Email!,
        UserName = x.UserName!,
        PhotoUrl=x.ProfilePhoto,
        AccountId = x.Accounts.Select(a => a.Id).FirstOrDefault()
    }).FirstOrDefaultAsync();


        return info;

    }


    public async Task<List<TransactionDto>> GetTransactionInfoAsync(string userId)
    {
        var transactions = await _context.Transactions
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount)
            .Include(t => t.FromCard)
            .Include(t => t.ToCard)
            .Where(t =>
                t.FromAccount.UserId == userId ||
                t.ToAccount.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto
            {
                TransactionId = t.Id,
                FromCard = t.FromCard.CardNumber,
                ToCard = t.ToCard.CardNumber,
                Amount = t.Amount,
                Type=t.Type,
                ExecutionTime = t.CreatedAt
            })
            .ToListAsync();

        return transactions;
    }


    public async Task UpdateProdileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            user.Firstname = dto.Firstname;
            user.Lastname = dto.Lastname;
            user.Address = dto.Address;
            user.Age = dto.Age;
            user.Email = dto.Email;
            user.UserName = dto.UserName;
            user.ProfilePhoto = dto.PhotoUrl;
            await _userManager.UpdateAsync(user);
        }
    }

    public async Task UpdateProfileAvatarAsync(string userId, string avatar)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User is not found");

        _fileService.DeleteFile(user.ProfilePhoto);

        user.ProfilePhoto = avatar;

        await _userManager.UpdateAsync(user);

    }
}
