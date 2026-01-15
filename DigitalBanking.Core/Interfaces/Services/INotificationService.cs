namespace DigitalBanking.Core.Interfaces.Services;

public interface INotificationService
{
    Task SendToUserAsync(string userId, string message);
    Task SendBalanceUpdateAsync(string userId, decimal newBalance);

}
