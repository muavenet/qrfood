

var $fixed = jQuery.noConflict();

$fixed("document").ready(function ($fixed) {


	var nav = $fixed('#header');

	$fixed(window).scroll(function () {
		if ($fixed(this).scrollTop() > 125) {
			nav.addClass("f-nav");
		} else {
			nav.removeClass("f-nav");
		}
	});



});