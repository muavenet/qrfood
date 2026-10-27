
function VideoKaldir(e) {

	var $vdSil = jQuery.noConflict();

	$vdSil("#silmeLoadGif").css("visibility", "visible");
	
	var id = e.id;
	var isIlanId = e.name;
	var $this = e;

	
	$vdSil.post('/MusteriPaneli/VideoSil', { videoId: id }, function (data) {

		$vdSil("#VideolarListPartial").html(data);

	});

	$vdSil("#silmeLoadGif").css("visibility","hidden");
};