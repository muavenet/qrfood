var MYLIBRARY = MYLIBRARY || (function () {

    //var serviceBase = window.location.href + '/', getSvcUrl = function (method) { return serviceBase + method; };

    var _args = {}; // private

    return {
        init: function (Args) {
            _args = Args;
            // some other initialising
        },

        ajaxGetJson: function (url_, data) {

            $.ajax({
                url: url_,
                type: "get",
                data: data,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data == true) {

                    }
                    else {

                    }
                },
                error: function (jqXHR, textStatus, error) {
                    alert(error);
                },
            });
        },

        ajaxPostJson: function (url_) {

            $.ajax({
                url: url_,
                type: "POST",
                data: data,
                cache: false,
                async: false,
                dataType: "json",
                success: function () {

                        $('body').prepend('<div class="alert alert-success alert-fixed" style="position:fixed; width:100%; top:0; z-index:99999; border-radius:0;">✅&nbsp;&nbsp;&nbsp;İŞLEM BAŞARIYLA GERÇEKLEŞTİRİLDİ</div>');

                        $(".alert").delay(3500).slideUp(200, function () {
                            $(this).alert('close');
                        });

                    },
                error: function (jqXHR, textStatus, error) {
                    alert(error);
                },

            });
		},

        VitrinUrunleriGetir: function () {

            var $filtreleme = jQuery.noConflict();

            $filtreleme("#drop_value_Selected").text(_args[4]);

            $filtreleme("#drop_value_Selected").val(_args[0]);

            $filtreleme("#loadgifLoadMore").append("<img src='/Content/FrontEnd/images/load.GIF'/>").fadeIn(200);

            $filtreleme.ajax({

                url: '/FrontEndModules/_VitrinUrunler',
                data: { sortorder: _args[0], AnaKategoriId: _args[1], AnaKategoriAdi: _args[2], AltKategoriId: _args[3], size: 12, ilId: _args[5] },
                datatype: 'json',
                success: function (data) {

                    $filtreleme("#urunler_ust").html(data);

                    $filtreleme("#loadgifLoadMore").html("");

                    $filtreleme("#hdnanaKategoriId").val(_args[1]);
                    $filtreleme("#hdnanaAltKategoriId").val(_args[3]);
                    $filtreleme("#hdnanaKategoriAdi").val(_args[2]);
                    $filtreleme("#hdnIlId").val(_args[5]);


                },
                error: function () {
                    alert("error");
                }
            });
        },

		VitrinUrunleriGetir_IllereGore: function () {

			var $filtreleme = jQuery.noConflict();

            $filtreleme("#drop_value_Selected_il").text(_args[4]);

			$filtreleme("#loadgifLoadMore").append("<img src='/Content/FrontEnd/images/load.GIF'/>").fadeIn(200);

			$filtreleme.ajax({

                url: '/FrontEndModules/_VitrinUrunler',
                data: { sortorder: _args[0], AnaKategoriId: _args[1], AnaKategoriAdi: _args[2], AltKategoriId: _args[3], size: 12, ilId : _args[5] },
				datatype: 'json',
				success: function (data) {

					$filtreleme("#urunler_ust").html(data);

                    $filtreleme("#loadgifLoadMore").html("");

                    $filtreleme("#hdnanaKategoriId").val(_args[1]);
                    $filtreleme("#hdnanaAltKategoriId").val(_args[3]);
                    $filtreleme("#hdnanaKategoriAdi").val(_args[2]);
                    $filtreleme("#hdnIlId").val(_args[5]);

                    $filtreleme("#myInput").val("");

				},
				error: function () {
					alert("error");
				}
			});
		},

        VitrinUrunleriGetir_Siralama: function () {

            var $filtreleme = jQuery.noConflict();

            $filtreleme("#drop_value_Selected_siralama").text(_args[4]);

            $filtreleme("#loadgifLoadMore").append("<img src='/Content/FrontEnd/images/load.GIF'/>").fadeIn(200);

            $filtreleme.ajax({

                url: '/FrontEndModules/_VitrinUrunler',
                data: { sortorder: _args[0], AnaKategoriId: _args[1], AnaKategoriAdi: _args[2], AltKategoriId: _args[3], size: 12, ilId: _args[5] },
                datatype: 'json',
                success: function (data) {

                    $filtreleme("#urunler_ust").html(data);

                    $filtreleme("#loadgifLoadMore").html("");

                    $filtreleme("#hdnanaKategoriId").val(_args[1]);
                    $filtreleme("#hdnanaAltKategoriId").val(_args[3]);
                    $filtreleme("#hdnanaKategoriAdi").val(_args[2]);
                    $filtreleme("#hdnIlId").val(_args[5]);

                    $filtreleme("#myInput").val("");

                },
                error: function () {
                    alert("error");
                }
            });
        },

        ajaxPostJsonDownloadsFile : function (url_) {

          $.ajax({
              url: url_,
              type: "POST",
              cache: false,
              async : false,
              dataType: "json",
              success: function (deger) {
                
                  if (deger == false) {

                      $('body').prepend('<div class="alert alert-danger alert-fixed;" style="position:fixed; width:100%; top:0; z-index:99999; border-radius:0;">&nbsp;&nbsp;&nbsp;İŞLEM GERÇEKLEŞTİRİLEMİYOR</div>');

                      $(".alert").delay(3700).slideUp(200, function () {
                          $(this).alert('close');
                      });

                  }
                  else {
                      window.location.href = "/Downloads/DownloadFile?dosyaAdi=" + deger;
                  }
                
              },
              error: function (jqXHR, textStatus, error) {
                  alert(error);
              },

          });
        },

        ajaxPostJsoUploadFile: function (url_, formAdi) {

            $odeme.ajax({
                // Your server script to process the upload
                url: url_,
                type: 'POST',

                // Form data
                data: new FormData($odeme('#' + formAdi)[0]),

                // Tell jQuery not to process data or worry about content-type
                // You *must* include these options!
                cache: false,
                contentType: false,
                processData: false,
                success: function (sonuc) {

                    $('body').prepend('<div class="alert alert-success alert-fixed" style="position:fixed; width:100%; top:0; z-index:99999; border-radius:0;">✅&nbsp;&nbsp;&nbsp;Dosya Yüklendi</div>');

                    $(".alert").delay(3500).slideUp(200, function () {
                        $(this).alert('close');
                    });
                },               
                //Custom XMLHttpRequest (Progress bar eklemeyi unutma)
                xhr: function() {
                    var myXhr = $odeme.ajaxSettings.xhr();
                    if (myXhr.upload) {
                        // For handling the progress of the upload
                        myXhr.upload.addEventListener('progress', function (e) {
                          
                            if (e.lengthComputable) {
                                $odeme('progress').attr({
                                    value: e.loaded,
                                    max: e.total,
                                });
                            }
                        } , false);
                    }
                    return myXhr;
                },
            })                   
        },

        uyariModeli: function () {

          $('body').prepend('<div class="modal fade" id="modalUyari" role="dialog"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="H4">UYARI</h4></div><div class="modal-body" id="_rkio"><label class="unlemIcon">Gerekli alanlar doldurulmadan kayıt yapılamaz.</label><div class="text-center"><a href="#" id="uyariKapat" class="btn btn-success btn-sm pad15to15" data-dismiss="modal" aria-hidden="true"><i class="fa fa-check-circle" aria-hidden="true"></i>&nbsp;Tamam</a></div></div></div></div></div>');

          $("#modalUyari").modal();
        },

        modalPartial: function (url_, name) {

            $('body').prepend('<div class="modal fade" id="myModal" role="dialog"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title" id="H4">' + name + '</h4></div><div class="modal-body" id="_rkio"><div id="replaceData"></div</div></div></div></div>');          
           

            $('#myModal').modal({
                show: true,
                backdrop: 'static',
                keyboard: false
            })

            var $detailDiv = $('#replaceData');
            
            $.get(url_, function (data) {
                $detailDiv.replaceWith(data);
            });

        },

        alert: function () {

            $('body').prepend('<div class="alert alert-success alert-fixed">✅&nbsp;&nbsp;&nbsp;Yeni kayıt eklendi</div>');

            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });

        }
    };

}());