using DigitalBanking.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Core.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int? FromAccountId { get; set; }
    public Account? FromAccount { get; set; }
    public int? ToAccountId { get; set; }
    public Account? ToAccount { get; set; }

    public int? FromCardId { get; set; }
    public VirtualCard? FromCard { get; set; }
    public int? ToCardId { get; set; }
    public VirtualCard? ToCard { get; set; }

    [Precision(10, 2)]
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public DateTime CreatedAt { get; set; }
}
