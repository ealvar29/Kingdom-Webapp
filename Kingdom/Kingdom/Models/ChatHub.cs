using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Kingdom.Models
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, ChatRoom> chatRooms = new ConcurrentDictionary<string, ChatRoom>();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

        }

        public async Task CreateRoom(string roomName, string password)
        {
            var chatRoom = new ChatRoom { RoomName = roomName, Password = password };
            chatRooms.TryAdd(roomName, chatRoom);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Caller.SendAsync("RoomCreated", roomName);
        }

        public async Task JoinRoom(string roomName, string password)
        {
            if (chatRooms.TryGetValue(roomName, out var room))
            {
                if (room.Password == password)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    await Clients.Caller.SendAsync("JoinedRoom", roomName);
                    await Clients.Group(roomName).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has joined the room.");
                }
                else
                {
                    await Clients.Caller.SendAsync("Error", "Wrong password");
                }
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Room does not exist");
            }
        }

        public async Task SendMessageToRoom(string roomName, string message)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }
    }
}
