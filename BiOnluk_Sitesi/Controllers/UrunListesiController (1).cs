using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web;
using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using MvcPaging;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
//using PagedList;


namespace HemenTanitim.Controllers
{
    public class UrunListesiController : Controller
    {
     
        private HemenTanitimContext entity = new HemenTanitimContext();
        //
        // GET: /UrunKategorileriListele/
        public ActionResult Urunler()
        {

            var user = entity.Users.Find(User.Identity.GetUserId());
            ApplicationUser ap = new ApplicationUser();

            ap.hotelAdi =  user.hotelAdi;
            ap.odaNo = user.odaNo;
            //if (anaKategoriId == null && altKategoriId == null && aramaString == "")
            //{
            //    return HttpNotFound();
            //}

            //TempData["anakatId"] = anaKategoriId;

            //ViewBag.CurrentSort = sortOrder;

            //int pageSize = 20;

            //int pageNumber = (page ?? 1);



            //if (!string.IsNullOrEmpty(aramaString) && anaKategoriId == 0 && altKategoriId == 0)
            //{
            //    var tupleModel = new Tuple<IPagedList<UrunListele>, List<Fotolar>, List<Fotolar>, AltKategoriEkle>(AramayaGoreGetir(sortOrder, aramaString, tags).ToPagedList(pageNumber, pageSize), slaytList(), slaytListUzun(), AltKategoriGetir());
            //    return View(tupleModel);
            //}
            //else if (!string.IsNullOrEmpty(aramaTipi) && anaKategoriId == 0 && altKategoriId == 0 || altKategoriId == null)
            //{
            //    var tupleModel = new Tuple<IPagedList<UrunListele>, List<Fotolar>, List<Fotolar>, AltKategoriEkle>(WebVeKampanyayaGoreGetir(aramaTipi, sortOrder).ToPagedList(pageNumber, pageSize), slaytList(), slaytListUzun(), AltKategoriGetir());
            //    return View(tupleModel);
            //}            
            //else if (minFiyat > 0)
            //{
            //    var tupleModel = new Tuple<IPagedList<UrunListele>, List<Fotolar>, List<Fotolar>, AltKategoriEkle>(FiyataGoreGetir(sortOrder, altKategoriId, anaKategoriId, minFiyat, maxFiyat).ToPagedList(pageNumber, pageSize), slaytList(), slaytListUzun(), AltKategoriGetir());
            //        return View(tupleModel);               
            //}
            //else
            //{
            //    var tupleModel = new Tuple<IPagedList<UrunListele2>>(KategoriListesi(anaKategoriId, sortOrder).ToPagedList(pageNumber, pageSize));
            //    return View(tupleModel);
            
            //}

            return View(ap);
        }

        public ActionResult FiltreliUrunler()
        {
            return View();
        }

        public ActionResult ilanListeSayfasiFiltreli()
        {
            
            int AnaKategoriId = RouteData.Values["anaKategoriId"] == null ? 0 : int.Parse(RouteData.Values["anaKategoriId"].ToString());
            int AltKategoriId = RouteData.Values["AltKategoriId"] == null ? 0 : int.Parse(RouteData.Values["AltKategoriId"].ToString());
            int minFiyat = RouteData.Values["minFiyat"] == null ? 0 : int.Parse(RouteData.Values["minFiyat"].ToString());
            int maxFiyat = RouteData.Values["maxFiyat"] == null ? 0 : int.Parse(RouteData.Values["maxFiyat"].ToString());
            int minAlexa = RouteData.Values["minAlexa"] == null ? 0 : int.Parse(RouteData.Values["minAlexa"].ToString());
            int maxAlexa = RouteData.Values["maxAlexa"] == null ? 0 : int.Parse(RouteData.Values["maxAlexa"].ToString());
            bool DmozKaydi = RouteData.Values["DmozKaydi"] == null ? false : bool.Parse(RouteData.Values["DmozKaydi"].ToString());
            bool NewsKaydi = RouteData.Values["NewsKaydi"] == null ? false : bool.Parse(RouteData.Values["NewsKaydi"].ToString());
            string sortOrder = RouteData.Values["sortOrder"] == null ? "" : RouteData.Values["sortOrder"].ToString();

            int DefaultPageSize = 20;

            int pageNumber = RouteData.Values["page"] == null ? 1 : int.Parse(RouteData.Values["page"].ToString());

            var sql = entity.Database.SqlQuery<UrunListele2>("exec [stpr_UrunListesiFiltreliArama] " +
                " @AnaKategoriId, @AltKategoriId, @minFiyat, @maxFiyat, @minAlexa, @maxAlexa, @DmozKaydi, @NewsKaydi, @sortOrder ",
                new SqlParameter("AnaKategoriId", AnaKategoriId),
                new SqlParameter("AltKategoriId", AltKategoriId),
                new SqlParameter("minFiyat", minFiyat),
                new SqlParameter("maxFiyat", maxFiyat),
                new SqlParameter("minAlexa", minAlexa),
                new SqlParameter("maxAlexa", maxAlexa),
                new SqlParameter("DmozKaydi", DmozKaydi),
                new SqlParameter("NewsKaydi", NewsKaydi),
                new SqlParameter("sortOrder", sortOrder))
                .ToList();

            return PartialView(sql.ToPagedList(pageNumber, DefaultPageSize));
        }

