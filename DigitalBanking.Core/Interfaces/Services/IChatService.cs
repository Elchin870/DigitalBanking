namespace DigitalBanking.Core.Interfaces.Services;

public interface IChatService
{
    Task SendMessageAsync(string fromUserId, string toUserId, string message);
}
