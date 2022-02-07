using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            _tracker=tracker;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId=Context.ConnectionId;
            string userName=Context.User.GetUserName();
            await _tracker.UsersConnected(userName,connectionId);

            await Clients.Others.SendAsync("UserIsOnline",Context.User.GetUserName());

            var currentUsers=await _tracker.getOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers",currentUsers);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
             await _tracker.UserDisconnected(Context.User.GetUserName(),Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline",Context.User.GetUserName());

            var currentUsers=await _tracker.getOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers",currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}