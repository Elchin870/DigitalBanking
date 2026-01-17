using DigitalBanking.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Contexts;

public class DigitalBankingDbContext:IdentityDbContext<AppUser>
{
    public DigitalBankingDbContext(DbContextOptions options):base(options)
    {     
    }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<VirtualCard> VirtualCards { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }
}
