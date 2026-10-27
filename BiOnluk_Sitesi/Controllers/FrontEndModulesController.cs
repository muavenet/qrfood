using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Json;
using System.Data.SqlClient;

namespace HemenTanitim.Controllers
{
    public class FrontEndModulesController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();
        //
        // GET: /FrontEndModules/
        public ActionResult _SocialMediaShare()
        {
            return PartialView();
        }

    

        public ActionResult _Footer()
        {
            var DBdata = db.Database.SqlQuery<FooterGenelBilgilerBirlesimi>("exec stpr_Footer").FirstOrDefault();

            return PartialView(DBdata);           
        }

        public ActionResult _LastTweets()
        {
            return PartialView(db.GenelBilgilers.ToList().Select(s => new GenelBilgiler { TwitterKullaniciAdi = s.TwitterKullaniciAdi}).FirstOrDefault());
        }

        public ActionResult _Slayt()
        {
            var list = db.Fotolars.Where(z => z.Slayt == true & z.AnasayfaEnUzunBanner == false & z.KategoriSlayt_h248 == false & z.SlaytKucukAnasayfa1 == false & z.SlaytKucukAnasayfa2 == false & z.KategoriSlayt_h430 == false & z.BiziTercihEdenFirmalar == false).ToList();            

            return PartialView(list);
        }

        public ActionResult _AnasayfaCiftBanner()
        {
            var list = db.Fotolars.Where(z => z.SlaytKucukAnasayfa1 == true | z.SlaytKucukAnasayfa2 == true).ToList();           
       
            return PartialView(list);
        }

        public ActionResult _AnasayfaEnUzunBanner()
        {
            var single = db.Fotolars.Where(z => z.AnasayfaEnUzunBanner == true).SingleOrDefault();

            return PartialView(single);
        }

        public ActionResult _Arama()
        {
            return PartialView();
        }
 
        public ActionResult _AramaLink(Urunler urunler)
        {
            return RedirectToRoute("Arama", new { sortOrder = "alfabetik", altKategoriId = urunler.UrunId, AnaKategoriId = 0, aramaString = urunler.UrunAdi });
        }

        public JsonResult AutoComplete(string kelime)
        {

            var sonuc = db.Urunlers.Where(x => x.UrunAdi.Contains(kelime)).Select(x => new { val = x.UrunAdi }).ToList();


            return Json(sonuc, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BlogSon3()
        {
            var res = db.Blogs.Take(3).ToList();

            return PartialView(res);
        }

        public ActionResult _BlogWidget()
        {
            var res = db.Blogs.OrderByDescending(x => x.Id).Take(9).ToList();

            return PartialView(res);
        }
        
         public ActionResult _BlogWidgetDetailPage()
        {
            var res = db.GaleriBasliks.OrderByDescending(x => x.Id).Take(9).ToList();

            return PartialView(res);
        }

        public ActionResult _Iller()
        {
            return PartialView(db.iller.ToList());
        }
        //public ActionResult _VitrinUrunler(string sortOrder,
        //    int AnaKategoriId, string AnaKategoriAdi,
        //    int? AltKategoriId, int size, int ilId)
        //{
        //    List<Urunler> model = new List<Urunler>();
        //    var vitrinUrunleri = db.IsIlanlaris.Where(x => x.Trending == true).ToList();
        //    if (vitrinUrunleri.Any())
        //    {
        //        model = vitrinUrunleri;
        //        return PartialView(model);
        //    }               
        //    else
        //        return PartialView();

        //}
        public ActionResult _VitrinUrunler()
        {
            var sql = db.Database.SqlQuery<UrunListele2>("exec stpr_VitrinUrunleri").OrderByDescending(x => x.Tarih); ;

            return PartialView(sql);
        }

        public ActionResult _AnaSayfa_Vitrini()
        {

            var sql = db.Database.SqlQuery<UrunListele2>("exec stpr_VitrinUrunleri").OrderByDescending(x => x.Tarih); 

            //if (sortOrder == "en_yeniler")
            //{   
            //    sql = sql.ToList().OrderByDescending(x => x.Trending).ThenBy(x => x.Tarih);
            //}
            //else if (sortOrder == "fiyat_artan")
            //{
            //    sql = sql.ToList().OrderByDescending(x => x.Trending).ThenBy(x => x.MinFiyat);
            //}
            //else if (sortOrder == "fiyat_azalan")
            //{
            //    sql = sql.ToList().OrderByDescending(x => x.Trending).ThenByDescending(x => x.MinFiyat);
            //}
            //else
            //{
            //    sql = sql.ToList().OrderByDescending(x => x.Tarih);
            //}

            return PartialView(sql);
        }

        public ActionResult _Ana_Kategoriler()
        {
            return PartialView(db.AnaKategorisEkles.ToList());
        }
    }
}