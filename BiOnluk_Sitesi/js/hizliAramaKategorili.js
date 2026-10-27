//Buradan İtibaren Hızlı Filtre Kategori İşlemleri

function HAltKatlariGetir(e) {

    var element = $fArama("#HAltKat li");
    var temizle1 = $fArama("#hizliAramaSiteAltKategori");


    $fArama.ajax({
        url: '/KategoriListesiGetir/HizliAramaaltKategorileriGetir',
        type: "GET",
        dataType: "JSON",
        data: { anaKatId: e },
        success: function (cities) {

            element.remove();

            temizle1.empty();

            temizle1.append("Site Kategorisi Seçiniz");

            $fArama.each(cities, function (i, city) {

                $fArama("#HAltKat").append(

                $fArama('<li><a href="#"><span id="firstKatAd">' + city.KategoriAdi + '</span> <span id="secondKatI" style="visibility:hidden">' + city.Id + '</span></a></li>'));
            });
        }
    });
}



$fArama("#btnHizliMenuArama").click(function () {


    var maxAlexa = $fArama("#hizliAramaMaxAlexa").val().replace(/\./g, "");
    var SiteKategoriAdi = $fArama("#hizliAramaSiteKategori span#firstKatAd").text();
    var SiteKategori = $fArama("#hizliAramaSiteKategori span#secondKatI").text();
    var SiteKategoriAltKatAdi = $fArama("#hizliAramaSiteAltKategori span#firstKatAd").text();
    var SiteAltKategori = $fArama("#hizliAramaSiteAltKategori span#secondKatI").text();

    if ($fArama("#hizliAramaSiteKategori").text() == "Kategori Seçiniz") {
        alert("Kategori Seçiniz");
        return false;
    }


    if ($fArama("#hizliAramaSiteAltKategori").text() == "Site Kategorisi Seçiniz") {
        alert("Site Kategorisi Seçiniz");
        return false;
    }



    if (maxAlexa == "")
        maxAlexa = 0;

    var url = "/UrunListesi/FiltreliUrunler/" + SiteKategoriAdi.trim() + "/" + SiteKategori.trim() + "/" + SiteKategoriAltKatAdi.trim() + "/" + SiteAltKategori.trim() + "/0/0/0/" + maxAlexa + "/false/false/en_yeniler/1" + "";

    window.location.href = url;


});

//Buradan İtibaren Hızlı Filtre Kategori İşlemleri Son
