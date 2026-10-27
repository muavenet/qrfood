var $sepetConf = jQuery.noConflict();


$sepetConf(function () {

            // Declare a proxy to reference the hub.
    var notifications = $sepetConf.connection.sepetHub;

            //debugger;
            // Create a function that the hub can call to broadcast messages.
            notifications.client.displayStatus = function () {
                getAllMessages()

            };
            // Start the connection.
    $sepetConf.connection.hub.start().done(function () {
                //alert("connection started")
                getAllMessages();
            }).fail(function (e) {
                alert(e);
            });
        });


        function getAllMessages() {
            var tbl = $('#messagesTable');
            $.ajax({
                url: '/home/GetMessages',
                contentType: 'application/html ; charset:utf-8',
                type: 'GET',
                dataType: 'html'
            }).success(function (result) {
                tbl.empty().append(result);
            }).error(function () {

            });
        }
