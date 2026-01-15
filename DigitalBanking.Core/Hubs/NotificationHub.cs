using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DigitalBanking.Core.Hubs;

public class NotificationHub:Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId=Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await base.OnConnectedAsync();
    }
}
