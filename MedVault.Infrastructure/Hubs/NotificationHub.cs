using Microsoft.AspNetCore.SignalR;

namespace MedVault.Infrastructure.Hubs;

public class NotificationHub : Hub
{
    public async Task JoinUser(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }
}
