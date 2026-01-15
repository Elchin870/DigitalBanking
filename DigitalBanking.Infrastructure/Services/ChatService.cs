using DigitalBanking.Core.Hubs;
using DigitalBanking.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace DigitalBanking.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly IHubContext<ChatHub> _hub;

    public ChatService(IHubContext<ChatHub> hub)
    {
        _hub = hub;
    }

    public async Task SendMessageAsync(string fromUserId, string toUserId, string message)
    {
        await _hub.Clients.User(toUserId)
            .SendAsync("ReceiveMessage", new
            {
                From = fromUserId,
                Text = message,
                Time = DateTime.UtcNow
            });
    }
}
