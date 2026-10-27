var $ParaFormat = jQuery.noConflict();

$ParaFormat('.currency').on('keyup', function () {
    jQuery(this).val(jQuery(this).val().split('.').join(''));
    jQuery(this).val((jQuery(this).val()).replace(/\B(?=(?:\d{3})+(?!\d))/g, "."));

});