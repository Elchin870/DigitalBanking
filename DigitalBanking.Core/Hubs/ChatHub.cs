using DigitalBanking.Core.Entities;
using DigitalBanking.Core.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DigitalBanking.Core.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatRepository _repo;

    public ChatHub(IChatRepository repo)
    {
        _repo = repo;
    }

    public async Task SendMessage(string toUserId, string message)
    {
        var fromUserId = Context.UserIdentifier;
        if (string.IsNullOrEmpty(fromUserId))
            throw new HubException("Unauthorized");

        var chatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Text = message,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.SaveAsync(chatMessage);

        await Clients.User(toUserId)
            .SendAsync("ReceiveMessage", new
            {
                From = fromUserId,
                Text = message,
                Time = chatMessage.CreatedAt
            });
    }
}
