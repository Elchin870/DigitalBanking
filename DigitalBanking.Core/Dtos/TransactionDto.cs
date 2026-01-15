using DigitalBanking.Core.Enums;

namespace DigitalBanking.Core.Dtos;

public class TransactionDto
{
    public int TransactionId { get; set; }

    public string FromCard { get; set; }
    public string ToCard { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime ExecutionTime { get; set; }

}
