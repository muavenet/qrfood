using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HemenTanitim
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*url}", new { url = @".*\.asmx(/.*)?" });

            //routes.AppendTrailingSlash = true;
            //routes.LowercaseUrls = true;

            routes.MapRoute(
                 name: "VericPage",
                 url: "Account/Verification/{email}/{hotelId}/{odaId}",
                 defaults: new { controller = "Account", action = "Verification" }
             );


            routes.MapRoute(
                  name: "LoginPage",
                  url: "Account/Login/{hotelId}/{odaId}",
                  defaults: new { controller = "Account", action = "Login" }
              );

            routes.MapRoute(
                name: "DomainRoute",       
                "{controller}-{action}.example.com",     // Domain with parameters
                "{id}",    // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
                );


            routes.MapRoute(
           name: "MDetay",
           url: "kiralikkaravan/{kelimeadi}/{il}/{ilce}",
           defaults: new { controller = "kiralikkaravan", action = "Index", il = UrlParameter.Optional , ilce = UrlParameter.Optional}
         );

            routes.MapRoute(
                name: "FiltreliArama2",
                url: "UrunListesi/FiltreliUrunler/{AnaKategoriAdi}/{AnaKategoriId}/{AltKategoriAdi}/{AltKategoriId}/{minFiyat}/{maxFiyat}/{minAlexa}/{maxAlexa}/{DmozKaydi}/{NewsKaydi}/{sortOrder}/{page}",
                defaults: new { controller = "UrunListesi", action = "FiltreliUrunler", 
                    AltKategoriAdi = UrlParameter.Optional,AltKategoriId= UrlParameter.Optional,  arananKelime = UrlParameter.Optional,
                    minAlexa = UrlParameter.Optional, maxAlexa = UrlParameter.Optional, anakategori = UrlParameter.Optional,
                    maxFiyat = UrlParameter.Optional, minFiyat = UrlParameter.Optional, page = UrlParameter.Optional }
                );


             routes.MapRoute(
                 name: "Kategoriler",
                 url: "UrunListesi/{sortOrder}/{anaKategoriId}/{anaKategoriAdi}/{AltKategoriAdi}/{AltKategoriId}/{ilId}",
                 defaults: new
                 {
                     controller = "UrunListesi",
                     action = "Urunler",
                     anaKategoriId = UrlParameter.Optional,
                     anaKategoriAdi = UrlParameter.Optional,
                     page = UrlParameter.Optional


                 }
               );




       routes.MapRoute(
                name: "AnaKat",
                url: "UrunListesi/KategoriSayfasi/{AnakategoriId}/{AnaKategoriAdi}",
                defaults: new { controller = "UrunListesi", action = "KategoriSayfasi" }
            );



            routes.MapRoute(
                name: "AramaMotoruSonuc",
                url: "Filtre/BasitAramaSonuc/{AramaTerimi}/{size}/{AltKategoriId}",
                defaults: new { controller = "Filtre", action = "BasitAramaSonuc" }
            );

       


            routes.MapRoute(
                name: "Blog",
                url: "Home/{urunAdi}/{Id}",
                defaults: new { controller = "Home", action = "BlogDetail" }
            );

            routes.MapRoute(
                name: "Arama",
                url: "UrunListesi/{sortOrder}/{altKategoriId}/{AnaKategoriId}/{aramaString}",
                defaults: new { controller = "UrunListesi", action = "Urunler" }

           );

            routes.MapRoute(
                name: "AramaTipi",
                url: "UrunListesi/{aramaTipi}/{sortOrder}/{AnaKategoriId}",
                defaults: new { controller = "UrunListesi", action = "Urunler" }
             );


            routes.MapRoute(
                name: "sepet",
                url: "UrunDetay/SepeteAt/{ilanAdi}/{ilan_Id}/{spFiyat}/{ilan_Aciklama}/{teslimatSuresi}/{ilanSahibiId}/{ilanGereksinimleri}/{spilanResim}/{spilanKullaniciFoto}/{hdAdet}",
                defaults: new { controller = "UrunDetay", action = "SepeteAt" }
            );


            routes.MapRoute(
                name: "OdemeSayfasi",
                url: "SiparisiTamamla/OdemeSayfasi/{siparisId}",
                defaults: new { controller = "SiparisiTamamla", action = "OdemeSayfasi" }
           );

            routes.MapRoute(
                name: "siparisOnayla",
                url: "SiparisiTamamla/siparisOnayla/{siparisId_}",
                defaults: new { controller = "SiparisiTamamla", action = "siparisOnayla" }
           );


         

            routes.MapRoute(
                name: "ZiyaretciKaydet",
                url: "UrunDetay/ZiyaretciKaydet/{aid}",
                defaults: new { controller = "UrunDetay", action = "ZiyaretciKaydet", aid = UrlParameter.Optional }
            );



            routes.MapRoute(
                name: "UrunDetay",
               url: "UrunDetay/{urunAdi}/{isId}",
                defaults: new { controller = "UrunDetay", action = "Index" }
               );

       

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }

    }
}
