using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Autobarn.Website.Hubs {
    public class AutobarnHub : Hub {
        public async Task NotifyWebUsers(string user, string message) {
            await Clients.All.SendAsync("DoSomethingReallyCool", user, message);
        }
    }
}
