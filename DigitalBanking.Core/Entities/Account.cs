using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Core.Entities;

public class Account
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    [Precision(10,2)]
    public decimal Balance { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<VirtualCard> Cards { get; set; }=new List<VirtualCard>();

}
