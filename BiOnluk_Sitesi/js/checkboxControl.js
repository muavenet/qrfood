
function checkForm(form) {
    if (!form.signing.checked) {

        alert(form.signing.checked);

        $("#uySpan").html("").css("color", "red").append("Lütfen üyelik şartlarını kabul ediniz.");

        form.signing.focus();

        return false;
    }
    return true;
}