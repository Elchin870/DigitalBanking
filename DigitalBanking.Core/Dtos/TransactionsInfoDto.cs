using DigitalBanking.Core.Enums;

namespace DigitalBanking.Core.Dtos;

public class TransactionsInfoDto
{
    public int TransactionId { get; set; }
    public string FromCard { get; set; }
    public string ToCard { get; set; }
    public int? FromAccountId { get; set; }
    public int? ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime ExecutionTime { get; set; }
}
