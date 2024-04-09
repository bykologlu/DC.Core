using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace DC.Core.Socket.Signalr
{
    public class SignalRHub : Hub
    {
        public async Task SendMessage(object message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            await Clients.All.SendAsync("ReceiveMessage", jsonMessage);
        }
    }
}
