using DigitalBanking.Core.Entities;
using DigitalBanking.Core.Enums;
using DigitalBanking.Core.Interfaces.Services;
using DigitalBanking.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace DigitalBanking.Infrastructure.Services;

public class TransferService : ITransferService
{
    private readonly DigitalBankingDbContext _context;
    private readonly INotificationService _notificationService;
    public TransferService(DigitalBankingDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<(bool IsSuccess, string Message)> TransferAsync(string senderUserId, int fromCardId, string toCardNumber, decimal amount)
    {
        if (amount <= 0)
            return (false, "Amount must be greater than 0");

        using var dbTx = await _context.Database.BeginTransactionAsync();

        try
        {
            if (toCardNumber.Length != 16)
            {
                return (false, "Card number must be 16 digit!");
            }
            var fromCard = await _context.VirtualCards
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Id == fromCardId && c.IsActive);

            if (fromCard == null)
                return (false, "Sender card not found or inactive");

            if (fromCard.Account.UserId != senderUserId)
                return (false, "You cannot use this card");

            var fromAccount = fromCard.Account;

            var toCard = await _context.VirtualCards
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.CardNumber == toCardNumber && c.IsActive);

            if (toCard == null)
                return (false, "Receiver card not found or inactive");

            var toAccount = toCard.Account;

            if (fromAccount.Id == toAccount.Id)
                return (false, "You cannot transfer to your own account");

            if (fromAccount.Balance < amount)
                return (false, "Insufficient balance");

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            var tx = new Transaction
            {
                FromAccountId = fromAccount.Id,
                ToAccountId = toAccount.Id,
                FromCardId = fromCard.Id,
                ToCardId = toCard.Id,
                Amount = amount,
                Type = TransactionType.Transfer,
                CreatedAt = DateTime.UtcNow.AddHours(4)
            };

            await _context.Transactions.AddAsync(tx);
            await _context.SaveChangesAsync();
            await dbTx.CommitAsync();

            await _notificationService.SendBalanceUpdateAsync(senderUserId,fromAccount.Balance);

            await _notificationService.SendBalanceUpdateAsync(toAccount.UserId,toAccount.Balance);

            await _notificationService.SendToUserAsync(senderUserId, $"You sent {amount} AZN to card {toCardNumber}");

            await _notificationService.SendToUserAsync(toAccount.UserId, $"You received {amount} AZN from card {fromCard.CardNumber}");

            return (true, "Transfer completed successfully");
        }
        catch (Exception)
        {
            await dbTx.RollbackAsync();
            return (false, "Transfer failed");
        }
    }

}
