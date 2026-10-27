

	var $filtreleme = jQuery.noConflict();

function filtreleme_clickler(AramaTerimi_, sortorder_, anaKategoriId_, anaKategoriAdi_, AltKategoriId_ , dropdownda_gösterilece_yazi) {

		$filtreleme("#drop_value_Selected").text(dropdownda_gösterilece_yazi);

		$filtreleme("#drop_value_Selected").val(sortorder_);

		$filtreleme("#loadgifLoadMore").append("<img src='/Content/FrontEnd/images/load.GIF'/>").fadeIn(200);

		var ke = AramaTerimi_;
		var so = sortorder_;	
		var aId = anaKategoriId_;
		var aDi = anaKategoriAdi_;
		var AltKId = AltKategoriId_;

		$filtreleme.ajax({

			url: '/Filtre/BasitAramaSonucPartialView_',
			data: { AramaTerimi: ke, sortorder: so, AnaKategoriId: aId, AnaKategoriAdi: aDi, AltKategoriId: AltKId, size: 10 },
			datatype: 'json',
			success: function (data) {

				$filtreleme("#_AramaMotoruSonucListView").html(data);

				$lazy_Load("#loadgifLoadMore").html("");
			},
			error: function () {
				alert("error");
			}
		});
};

	var $lazy_Load = jQuery.noConflict();

	var takeCount = 10 

	$lazy_Load('#_AramaMotoruSonucListViewGenel').on('click', '#load_more_btn', function () {

	$lazy_Load("#loadgifLoadMore").append("<img src='/Content/FrontEnd/images/load.GIF'/>");

		var me = $lazy_Load(this), data = me.data('key');

		var ke = data.param1;
		var so = $lazy_Load("#drop_value_Selected").val();
		var aId = data.param3;
		var aDi = data.param4;
		var AltKId = data.param5;
		var si = data.param6;	
		

		$lazy_Load.ajax({

			url: '/Filtre/BasitAramaSonucPartialView_',
			data: { AramaTerimi: ke, sortorder: so, AnaKategoriId: aId, AnaKategoriAdi: aDi, AltKategoriId: AltKId , size: takeCount = takeCount + 10 },
			datatype: 'json',
			success: function (data) {

				$lazy_Load("#_AramaMotoruSonucListView").html(data);

				$lazy_Load("#loadgifLoadMore").html("");
			},
			error: function () {
				alert("error");
			}
		});
	});