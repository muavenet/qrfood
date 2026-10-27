$(function () {

	var chatHub = $.connection.chatHub;

    $.connection.hub.start().done(function () {
        console.log("connection started");
        //model.roomList();		
        model.getUserList();

        //model.joinedRoom = "Lobby";		
        //model.joinRoom();
    });

    // Client Operations
    chatHub.client.newMessage = function (messageView) {
     
        //if the message is for current user and he is not chatting with the sender then increase the unread message count.
        if ((messageView.receiverId == model.currentUserId()) && model.chatWithUserId != messageView.senderId)
            model.addUnreadMessage(messageView.senderId);

        //check if the message is for the right user.
        //either he is a sender or receiver and chatting with the same user for which message came.
        if (!((messageView.receiverId == model.currentUserId() || messageView.senderId == model.currentUserId())
            && (model.chatWithUserId == messageView.receiverId || model.chatWithUserId == messageView.senderId))) {
            return;
        }

        var isMine = messageView.fromUser === model.myName();

        var message = new ChatMessage(messageView.Content,
			messageView.saat,
			
            messageView.fromUser,
            isMine,
			//messageView.toUser,
			messageView.profil_fotosu
		);

		model.chatMessages.push(message);

        if (messageView.receiverId == model.currentUserId())
            model.markAsReadInDB(messageView.receiverId, messageView.senderId, messageView.Id);


        $(".chat-body").animate({ scrollTop: $(".chat-body")[0].scrollHeight }, 1);

    };

    chatHub.client.getProfileInfo = function (displayName, profil_fotosu, userId) {

        model.myName(displayName);
        model.currentUserId(userId);
        model.myprofil_fotosu(profil_fotosu);
	};

    $('ul#room-list').on('click', 'a', function () {
        var roomName = $(this).text();
        model.joinedRoom = roomName;
        model.joinRoom();
        model.chatMessages.removeAll();
        $("input#iRoom").val(roomName);
        $("#joinedRoom").text(roomName);
        $('#room-list a').removeClass('active');
        $(this).addClass('active');
    });
 
    chatHub.client.onError = function (message) {
        model.serverInfoMessage(message);

        $("#errorAlert").removeClass("hidden").show().delay(5000).fadeOut(500);
    };
    chatHub.client.onUserConnected = function (user) {
        console.log("on user connected:" + user);
        if (model.chatUserList_().length > 0) {
            var userIndex = model.chatUserList_().findIndex((obj => obj.Username == user.Username));
            if (userIndex < 0) return;
            let unreadMessageCount = model.chatUserList_()[userIndex].UnreadMessageCount;
            user.UnreadMessageCount = unreadMessageCount;
            model.chatUserList_().splice(userIndex, 1);
            model.chatUserList_.unshift(user);
            model.filterUser($("#txtSearchUser").val());
        }

    };
    chatHub.client.onUserDisConnected = function (user) {
        console.log("on user disconnected:" + user);
        if (model.chatUserList_().length > 0) {
            var userIndex = model.chatUserList_().findIndex((obj => obj.Username == user.Username));
            if (userIndex < 0) return;
            let unreadMessageCount = model.chatUserList_()[userIndex].UnreadMessageCount;
            user.UnreadMessageCount = unreadMessageCount;
            model.chatUserList_().splice(userIndex, 1);
            model.chatUserList_.push(user);
            model.filterUser($("#txtSearchUser").val());
        }
    };
    chatHub.client.otherUserIsTyping = function (userName) {
        if (model.chatUserList_().length > 0) {
            model.otherUserIsTyping(userName);
        }

    };

    var Model = function () {
        var self = this;
        self.message = ko.observable("");
        self.message_icinde_gizle_bilgileri = ko.observable("");
		self.chatRooms = ko.observableArray([]);
		self.mesajBaslik = ko.observable("");
        //filtered user list
        self.chatUsers = ko.observableArray([]);
        self.chatMessages = ko.observableArray([]);
        //its a master user list
        self.chatUserList_ = ko.observableArray([]);
        self.joinedRoom = ko.observable("");
        self.serverInfoMessage = ko.observable("");
        self.myName = ko.observable("");
        self.currentUserId = ko.observable("");
        self.chatWithUserId = ko.observable("");
        self.chatWithUserName = ko.observable("");
        self.isChatWithUserOnline = ko.observable("");
        self.myprofil_fotosu = ko.observable("");
        self.searchUserTimer = null;
        self.onEnter = function (d, e) {
            if (e.keyCode === 13) {
				var userId = $('#chat-message-hidden').val();				
                self.message_icinde_gizle_bilgileri = userId + "" + self.message();
                self.sendNewMessage();

            } else if ((e.keyCode >= 32 && e.keyCode <= 127) || (e.keyCode === 8)) {
                this.currentUserIsTyping();
            }
            return true;
        };
        self.searchUser = function (d, e) {
            if ((e.keyCode >= 32 && e.keyCode <= 127) || (e.keyCode === 8 || e.keyCode === 13)) {
                //code for to get users from database after some interval.
                //self.currentUserTypingTimer = setTimeout(function () {
                //    //api call to get users
                //    self.searchUserTimer = null;
                //}, 3000);
                //filter user from master list
                self.filterUser($("#txtSearchUser").val());
            }

        };
        self.filterUser = function (lookupString) {
            self.chatUsers.removeAll();
            if (!lookupString) {
                self.chatUsers.push(...self.chatUserList_());
                return;
            }
            self.chatUserList_().forEach(function (item, index) {
                if (item.Username.toLowerCase().startsWith(lookupString.toLowerCase())) {
                    self.chatUsers.push(item);
                }
            });
        };
        self.markAsRead = function (userId) {

            if (!userId) return;

            var uIndex = self.chatUserList_().findIndex(u => u.UserId == userId);
            if (uIndex < 0) return;
            self.chatUserList_()[uIndex].UnreadMessageCount = 0;
            self.filterUser($("#txtSearchUser").val());
        };
        self.markAsReadInDB = function (recieverUserId, senderUserId, messageId) {
            if (!recieverUserId || !messageId) return;

            self.markAsRead(senderUserId);
            $.ajax({
                type: "POST",
                url: '/Home/MarkMessageAsRead?userId=' + recieverUserId + '&messageId=' + messageId,
                //data: { userName: self.myName() },
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {

                },
                error: function (error) {
                    //console.log("error" + error);
                }
            });

        };
        self.addUnreadMessage = function (userId) {
            if (!userId) return;

            var uIndex = self.chatUserList_().findIndex(u => u.UserId == userId);
            if (uIndex < 0) return;
            self.chatUserList_()[uIndex].UnreadMessageCount = self.chatUserList_()[uIndex].UnreadMessageCount ? self.chatUserList_()[uIndex].UnreadMessageCount + 1 : 1;
            self.filterUser($("#txtSearchUser").val());
        };
        self.currentUserTypingTimer = null;
        self.otherUserTypingTimer = null;
        self.currentUserIsTyping = function () {
            if (self.currentUserTypingTimer == null && self.isChatWithUserOnline() && (self.isChatWithUserOnline() == "true" || self.isChatWithUserOnline() == true)) {
                $.ajax({
                    type: "POST",
                    url: '/Home/UserIsTyping?userName=' + self.myName(),
                    //data: { userName: self.myName() },
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {

                    },
                    error: function (error) {
                        //console.log("error" + error);
                    }
                });

                self.currentUserTypingTimer = setTimeout(function () {
                    self.currentUserTypingTimer = null;
                }, 2000);

            }

        };
        self.otherUserIsTyping = function (userName) {

            if (self.chatWithUserName() == userName) {
                $("#chatOperations").text(userName + " is typing...");
                $("#chatOperations").show();

                clearTimeout(self.otherUserTypingTimer);

                self.otherUserTypingTimer = setTimeout(function () {
                    $("#chatOperations").hide();
                }, 6000);
            }
		};
        self.startChatWithUser = function (userId, userName, isOnline) {
            self.chatWithUserId = userId;
            self.isChatWithUserOnline(isOnline);
            model.chatWithUserName(userName);
            model.messageHistory(self.chatWithUserId);
            self.markAsRead(userId);

            var input = $('#chat-message'), input2 = $('#chat-message-hidden');
            var text = input.val();
            var text2 = userName + "," + userId + "," + text;
			
            self.message_icinde_gizle_bilgileri = userId;

            //if (text.startsWith("/")) {
            //	text = text.split(")")[1];
            //}
            //text = text;
            input.val(text);
            input2.val(text2);
            input.focus();
        };
        self.filter = ko.observable("");
        //self.chatUserList_ = ko.computed(function () {			
        //          if (!self.filter()) {
        //		return self.chatUsers();
        //          } else {
        //		return ko.utils.arrayFilter(self.chatUsers(), function (user) {
        //			console.log(user);
        //			var displayName = user.toUser().toLowerCase();
        //			console.log(displayName);
        //                  return displayName.includes(self.filter().toLowerCase());
        //              });
        //          }
        //      });

    };

    $('ul#user-list').on('click', 'li', function () {
        var username = $("input[type=hidden].username", $(this)).val();
        var userId = $("input[type=hidden].userId", $(this)).val();
        var IsOnline = $("input[type=hidden].IsOnline", $(this)).val();
        model.startChatWithUser(userId, username, IsOnline);
    });

    Model.prototype = {

        // Server Operations
        sendNewMessage: function () {
            var self = this;
            chatHub.server.sendPrivate(self.message_icinde_gizle_bilgileri);
            self.message("");
        },

        joinRoom: function () {
            var self = this;
            chatHub.server.join(self.joinedRoom).done(function () {
                //self.userList();
                self.messageHistory();
                self.getUserList();
            });
        },

        roomList: function () {
            var self = this;
            chatHub.server.getRooms().done(function (result) {
                self.chatRooms.removeAll();
                for (var i = 0; i < result.length; i++) {
                    self.chatRooms.push(new ChatRoom(result[i].Id, result[i].Name));
                }
            });
        },

        userList: function () {
            var self = this;
            chatHub.server.getUsers(self.joinedRoom).done(function (result) {
                self.chatUsers.removeAll();
                for (var i = 0; i < result.length; i++) {
                    self.chatUsers.push(new ChatUser(result[i].Username,
                        result[i].DisplayName,
                        result[i].profil_fotosu,
                        result[i].CurrentRoom,
                        result[i].Device))
                }
            });

        },

        createRoom: function () {
            var name = $("#roomName").val();
            chatHub.server.createRoom(name);
        },

        deleteRoom: function () {
            var self = this;
            chatHub.server.deleteRoom(self.joinedRoom);
        },

        messageHistory: function (toUser) {
            var self = this;
            self.chatWithUserId = toUser;
            chatHub.server.getMessageHistory(self.chatWithUserId).done(function (result) {

                self.chatMessages.removeAll();
                console.log(result);
                for (var i = 0; i < result.length; i++) {
                    var isMine = result[i].From == self.myName();
                    self.chatMessages.push(new ChatMessage(result[i].Content,
                        result[i].saat,
                        result[i].From,
                        isMine,
						result[i].profil_fotosu))
				
                }

				$(".chat-body").animate({ scrollTop: $(".chat-body")[0].scrollHeight }, 1);

            });
        },

        getUserList: function () {
            var self = this;
            var IsFirst = true;
            chatHub.server.getUserList().done(function (result) {
                self.chatUserList_.removeAll();
				self.chatUsers.removeAll();

				for (var i = 0; i < result.length; i++) {
                    //New Code
                    if (result[i].UserId && result[i].UserId != "undefined") {
                        //setting first user's id and name in hidden control
                        if (IsFirst == true) {
                         
                            IsFirst = false;
                            self.chatUserList_.push(result[i]);
                            self.chatUsers.push(result[i]);
							self.startChatWithUser(result[i].UserId, result[i].Username, result[i].IsOnline, result[i].Fotograf, result[i].mesaj_icerik);
                        }
                        else {
                            self.chatUserList_.push(result[i]);
                            self.chatUsers.push(result[i]);
                        }
                    }

					//$(".messages").animate({ scrollTop: $(".messages")[0].scrollHeight }, 1000);

                    //chatHub.server.getUsers(self.joinedRoom).done(function (result) {
                    //	self.chatUsers.removeAll();
                    //	for (var i = 0; i < result.length; i++) {
                    //		self.chatUsers.push(new ChatUser(result[i].Username,
                    //			result[i].DisplayName,
                    //			result[i].profil_fotosu,
                    //			result[i].CurrentRoom,
                    //			result[i].Device))
                    //	}
                    //});
                }
            });

            //model.messageHistory();
        },

        roomAdded: function (room) {
            var self = this;
            self.chatRooms.push(room);
        },

        roomDeleted: function (id) {
            var self = this;
            var temp;
            ko.utils.arrayForEach(self.chatRooms(), function (room) {
                if (room.roomId() == id)
                    temp = room;
            });
            self.chatRooms.remove(temp);
        },

        userAdded: function (user) {
            var self = this;
            self.chatUsers.push(user)
        },

        userRemoved: function (id) {
            var self = this;
            var temp;
            ko.utils.arrayForEach(self.chatUsers(), function (user) {
                if (user.userName() == id)
                    temp = user;
            });
            self.chatUsers.remove(temp);
        },
    };

    // Represent server data
    function ChatRoom(roomId, name) {
        var self = this;
        self.roomId = ko.observable(roomId);
        self.name = ko.observable(name);
    }

    function ChatUser(userName, displayName, profil_fotosu, currentRoom, device) {
        var self = this;
        self.userName = ko.observable(userName);
        self.displayName = ko.observable(displayName);
        self.profil_fotosu = ko.observable(profil_fotosu);
        self.currentRoom = ko.observable(currentRoom);
        self.device = ko.observable(device);
    }

    function ChatMessage(content, saat, from, isMine, profil_fotosu) {
        var self = this;
        self.content = ko.observable(content);
        self.saat = ko.observable(saat);
        self.from = ko.observable(from);
        self.isMine = ko.observable(isMine);
        self.profil_fotosu = ko.observable(profil_fotosu);
    }

    function ChatUsersList(toUser, UsersId) {
        var self = this;
        self.toUser = ko.observable(toUser);
        self.UsersId_ = ko.observable(UsersId);
	}

    function convertToObservable(list) {
        var newList = [];
        $.each(list, function (i, obj) {
            var newObj = {};
            Object.keys(obj).forEach(function (key) {
                newObj[key] = ko.observable(obj[key]);
            });
            newList.push(newObj);
        });
        return newList;
    }

	var model = new Model();

    ko.applyBindings(model);

});