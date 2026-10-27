$(function () {


    $('#emojis-container').on('click', 'a', function () {
        var value = $("input", $(this)).val();
        var input = $('#chat-message');

        input.val(input.val() + value);
        input.focus();
    });

    // Show/Hide Emoji Window
    $("#emojibtn").click(function () {
        var x = $("#emojis-container");
        if (x.hasClass("hidden")) {
            x.removeClass("hidden");
        }
        else {
            x.addClass("hidden");
        }
    });

    $("#chat-message").click(function () {
        $("#emojis-container").addClass("hidden")
    });

    // Upload image with Ajax
    $("#btnUpload").change(function () {
        $("#chatOperations").show();
        $("#chatOperations").text("Dosya yükleniyor. Lütfen bekleyiniz...");
        var data = new FormData();
        var file = document.getElementById("btnUpload").files[0];
        var receiverId = $("#chat-message-hidden").val().split(',')[1];
        data.append("btnUpload", file);
        data.append("receiver_userName", $("#chat-message-hidden").val().split(',')[0]);
        data.append("receiverId", receiverId);

        $.ajax({
            type: "POST",
            url: '/Home/Upload',
            data: data,
            dataType: 'json',
            contentType: false,
            processData: false,
			success: function (response) {
				if (response != "1")
					alert(response);
                $("#chatOperations").hide();
            },
            error: function (error) {
                alert(error);
            }
        });

        //console.log(files);

    });
});