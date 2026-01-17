namespace DigitalBanking.Core.Interfaces.Services;

public interface IErrorLogService
{
    Task SaveAsync(Exception ex);
}
