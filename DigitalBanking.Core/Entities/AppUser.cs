using Microsoft.AspNetCore.Identity;

namespace DigitalBanking.Core.Entities;

public class AppUser:IdentityUser
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }
    public string? ProfilePhoto { get; set; }

    public ICollection<Account> Accounts { get; set; }= new List<Account>();
}
