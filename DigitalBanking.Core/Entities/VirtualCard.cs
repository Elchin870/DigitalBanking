namespace DigitalBanking.Core.Entities;

public class VirtualCard
{
    public int Id { get; set; }
    public string CardNumber { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }

    public bool IsActive { get; set; } = true;

}
