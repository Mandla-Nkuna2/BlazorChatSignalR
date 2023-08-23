using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace BlazorChatSignalR.Server.Hubs;
public class ChatHub : Hub
{
    private static Dictionary<string, string> Users = new Dictionary<string, string>();
    public override async Task OnConnectedAsync()
    {
        string username = Context.GetHttpContext().Request.Query["username"];
        Users.Add(Context.ConnectionId, username);

        await SendMessage(string.Empty, $"{username} connected!");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string username = Users.FirstOrDefault(user => user.Key == Context.ConnectionId).Value;
        await SendMessage(string.Empty, $"{username} disconnected.");
    }
    public async Task SendMessage(string user,  string message)
    {
        Debug.WriteLine("SendMessage {user}: {message}");
        Debug.WriteLine($"SendMessage {user}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
