var $odeme = jQuery.noConflict();


$odeme(document).ready(function () {


    $odeme(':file').on('change', function () {

        var file = this.files[0];

        if (file.size > 1048576) {

            $odeme("#upload-image-two").val('');        

            alert('Maksimum dosya boyutu : 1 MB');
         
        }
        // Also see .name, .type
    });


    var $validatorIcerik = $odeme("#formIcerik").validate({

        rules: {

            chckKabulEt: { required: true }

        },
        messages: {

            chckKabulEt: "&nbsp;<b style='color:red'>Sözleşmeyi kabul etmediniz</b> !"
        }

	});

	$odeme('#siparisiBaslat').click(function () {

		$odeme("#loadgifdiv").append("<img src='Content/FrontEnd/images/load.GIF'/><br/>Lütfen bekleyiniz...");

		$odeme.ajax({

			url: '/SiparisiTamamla/SiparisGir',
			type: 'POST',			
			data: new FormData($odeme('#formIcerik')[0]),
			cache: false,
			contentType: false,
			processData: false,
			success: function (sonuc) {

				$odeme('body').append(sonuc.Token);

				$odeme('#div_bildirimler').append('<br/><span class="title alert-info" style="display:block;"><i class="fa fa-check" style="color:green;"></i>Sipariş Başlatıldı<small class="text-info text-right" style="display:inline-block"></small></span>');
				

		

				$odeme("#formIcerik").html("");
			}

		});
		//window.prettyPrint && prettyPrint()
	});

	$odeme('#siparisTeslimEdildi').click(function () {

		$odeme("#loadgifdiv").append("<img src='Content/FrontEnd/images/load.GIF'/><br/>Lütfen bekleyiniz...");

		

		$odeme.ajax({

			url: '/SiparisiTamamla/SiparisTeslimEdildi',
			type: 'POST',			
			data: new FormData($odeme('#formYorum')[0]),
			cache: false,
			contentType: false,
			processData: false,
			success: function (sonuc) {

				$odeme('body').append(sonuc.Token);

				$odeme('#div_bildirimler')
					.append('<br/><span class="title alert-info" style="display:block;"><i class="fa fa-check" style="color:green;"></i> Comment has beend send.<small class="text-info text-right" style="display:inline-block"></small></span>');
				
				$odeme("#formYorum").html("");


			
			}

		});
		//window.prettyPrint && prettyPrint()
	});
});

//$odeme("#btn_HavaleSiparisi").click(function () {

//    alert("havale");

//    $odeme.ajax({
//        url: '/SiparisiTamamla/HavaleOdeme',
//        type: "post",
//        dataType: "JSON",
//        async: true,
//        success: function (sonuc) {

//            window.location.href = "/SiparisiTamamla/OdemeSonucSayfasi";

//        }
//    });
//})