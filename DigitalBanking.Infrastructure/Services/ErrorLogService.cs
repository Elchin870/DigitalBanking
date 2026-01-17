using DigitalBanking.Core.Entities;
using DigitalBanking.Core.Interfaces.Services;
using DigitalBanking.Infrastructure.Contexts;

namespace DigitalBanking.Infrastructure.Services;

public class ErrorLogService : IErrorLogService
{
    private readonly DigitalBankingDbContext _context;

    public ErrorLogService(DigitalBankingDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(Exception ex)
    {
        var log = new ErrorLog
        {
            Message = ex.Message,
            StackTrace = ex.StackTrace,
            CreatedAt = DateTime.UtcNow.AddHours(4)
        };

        await _context.ErrorLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
