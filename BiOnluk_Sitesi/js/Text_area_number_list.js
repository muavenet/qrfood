var $newList = jQuery.noConflict();

// setup editors
$newList(".editor").each(function () {
	$newList(this).append($newList("#ref_line").html());
});
// line focus / blur
$newList(".editor").on("focus", ".line pre", function () {
	var pre = $newList(this);
	if (pre.text() == " ")
		pre.text("");
});
$newList(".editor").on("blur", ".line pre", function () {
	var pre = $newList(this);
	if (pre.text() == "")
		pre.text(" ");
});
// line add / remove
$newList(".editor").on("keydown", ".line pre", function (event) {
	var pre = $newList(this);
	if (event.which == 13) {
		event.stopPropagation();
		event.preventDefault();
		pre.parent().after($newList("#ref_line").html());
		pre.blur();
		pre.parent().next().find("pre").focus();
	} else if (event.which == 8 && pre.text() == "" && this != pre.parents(".editor").find("pre:first")[0]) {
		var back = pre.parent().prev();
		pre.parent().remove();
		back.find("pre").focus();
	}
});









