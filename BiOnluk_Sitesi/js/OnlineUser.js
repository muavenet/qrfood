var $_online = jQuery.noConflict();


$_online(function () {

	var chatHub = $_online.connection.chatHub;

	$_online.connection.hub.start().done(function () {

		console.log("start");
		model.getUserList();
	});


	chatHub.client.onError = function (message) {
		model.serverInfoMessage(message);
		$_online("#errorAlert").removeClass("hidden").show().delay(5000).fadeOut(500);
	};

	var Model = function () {
		var self = this;
		self.durum = ko.observable();
	};



	Model.prototype = {
		getUserList: function () {

			console.log("getUserList");

			self = this;

			var kId = $_online('#kId').val();

			chatHub.server.onlineUser(kId).done(function (result) {

				self.durum(result.IsOnline);		

			});

		},
	};

	var model = new Model();
	ko.applyBindings(model);

});