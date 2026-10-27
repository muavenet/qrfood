using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HemenTanitim.Models.ViewModels;
using HemenTanitim.Models;
using Microsoft.AspNet.Identity;
using HemenTanitim.HtmlHelpers;

namespace HemenTanitim.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        HemenTanitimContext database_ = new HemenTanitimContext();
        #region Properties
        /// <summary>
        /// List of online users
        /// </summary>
        public readonly static List<Models.ViewModels.UserViewModel> _Connections = new List<Models.ViewModels.UserViewModel>();


        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        #endregion

        public void Send(string roomName, string message)
        {
            if (message.StartsWith("/private"))
                SendPrivate(message);
            else
                SendToUser(roomName, message);
        }

        public MessageViewModel SendPrivate(string message)
        {
            // message format: /private(receiverName) Lorem ipsum...
            string[] split = message.Split(',');

            MessageViewModel receiver = new MessageViewModel();

            if (split.Length > 1)
            {
                receiver.To = split[0];
                receiver.receiverId = split[1];
                message = Regex.Replace(split[2], @"(?i)<(?!img|a|/a|/img).*?>", string.Empty).Trim();
            }
            else
            {
                receiver = database_.Messages.Where(x => x.senderId == IdentityId)
                            .OrderByDescending(p => p.tarih)
                            .Select(r => new MessageViewModel { receiverId = r.receiverId, To = r.toUser})
                            .First();
                message = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty).Trim();
            }

            var sender = _Connections.Where(u => u.Username == IdentityName).First();
         

            using (var db = new HemenTanitimContext())
            {
                Message msg = new Message()
                {
                    fromUser = sender.Username,
                    profil_fotosu = sender.Fotograf,
                    toUser = receiver.To,
                    Content = message,
                    tarih = DateTime.Now.ToShortDateString(),
                    saat = DateTime.Now.ToShortTimeString(),
                    senderId = IdentityId,
                    receiverId = receiver.receiverId

                };

                db.Messages.Add(msg);
                db.SaveChanges();

                string receiverId;

                if (_ConnectionsMap.TryGetValue(receiver.To, out receiverId))
                {
                    Clients.Client(receiverId).newMessage(msg);
                }
                Clients.Caller.newMessage(msg);
                return Mapper.Map<Message, MessageViewModel>(msg);
            }
        }

        public IEnumerable<MessageViewModel> GetMessageHistory(string signaldan_gelen_receiverId)
        {
            using (var db = new HemenTanitimContext())
            {
                var messageHistory = new List<Message>();

                //var receiverOrSender = new MessageViewModel();
                string chatWithUserId = string.Empty;

                if (String.IsNullOrEmpty(signaldan_gelen_receiverId))
                {
                   
                    //New Code: 
                    chatWithUserId = db.Messages.Where(x => x.senderId == IdentityId || x.receiverId == IdentityId).OrderByDescending(x => x.Id)
                            .Select(x => x.receiverId == IdentityId ? x.senderId : x.receiverId).FirstOrDefault();



                    //New Code:
                    messageHistory = db.Messages
                        .Where(x =>
                                (
                                    x.senderId == IdentityId && x.receiverId == chatWithUserId
                                )
                                ||
                                (
                                    x.senderId == chatWithUserId && x.receiverId == IdentityId
                                )
                              )
                             .OrderByDescending(x => x.Id)
                             .Take(20)
                             .ToList();

                }
                else
                {
                   
                    //New Code:
                    messageHistory = db.Messages
                        .Where(x =>
                                 (
                                     x.senderId == IdentityId && x.receiverId == signaldan_gelen_receiverId
                                 )
                                 ||
                                 (
                                     x.senderId == signaldan_gelen_receiverId && x.receiverId == IdentityId
                                 )
                             )
                             .OrderByDescending(x => x.Id)
                             .Take(20)
                             .ToList();
                    //if there are any unread messages
                    //then update those message as read
                    if(messageHistory.Any(m=> m.ReadOn == null && m.receiverId == IdentityId))
                    {
                        messageHistory.Where(m => m.ReadOn == null && m.receiverId == IdentityId).ToList().ForEach(m => m.ReadOn = DateTime.Now);
                        db.SaveChanges();
                    }
                }
                messageHistory.Reverse();

                return Mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messageHistory);
            }
        }

        public IEnumerable<Models.ViewModels.UserViewModel> GetUserList()
        {
            using (var db = new HemenTanitimContext())
            {

                //İlgili Projeyi çeken kod SİLME İLERİDE LAZIM OLACAK!!!
                //List<Models.ViewModels.UserViewModel> userList = 
                //     db.Messages.Where(m => m.senderId == IdentityId || m.receiverId == IdentityId)
                //    .OrderByDescending(m => m.Id)
                //    .Join(db.IsIlanlaris, p => p.isIlanId, u2 => u2.isId, (p, u2) => new { p, u2 })
                //    .Select(x => new Models.ViewModels.UserViewModel
                //    {
                //        Username = x.p.senderId == IdentityId ? x.p.toUser : x.p.fromUser,
                //        UserId = x.p.senderId == IdentityId ? x.p.receiverId : x.p.senderId,
                //        ilgiliProjeAdi = x.u2.baslik

                //    })
                //    .Distinct()
                //    .ToList()
                //    .Join(db.Users, u => u.UserId, uir => uir.Id, (u, uir) => new { u, uir })                    
                //    .Select(x => new Models.ViewModels.UserViewModel
                //     {
                //         Username = x.uir.UserName,
                //         UserId = x.uir.Id,
                //         Fotograf = x.uir.Fotograf,
                //        ilgiliProjeAdi = x.u.ilgiliProjeAdi

                //     })
                //     .DistinctBy(c => c.UserId)
                //    .ToList();
                //İlgili Projeyi çeken kod SİLME İLERİDE LAZIM OLACAK!!!


                List<Models.ViewModels.UserViewModel> userList =
                db.Messages.Where(m => m.senderId == IdentityId || m.receiverId == IdentityId)
               .OrderByDescending(m => m.Id)
               .Select(x => new Models.ViewModels.UserViewModel
               {
                   Username = x.senderId == IdentityId ? x.toUser : x.fromUser,
                   UserId = x.senderId == IdentityId ? x.receiverId : x.senderId,
                   mesaj_icerik = x.Content
               })
               .DistinctBy(x => x.Username)
               .ToList()
               .Join(db.Users, u => u.UserId, uir => uir.Id, (u, uir) => new { u, uir })
               .Select(x => new Models.ViewModels.UserViewModel
               {
                   Username = x.uir.UserName,
                   UserId = x.uir.Id,
                   Fotograf = x.uir.Fotograf,
                   mesaj_icerik = x.u.mesaj_icerik

               })
                .DistinctBy(c => c.UserId)
               .ToList();



                var unreadMessagesCountByUser = db.Messages
                    .Where(m => m.ReadOn == null && m.receiverId == IdentityId)
                    .GroupBy(m => m.senderId).Select(x => new
                    Models.ViewModels.UserViewModel
                    {
                        UserId = x.Key,
                        UnreadMessageCount = x.Count()
                    }).ToList();
                //update user's online status
                for (int i = 0; i < userList.Count; i++)
                {
                    userList[i].UnreadMessageCount = unreadMessagesCountByUser.FirstOrDefault(u => u.UserId == userList[i].UserId)?.UnreadMessageCount;

                    if (_ConnectionsMap.Any(u => u.Key == userList[i].Username))
                        userList[i].IsOnline = true;
                }

                return userList;
            }
        }

        public Models.ViewModels.UserViewModel OnlineUser(string userAdi)
        {
            using (var db = new HemenTanitimContext())
            {
                //Models.ViewModels.UserViewModel user = db.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();

                var user = db.Users.Where(u => u.UserName == userAdi).FirstOrDefault();

                var userViewModel = Mapper.Map<ApplicationUser, Models.ViewModels.UserViewModel>(user);

                if (_ConnectionsMap.Any(u => u.Key == user.UserName))                  
                    userViewModel.IsOnline = true;
                else
                    userViewModel.IsOnline = false;

                return userViewModel;
            }
        }


        public void SendToUser(string toUserId, string message)
        {
            try
            {
                using (var db = new HemenTanitimContext())
                {

                    var toUser = db.Users.Where(r => r.Id == toUserId).FirstOrDefault();

                    // Create and save message in database
                    Message msg = new Message()
                    {
                        Content = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", String.Empty),
                        fromUser = IdentityName,
                        toUser = toUser.UserName,
                        senderId = IdentityId
                    };
                    db.Messages.Add(msg);
                    db.SaveChanges();

                    // Broadcast the message
                    var messageViewModel = Mapper.Map<Message, MessageViewModel>(msg);
                    Clients.Group(toUserId).newMessage(messageViewModel);
                }
            }
            catch (Exception)
            {
                Clients.Caller.onError("Message not send!");
            }
        }

        public void Join(string roomName)
        {
            try
            {
                //GetMessageHistory(roomName);

                var user = _Connections.Where(u => u.Username == IdentityName).FirstOrDefault();
                if (user.CurrentRoom != roomName)
                {
                    // Remove user from others list
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        Clients.OthersInGroup(user.CurrentRoom).removeUser(user);

                    // Join to new chat room
                    Leave(user.CurrentRoom);
                    Groups.Add(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;

                    // Tell others to update their list of users
                    Clients.OthersInGroup(roomName).addUser(user);
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.onError("You failed to join the chat room!" + ex.Message);
            }
        }

        private void Leave(string roomName)
        {
            Groups.Remove(Context.ConnectionId, roomName);
        }



        public IEnumerable<Models.ViewModels.UserViewModel> GetUsers(string fromUser)
        {
            return _Connections.Where(u => u.Username == IdentityName).ToList();
        }

        #region OnConnected/OnDisconnected
        public override Task OnConnected()
        {
            using (var db = new HemenTanitimContext())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();

                    var userViewModel = Mapper.Map<ApplicationUser, Models.ViewModels.UserViewModel>(user);
                    userViewModel.UserId = user.Id;
                    userViewModel.Device = GetDevice();
                    userViewModel.CurrentRoom = "";
                    userViewModel.IsOnline = true;

                    _Connections.Add(userViewModel);
                    if (!_ConnectionsMap.ContainsKey(IdentityName))
                        _ConnectionsMap.Add(IdentityName, Context.ConnectionId);

                    Clients.Caller.getProfileInfo(user.UserName, user.Avatar, user.Id);
                    Clients.All.onUserConnected(userViewModel);
                }
                catch (Exception ex)
                {
                    Clients.Caller.onError("OnConnected:" + ex.Message);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                var user = _Connections.Where(u => u.Username == IdentityName).First();
                _Connections.Remove(user);


                database_.Users.Where(m => m.UserName == IdentityName).FirstOrDefault().SonGorulmeTarihi = DateTime.Now;
                database_.SaveChanges();


                // Tell other users to remove you from their list
                if (!string.IsNullOrWhiteSpace(user.CurrentRoom))
                    Clients.OthersInGroup(user.CurrentRoom).removeUser(user);

                // Remove mapping
                _ConnectionsMap.Remove(user.Username);
                user.IsOnline = false;
                Clients.All.onUserDisConnected(user);
            }
            catch (Exception ex)
            {
                Clients.Caller.onError("OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            //var user = _Connections.Where(u => u.Username == IdentityName).First();
            //Clients.Caller.getProfileInfo(user.DisplayName, user.Avatar);

            return base.OnReconnected();
        }
        #endregion

       
        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }

        private string IdentityId
        {
            get { return Context.User.Identity.GetUserId(); }
        }

        private string GetDevice()
        {
            string device = Context.Headers.Get("Device");

            if (device != null && (device.Equals("Desktop") || device.Equals("Mobile")))
                return device;

            return "Web";
        }

    }
}