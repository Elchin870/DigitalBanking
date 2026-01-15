using DigitalBanking.Core.Entities;
using DigitalBanking.Core.Enums;
using DigitalBanking.Core.Interfaces.Services;
using DigitalBanking.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace DigitalBanking.Infrastructure.Services;

public class DepositService : IDepositService
{
    private readonly DigitalBankingDbContext _context;
    private readonly INotificationService _notificationService;
    public DepositService(DigitalBankingDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }
    public async Task<string> CreateDepositIntentAsync(string userId, decimal amount)
    {
        if (amount <= 0)
            throw new Exception("Invalid amount");

        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (account == null)
            throw new Exception("Account not found");

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = "usd",
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId },
                { "accountId", account.Id.ToString() }
            }
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);

        return intent.ClientSecret;
    }

    public async Task HandlePaymentSucceededAsync(PaymentIntent intent)
    {
        var exists = await _context.Transactions
            .AnyAsync(t => t.StripePaymentIntentId == intent.Id);

        if (exists)
            return;

        var accountId = int.Parse(intent.Metadata["accountId"]);
        var userId = intent.Metadata["userId"];
        var amount = intent.Amount / 100m;

        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
            return;

        account.Balance += amount;

        await _context.Transactions.AddAsync(new Transaction
        {
            ToAccountId = account.Id,
            Amount = amount,
            Type = TransactionType.Deposit,
            StripePaymentIntentId = intent.Id,
            CreatedAt = DateTime.UtcNow.AddHours(4)
        });

        await _context.SaveChangesAsync();

        await _notificationService.SendBalanceUpdateAsync(userId,account.Balance);

        await _notificationService.SendToUserAsync(userId,$"Deposit successful: +{amount}₼");
    }
}
