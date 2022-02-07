using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string , List<string>> onlineUsers = new Dictionary<string,List<string>>();
        public Task UsersConnected(string userName,string connectionId)
        {
            lock(onlineUsers)
            {
                if(onlineUsers.ContainsKey(userName))
                {
                    onlineUsers[userName].Add(connectionId); //if user is already in dictionary
                }
                else{
                    onlineUsers.Add(userName,new List<string>{connectionId});
                }
            }
            return Task.CompletedTask;
        }
        public Task UserDisconnected(string userName,string connectionId)
        {
            lock(onlineUsers)
            {
                if(!onlineUsers.ContainsKey(userName)) return Task.CompletedTask;
                onlineUsers[userName].Remove(connectionId);
                if(onlineUsers[userName].Count==0)
                {
                    onlineUsers.Remove(userName);
                }
            }
            return Task.CompletedTask;
        }
        public Task<string[]> getOnlineUsers()
        {
            string[] usersOnline;
            lock(onlineUsers)
            {
                usersOnline=onlineUsers.OrderBy(k=>k.Key).Select(k=>k.Key).ToArray();
            }
            return Task.FromResult(usersOnline);
        }
    }
}