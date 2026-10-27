var MYLIBRARY = MYLIBRARY || (function () {
        var _args = {}; // private

        return {
            init: function (Args) {
                _args = Args;
                // some other initialising
            },

            yorumfonksiyonu: function () {

                    var url_ = "/Home/YorumGonder?adsoyad=" + _args[0] + "&Aciklama=" + _args[1] + "&UrunId=" + _args[2];

                    $.ajax({
                        url: url_,
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data == true) {
                                $("#badgeYorumOnay").text("Lütfen bekleyiniz...");
                                setTimeout(function () {
                                    $("#badgeYorumOnay").text("Yorumunuz onaya gönderildi.");
                                    $("#exampleInputReview").val("");
                                    $("#exampleInputName").val("");
                                }, 1000);
                            }
                            else {

                            }
                        },
                        error: function (jqXHR, textStatus, error) {

                            alert(error);
                        },
                    });               
            },

            sepeteAtFonksiyonu: function ()
			{

				var param = JSON.stringify({
					'ilanAdi': _args[0], 'ilan_Id': _args[1], 'spFiyat': _args[2], 'spAciklama': _args[3],
					'teslimatSuresi': _args[4], 'ilanSahibiId': _args[5], 'ilanGereksinimleri': _args[6], 'spilanResim': _args[7],
                    'spilanKullaniciFoto': _args[8], 'hdAdet': _args[9]
				});


                    $sepetUrunDetay.ajax({
						url: "/UrunDetay/SepeteAt/",
						data: param,
						type: "POST",
						contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data == true) {

                                $sepetUrunDetay("#thickOlacak").removeClass().addClass("feather-check-square");
                                var text = parseInt($sepetUrunDetay("#spanCountId").text());
                                $sepetUrunDetay("#spanCountId").text(parseInt(text + 1));
                            }
                            else {
                                window.location = '/Account/Login';
                            }
                        },
                        error: function (jqXHR, textStatus, error) {

                            alert(error);
                        },
                    });               
            },

            ziyaretciKaydetFonskiyonu: function ()
            {
              
                //$.cookie("site_ziyaretcici", null, { expires: -1 }) siler;
                //if ($.cookie(window.location.href) == null) {

                //    $.cookie(window.location.href, window.location.href) // cookie'ye değer atamak

                //    $.cookie(window.location.href, window.location.href, { expires: 999 }); // cookie'ye belli bir süre (gün olarak) için değer atama

                var $ZiyaretciKaydet = jQuery.noConflict();

                var url_ = "/UrunDetay/ZiyaretciKaydet/" + _args[1];

                $.ajax({
                    url: url_,
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (data) {
                        //alert("tamam");
                    },
                    error: function () {
                        //alert("hata");
                    }
                });
                //}
                //else {

                //    //console.log("var");
                //}
            },

            carouselFonksiyonu: function () {

                var $car = jQuery.noConflict();

                $car('#myCarousel').carousel({
                    interval: false
                });

                $car('#carousel-text').html($car('#slide-content-0').html());

                //Handles the carousel thumbnails
                $car('[id^=carousel-selector-]').click(function () {
                    var id_selector = $car(this).attr("id");
                    var id = id_selector.substr(id_selector.lastIndexOf("-") + 1);
                    var id = parseInt(id);
                    $car('#myCarousel').carousel(id);
                });


                // When the carousel slides, auto update the text
                //$('#myCarousel').on('slid', function (e) {
                //    var id = $('.item.active').data('slide-number');
                //    $('#carousel-text').html($('#slide-content-' + id).html());
                //});

			}

	

        };

    }());
