(function (OrdersApp) {

    "use strict";

    var serviceBase = window.location.href + '/', getSvcUrl = function (method) { return serviceBase + method; };

    OrdersApp.ajaxService = (function () {
      
        var ajaxGetJson = function (method, jsonIn, callback) {
            $.ajax({
                url: getSvcUrl(method),
                type: "GET",
                data: ko.toJSON(jsonIn),
                dataType: "json",
                contentType: "application/json",
                success: function (json) {
                    callback(json);
                }
            });
        },
            ajaxPostJson = function (method, jsonIn, callback) {
				
                $.ajax({
                    url: getSvcUrl(method),
                    type: "POST",
                    data: ko.toJSON(jsonIn),
                    dataType: "json",
                    async : true,
                    contentType: "application/json",
					success: function (json) {
					},

                });
			},

			ajaxPostJsonHref = function (method, jsonIn) {

				$("#spin_i").css("display", "inline-block");
				
				$.ajax({
					url: getSvcUrl(method),
					type: "POST",
					data: ko.toJSON(jsonIn),
					dataType: "json",
					async: true,
					contentType: "application/json",
					success: function (response) {

						$('body').append(response.CheckoutFormContent);


						$("#spin_i").css("display", "none");
						$("#btnTamamla").css("display", "none");
						$("#btnTamamla_Cash").css("display", "none");
						$(".ustAlan_sepet").css("display", "none");
						$(".sakla_totals").css("display", "none");

						$("#iyzipay-checkout-form")
							.css("position", "fixed")
							.css("width", "100%")
							.css("top", "0px")
							.css("left", "0px")
							.css("bottom", "0px")
							.css("margin", "0px")
							.css("min-height", "200px")
							.css("height", "100%")
							.css("z-index", "999")
							.css("background-color", "#fff");

					}
				});
			},


		ajaxPostJsonHrefURL = function (method, jsonIn) {

			$("#spin_i").css("display", "inline-block");
	
			$.ajax({
				url: method,
				type: "POST",
				data: ko.toJSON(jsonIn),
				dataType: "json",
				async: true,
				contentType: "application/json",
				success: function (response) {
						location.href = response;
				}
			});
		};

        return {
            ajaxGetJson: ajaxGetJson,
			ajaxPostJson: ajaxPostJson,
			ajaxPostJsonHref: ajaxPostJsonHref,
			ajaxPostJsonHrefURL: ajaxPostJsonHrefURL
        };
    })();
}(OrdersApp));