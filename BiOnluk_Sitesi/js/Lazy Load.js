

var $lazy_Load = jQuery.noConflict();

var skipCount = 3; // start at 6th record (assumes first 5 included in initial view)
var takeCount = 3 // return new 5 records
var hasMoreRecords = true;



	$lazy_Load('#_VitrinIlantiListView').on('click', '#load_more_btn', function () {

	$lazy_Load("#loadgifLoadMore").append("<img src='/Content/FrontEnd/images/load.GIF'/>");

	//e.preventDefault();
	//if (!hasMoreRecords) {
	//  return;
	//}

		$lazy_Load.ajax({

		url: '/FrontEndModules/_VitrinUrunler',
		data: { size: takeCount = takeCount + 2 },
		datatype: 'json',
		success: function (data) {

			$lazy_Load("#_VitrinIlantiListView").html(data);

			//if (data.length !== 0) {
			//	$lazy_Load(data.ModelString).insertBefore("#loadMore").hide().fadeIn(2000);
			//}

			//var ajaxModelCount = data.ModelCount - (loadCount * 4);
			//if (ajaxModelCount <= 0) {
			//	$lazy_Load("#loadMore").hide().fadeOut(2000);
			//}

			//   if (data.Count < 5) {
			//     hasMoreRecords = false; // signal no more records to display
			//}

		},
		error: function () {
			alert("error");
		}
	});
});