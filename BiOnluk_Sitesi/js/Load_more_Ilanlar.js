var takeCount = 12

var $lazy_Load = jQuery.noConflict();

$lazy_Load('#_VitrinIlantiListView').on('click', '#load_more_btn', function () {

	$lazy_Load("#loadgifLoadMore").append("<img src='/Content/FrontEnd/images/load.GIF'/>");

	var me = $lazy_Load(this), data = me.data('key');

	var so = $lazy_Load("#hdnSiralama").val();

	var ilId_ = $hdn("#hdnIlId").val();

	console.log(so);

	//var aId = data.param1;
	//var aDi = data.param2;
	//var altKategoriId = data.param3;

	var aId = $lazy_Load("#hdnanaKategoriId").val();
	var aDi = $lazy_Load("#hdnanaKategoriAdi").val();
	var altKategoriId = $lazy_Load("#hdnanaAltKategoriId").val();

	$lazy_Load.ajax({

		url: '/FrontEndModules/_VitrinUrunler',
		data: { sortorder: so, AnaKategoriId: aId, AnaKategoriAdi: aDi, AltKategoriId: altKategoriId, size: takeCount = takeCount + 12, ilId: ilId_ },
		datatype: 'json',
		success: function (data) {

			$lazy_Load("#urunler_ust").html(data);

			$lazy_Load("#loadgifLoadMore").html("");
		},
		error: function () {
			alert("error");
		}
	});
});