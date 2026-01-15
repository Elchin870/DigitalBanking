namespace DigitalBanking.Core.Dtos;

public class TransferRequestDto
{
    public int FromCardId { get; set; }
    public string ToCardNumber { get; set; }
    public decimal Amount { get; set; }
}
