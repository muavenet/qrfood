var $onlineUser = jQuery.noConflict();

$onlineUser(function () {

	var chatHub = $onlineUser.connection.chatHub;

	$onlineUser.connection.hub.start().done(function () {
		console.log("connection started");
	});

	//var Model = function () {
	//	//var self = this;
	//	//self.message = ko.observable("");
	//};

	chatHub.client.onError = function (message) {

		model.serverInfoMessage(message);
		console.log(message);

		$onlineUser("#errorAlert").removeClass("hidden").show().delay(5000).fadeOut(500);
	};

	chatHub.client.onUserConnected = function (user) {
		console.log("on user connected:" + user);	
	};

	chatHub.client.onUserDisConnected = function (user) {
		console.log("on user disconnected:" + user);		
	};

	//var model = new Model();
	//ko.applyBindings(model);
});
