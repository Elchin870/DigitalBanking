using DigitalBanking.Core.Dtos;
using DigitalBanking.Core.Interfaces.Services;
using DigitalBanking.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly DigitalBankingDbContext _context;

    public AdminService(DigitalBankingDbContext context)
    {
        _context = context;
    }

    public async Task<List<TransactionsInfoDto>> GetAllTransactionsAsync()
    {
        var transactions = await _context.Transactions.Select(x => new TransactionsInfoDto
        {
            TransactionId = x.Id,
            FromCard = x.FromCard!.CardNumber,
            ToCard = x.ToCard!.CardNumber,
            Amount = x.Amount,
            Type = x.Type,
            FromAccountId=x.FromAccountId,
            ToAccountId=x.ToAccountId,
            ExecutionTime = x.CreatedAt,
        }).ToListAsync();



        return transactions;
    }


    async Task<List<UserDto>> IAdminService.GetAllUsersAsync()
    {
        var users = await _context.Users.Select(x => new UserDto
        {
            Id = x.Id,
            UserName = x.UserName!,
            Email = x.Email!,
            Address = x.Address,
            Age = x.Age,
            Firstname = x.Firstname,
            Lastname = x.Lastname,
            PhotoUrl = x.ProfilePhoto
        }).ToListAsync();


        return (users);
    }
}
