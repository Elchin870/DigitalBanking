using DigitalBanking.Core.Hubs;
using DigitalBanking.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace DigitalBanking.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hub = hubContext;
    }

    public async Task SendBalanceUpdateAsync(string userId, decimal newBalance)
    {
        await _hub.Clients.User(userId).SendAsync("BalanceUpdated", newBalance);
    }

    public async Task SendToUserAsync(string userId, string message)
    {
        await _hub.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}
