
		var $odeme = jQuery.noConflict();

		$odeme(document).ready(function () {	

			// Dosya Boyutu Kontrol
			//$odeme(':file').on('change', function () {

			//    var file = this.files[0];

			//    if (file.size > 1048576) {

			//        $odeme("#upload-image-two").val('');        

			//        alert('Maksimum dosya boyutu : 1 MB');
         
			//    }
			//    // Also see .name, .type
			//});

			console.log("pess;")

			var $validatorIcerik = $odeme("#formIcerik").validate({

				rules: {

					chckKabulEt: { required: true }

				},
				messages: {

					chckKabulEt: "&nbsp;<b style='color:red'> Sözleşmeyi kabul etmediniz </b> !"

				}
			});


			$odeme('#rootwizard').bootstrapWizard ({
		
				'onTabClick': function (tab, navigation, index) { return false; },

				'tabClass': 'nav nav-pills',

				'onNext': function (tab, navigation, index) {
		
					//var $total = navigation.find('li').length - 1;
					//var $current = index + 1;
					//var $percent = ($current / $total) * 100;					

					if (index == 1) {					

						var $valid = $odeme("#formIcerik").valid();		

						if (!$valid) {								

							$validatorIcerik.focusInvalid();
							return false;
						}
						else {

							$odeme("#loadgifdiv").append("<img src='/Content/FrontEnd/images/load.GIF'/>");

							$odeme.ajax({                        
								url: '/MusteriPaneli/IsAciklamas_IsIlanAciklama',
								type: 'POST',
								// Form data
								data: new FormData($odeme('#formIcerik')[0]),
								cache: false,
								contentType: false,
								processData: false,
								success: function (sonuc) {

									$odeme("#loadgifdiv").html("");		

									$odeme('#rootwizard').bootstrapWizard('show', 1); // Next Tab
								}                      
							});

						}
					}

					if (index == 2) {

						var $valid = $odeme("#formIcerik").valid();			

						if (!$valid) {

							$validatorIcerik.focusInvalid();
									

							return false;
						}
						else {

							$odeme("#loadgifdiv").append("<img src='/Content/FrontEnd/images/load.GIF'/>");

							$odeme.ajax({
								url: '/MusteriPaneli/IsAciklamas_IsIlanAciklama',
								type: 'POST',
								// Form data
								data: new FormData($odeme('#formIcerik')[0]),
								cache: false,
								contentType: false,
								processData: false,
								success: function (sonuc) {

									$odeme("#loadgifdiv").html("");

									$odeme('#rootwizard').bootstrapWizard('show', 2); // Next Tab	

								}

							});

						}
					}

					if (index == 3) {

						var $valid = $odeme("#formIcerik").valid();

						if (!$valid) {

							$validatorIcerik.focusInvalid();

							return false;
						}
						else {

							$odeme("#loadgifdiv").append("<img src='/Content/FrontEnd/images/load.GIF'/>");

							$odeme.ajax({
								url: '/MusteriPaneli/IsAciklamas_IsIlanAciklama',
								type: 'POST',
								// Form data
								data: new FormData($odeme('#formIcerik')[0]),
								cache: false,
								contentType: false,
								processData: false,
								success: function (sonuc) {

									//Videoları Getiriyoruz (Ajax > Partial > Div)  
									var segments = window.location.href.split('/');							
									var id = segments[5];
									$odeme.get('/MusteriPaneli/VideolarPartialiRefreshEdenAction', { isIlanId_: id }, function (data) {
										$odeme("#VideolarListPartial").html(data);
									});

									$odeme("#loadgifdiv").html("");

									$odeme('#rootwizard').bootstrapWizard('show', 3); // Next Tab




								}

							});

						}
					}

				}
			});
		});



		$odeme("#btn_HavaleSiparisi").click(function () {

			alert("havale");

			$odeme.ajax({
				url: '/SiparisiTamamla/HavaleOdeme',
				type: "post",
				dataType: "JSON",
				async: true,
				success: function (sonuc) {

					window.location.href = "/SiparisiTamamla/OdemeSonucSayfasi";

				}
			});
		});