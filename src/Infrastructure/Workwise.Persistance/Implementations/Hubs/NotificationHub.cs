using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Workwise.Application.Dtos.Hubs;

namespace Workwise.Persistance.Implementations.Hubs
{
    public class NotificationHub : Hub
    {
        public static List<UserConnectionDto> Connections = new();

        public override Task OnConnectedAsync()
        {
            AddConnectionId();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            RemoveConnectionIds();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveNotificationMessage", message);
        }

        private void AddConnectionId()
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserConnectionDto connection = Connections.FirstOrDefault(x => x.AppUserId == userId);
            if (connection is null)
            {
                connection = new UserConnectionDto() { AppUserId = userId, ConnectionIds = new() { Context.ConnectionId } };
                Connections.Add(connection);
            }
            else
            {
                connection.ConnectionIds.Add(Context.ConnectionId);
            }
        }

        private void RemoveConnectionIds()
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Connections.RemoveAll(x => x.AppUserId == userId);
        }
    }
}
