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

namespace HemenTanitim.Hubs
{
    [Authorize]
    public class ChatHub_yedek : Hub
    {
        HemenTanitimContext database_ = new HemenTanitimContext();
        #region Properties
        /// <summary>
        /// List of online users
        /// </summary>
        public readonly static List<Models.ViewModels.UserViewModel> _Connections = new List<Models.ViewModels.UserViewModel>();

        /// <summary>
        /// List of available chat rooms
        /// </summary>
        private readonly static List<RoomViewModel> _Rooms = new List<RoomViewModel>();

        /// <summary>
        /// Mapping SignalR connections to application users.
        /// (We don't want to share connectionId)
        /// </summary>


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
                            .Select(r => new MessageViewModel { receiverId = r.receiverId, To = r.toUser })
                            .First();
                message = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty).Trim();
            }

            var sender = _Connections.Where(u => u.Username == IdentityName).First();

            using (var db = new HemenTanitimContext())
            {
                Message msg = new Message()
                {
                    fromUser = sender.Username,
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
                    //old code
                    //receiverOrSender = db.Messages.Where(x => x.senderId == IdentityId || x.receiverId == IdentityId).OrderByDescending(x => x.tarih)                          
                    //        .Select(x => new MessageViewModel { receiverId = x.receiverId == IdentityId ? x.senderId : x.receiverId }).First();

                    //New Code: 
                    chatWithUserId = db.Messages.Where(x => x.senderId == IdentityId || x.receiverId == IdentityId).OrderByDescending(x => x.Id)
                            .Select(x => x.receiverId == IdentityId ? x.senderId : x.receiverId).FirstOrDefault();


                    //Old Code
                    //messageHistory = db.Messages.Where(x => x.senderId == IdentityId && x.receiverId == receiverOrSender.receiverId || x.receiverId == IdentityId && x.senderId == receiverOrSender.receiverId)
                    //         .OrderByDescending(x => x.tarih)
                    //         .Take(20)
                    //        .AsEnumerable()
                    //          .Reverse()
                    //        .ToList();

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
                    //Old Code
                    //messageHistory = db.Messages.Where(x => x.senderId == IdentityId && x.receiverId == signaldan_gelen_receiverId || x.receiverId == IdentityId && x.senderId == signaldan_gelen_receiverId)
                    //   .OrderByDescending(x => x.tarih)
                    //   .Take(20)
                    //                   .AsEnumerable()
                    //                    .Reverse()
                    //                   .ToList();

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
                }
                messageHistory.Reverse();

                return Mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messageHistory);
            }
        }

        public IEnumerable<MessageViewModel> GetUserList()
        {

            using (var db = new HemenTanitimContext())
            {
                var messageHistory = db.Messages.Where(m => m.senderId == IdentityId || m.receiverId == IdentityId)
                    //.OrderByDescending(m => m.tarih)
                    .OrderByDescending(m => m.Id) //Replaced with id
                   //.GroupBy(x => x.senderId == IdentityId ? x.fromUser : x.toUser) 
                    .Select(x => new MessageViewModel
                    {
                        To = x.senderId == IdentityId ? x.toUser : x.fromUser,
                        UsersId = x.senderId == IdentityId ? x.receiverId : x.senderId

                    }).Distinct().ToList();


                return messageHistory;
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

        public void CreateRoom(string roomName)
        {
            try
            {
                using (var db = new HemenTanitimContext())
                {
                    // Accept: Letters, numbers and one space between words.
                    Match match = Regex.Match(roomName, @"^\w+( \w+)*$");
                    if (!match.Success)
                    {
                        Clients.Caller.onError("Invalid room name!\nRoom name must contain only letters and numbers.");
                    }
                    else if (roomName.Length < 5 || roomName.Length > 20)
                    {
                        Clients.Caller.onError("Room name must be between 5-20 characters!");
                    }
                    else if (db.Rooms.Any(r => r.Name == roomName))
                    {
                        Clients.Caller.onError("Another chat room with this name exists");
                    }
                    else
                    {
                        // Create and save chat room in database
                        var user = db.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();
                        var room = new Room()
                        {
                            Name = roomName,
                            UserAccount = user
                        };
                        db.Rooms.Add(room);
                        db.SaveChanges();

                        if (room != null)
                        {
                            // Update room list
                            var roomViewModel = Mapper.Map<Room, RoomViewModel>(room);
                            _Rooms.Add(roomViewModel);
                            Clients.All.addChatRoom(roomViewModel);
                        }
                    }
                }//using
            }
            catch (Exception ex)
            {
                Clients.Caller.onError("Couldn't create chat room: " + ex.Message);
            }
        }

        public void DeleteRoom(string roomName)
        {
            try
            {
                using (var db = new HemenTanitimContext())
                {
                    // Delete from database
                    var room = db.Rooms.Where(r => r.Name == roomName && r.UserAccount.UserName == IdentityName).FirstOrDefault();
                    db.Rooms.Remove(room);
                    db.SaveChanges();

                    // Delete from list
                    var roomViewModel = _Rooms.First<RoomViewModel>(r => r.Name == roomName);
                    _Rooms.Remove(roomViewModel);

                    // Move users back to Lobby
                    Clients.Group(roomName).onRoomDeleted(string.Format("Room {0} has been deleted.\nYou are now moved to the Lobby!", roomName));

                    // Tell all users to update their room list
                    Clients.All.removeChatRoom(roomViewModel);
                }
            }
            catch (Exception)
            {
                Clients.Caller.onError("Can't delete this chat room.");
            }
        }

        public IEnumerable<RoomViewModel> GetRooms()
        {
            using (var db = new HemenTanitimContext())
            {
                // First run?
                if (_Rooms.Count == 0)
                {
                    foreach (var room in db.Rooms)
                    {
                        var roomViewModel = Mapper.Map<Room, RoomViewModel>(room);
                        _Rooms.Add(roomViewModel);
                    }
                }
            }

            return _Rooms.ToList();
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
                    userViewModel.Device = GetDevice();
                    userViewModel.CurrentRoom = "";

                    _Connections.Add(userViewModel);
                    _ConnectionsMap.Add(IdentityName, Context.ConnectionId);

                    Clients.Caller.getProfileInfo(user.UserName, user.Avatar);
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

                // Tell other users to remove you from their list
                Clients.OthersInGroup(user.CurrentRoom).removeUser(user);

                // Remove mapping
                _ConnectionsMap.Remove(user.Username);
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