        public ActionResult ilanListeSayfasi(string AnaKategoriAdi, int? AnaKategoriId, int? AltKategoriId, string sortOrder, int? page)
        {

            int DefaultPageSize = 20;

            int pageNumber = (page ?? 1);

            var sql = entity.Database.SqlQuery<UrunListele2>("exec [stpr_UrunListesi]  @AnaKategoriId, @AltKategoriId, @SortOrder ",
                new SqlParameter("AnaKategoriId", AnaKategoriId),
                new SqlParameter("AltKategoriId", AltKategoriId),
                new SqlParameter("SortOrder", sortOrder)).ToList();

            return PartialView(sql.ToPagedList(pageNumber, DefaultPageSize));
        }

        public ActionResult _SiralamaDropDownListesi()
        {
            return PartialView();
        }

        public AltKategoriEkle AltKategoriGetir()
        {
            int aid = Convert.ToInt32(RouteData.Values["altKategoriId"]);

            if (aid == 0)
            {
                return null;
            }
            return entity.AltKategoriEkles.Where(x => x.Id == aid).FirstOrDefault();        

        }

        public List<Fotolar> slaytList()
        {
            var res = entity.Fotolars.Where(z => z.KategoriSlayt_h248 == true).OrderBy(z => Guid.NewGuid()).Take(1).ToList();

            return res;
        }

        public List<Fotolar> slaytListUzun()
        {
            var res = entity.Fotolars.Where(z => z.KategoriSlayt_h430 == true).OrderBy(z => Guid.NewGuid()).Take(1).ToList();

            return res;
        }

        //public List<UrunListele> AramayaGoreGetir(string sortOrder, string arananKelime, int? tags)
        //{
        //    var tags_ = false;

        //    if (tags == 1)
        //    {
        //        tags_ = true;
        //    }

        //    var res = entity.Urunlers
        //        .Join(entity.Fotolars, x => x.Guid, x => x.UrunId, (x, y) => new { x, y })

        //             .OrderByDescending(a => a.y.UrunId)
        //             .GroupBy(a => a.x.Guid, (key, items) => items.FirstOrDefault())                    
        //             .Where(z => (!tags_) ? z.x.UrunAdi.Contains(arananKelime) : z.x.tags.Contains(arananKelime))

        //             .Select(a => new
        //             {
        //                 ID = a.x.UrunId,
        //                 UrunAdi = a.x.UrunAdi,
        //                 ResimYolu = a.y.ResimYolu,
        //                 fiyat = a.x.Fiyat,
        //                 ustuCizili = a.x.UstuCiziliFiyat,
        //                 indirimOrani = a.x.IndirimOrani

        //             })
        //                    .AsEnumerable().Select(x => new UrunListele
        //                    {
        //                        Id = x.ID,
        //                        UrunAdi = x.UrunAdi,
        //                        ResimYolu = x.ResimYolu,
        //                        Fiyat = x.fiyat,
        //                        IndirimOrani = x.indirimOrani



        //                    });

        //    //ViewBag.kategoriAdi = entity.AltKategoriEkles.Where(x => x.Id == Id).FirstOrDefault();

        //    switch (sortOrder)
        //    {
        //        case "Alfabetik":
        //            res = res.OrderBy(s => s.UrunAdi);
        //            break;
        //        case "Fiyat_azalan":
        //            res = res.OrderByDescending(s => s.Fiyat);
        //            break;
        //        case "Fiyat_Artan":
        //            res = res.OrderBy(s => s.Fiyat);
        //            break;

        //    }

        //    return res.ToList();
        //}


        //public ActionResult UrunListesiFiltreliArama(int AnaKategoriId, decimal? minFiyat, decimal? maxFiyat, decimal? minAlexa, decimal? maxAlexa, string? dmozKaydi, string? newsKaydi, string? arananKelime, string? sortOrder, int? page)
        //{


        //    int DefaultPageSize = 20;

        //    int pageNumber = (page ?? 1);

        //    var sql = entity.Database.SqlQuery<UrunListele2>("exec stpr_UrunListesiFiltreliArama  @AnaKategoriId, @SortOrder", new SqlParameter("AnaKategoriId", anakategori), new SqlParameter("SortOrder", sortOrder)).ToList();

        //    return PartialView(sql.ToPagedList(pageNumber, DefaultPageSize));

        //    return PartialView();
        //}


        //public ActionResult TekBirKategori()
        //{

        //    int aid = Convert.ToInt32(TempData["anakatId"]);
        //    ViewBag.TekBirKategori = (from post in entity.AnaKategorisEkles
        //                              where post.Id == aid
        //                              join urunler in entity.Urunlers on post.Id equals urunler.AnaKategoriId into g
        //                              select new KategoriSayi
        //                              {
        //                                  Id = post.Id,
        //                                  KategoriAdi = post.KategoriAdi,
        //                                  UrunAdet = g.Count()
        //                              });


        //    return PartialView(entity.AltKategoriEkles.Where(x => x.AnaKategoriId == aid).ToList());
        //}

        public ActionResult Trending()
        {
            var sql = entity.Database.SqlQuery<UrunListele2>("exec stpr_VitrinUrunleri")
                  .OrderByDescending(x => x.Tarih).Where(x => x.Vitrin == true);
            return View(sql);
        }

        public ActionResult KategoriSayfasi(int AnakategoriId, string AnaKategoriAdi)
        {
            var sql = entity.Database.SqlQuery<UrunListele2>("exec stpr_AnaKategoriyeGoreGetir @AnaKategoriId",
                new SqlParameter("AnaKategoriId", AnakategoriId)).ToList();
            return View(sql);
        }
    }
}