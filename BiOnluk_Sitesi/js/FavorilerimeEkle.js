function FavorilerimeEkle(e) {

    var id = e.id;

    var $this = e;

    var $fav = jQuery.noConflict();

            

    $fav.ajax({
        url: '/MusteriPaneli/FavorilerimeEkle',
        type: "post",
        dataType: "JSON",
        data: { uid: id },
        async: true,
        success: function (sonuc) {

            if (sonuc == true)
            {
                $fav($this).text("✔");
            }

        }
    });
}

