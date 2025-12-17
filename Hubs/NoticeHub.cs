using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SchoolWebsite1.Hubs
{
    public class NoticeHub : Hub
    {
        // You can add server-side methods here if needed
        public async Task SendNotice(string title, string message, string createdAt)
        {
            await Clients.All.SendAsync("ReceiveNotice", title, message, createdAt);
        }
    }
}
