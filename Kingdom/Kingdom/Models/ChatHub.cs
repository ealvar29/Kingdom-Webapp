using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Kingdom.Models
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, ChatRoom> chatRooms = new ConcurrentDictionary<string, ChatRoom>();
        private static List<string> roles = new List<string> { "King", "Knight", "Bandit", "Bandit", "Assassin" };

        public override Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task CreateRoom(string roomName, string password, int numOfPlayers)
        {
            OnConnectedAsync();
            var chatRoom = new ChatRoom { RoomName = roomName, Password = password, Players = numOfPlayers };
            chatRooms.TryAdd(roomName, chatRoom);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            CreatedRoom createdRoom = new CreatedRoom()
            {
                Name = roomName,
                RoomId = Context.ConnectionId,
                RoomParticipants = UserHandler.ConnectedIds.Count,
                NumberOfPlayers = numOfPlayers
            };
            await Clients.Caller.SendAsync("RoomCreated", createdRoom);
        }

        public async Task JoinRoom(string roomName, string password)
        {
            if (chatRooms.TryGetValue(roomName, out var room))
            {
                if (room.Password == password)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    CreatedRoom createdRoom = new CreatedRoom()
                    {
                        Name = roomName,
                        RoomId = Context.ConnectionId,
                        RoomParticipants = UserHandler.ConnectedIds.Count,
                    };
                    await Clients.Caller.SendAsync("JoinedRoom", createdRoom);
                    await Clients.Group(roomName).SendAsync("ReceiveMessage", createdRoom);

                    if (room.Players == UserHandler.ConnectedIds.Count)
                    {
                        /*if (UserHandler.ConnectedIds.Count > 5)
                        {
                            roles.Add("Usurper");
                        }*/
                        AssignRoles(roomName);
                    }
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

        private async Task AssignRoles(string roomName)
        {
            if (chatRooms.TryGetValue(roomName, out var room))
            {
                var playerIds = UserHandler.ConnectedIds.Take(room.Players).ToList();
                var shuffledRoles = roles.OrderBy(x => Guid.NewGuid()).Take(room.Players).ToList();

                for (int i = 0; i < playerIds.Count; i++)
                {
                    await Clients.Client(playerIds[i]).SendAsync("ReceiveRole", shuffledRoles[i]);
                }
            }
        }

        public async Task SendMessageToRoom(string roomName, string message)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }
    }
}
