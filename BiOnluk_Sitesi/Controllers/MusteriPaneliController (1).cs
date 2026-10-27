using GleamTech.VideoUltimate;
using HemenTanitim.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static HemenTanitim.MvcApplication;
using HemenTanitim.HtmlHelpers;
using System.Web.Helpers;
using System.Web.UI.WebControls;


namespace HemenTanitim.Controllers
{

    [Authorize]

    public class MusteriPaneliController : Controller
    {

        HemenTanitimContext db = new HemenTanitimContext();
        Utils ut = new Utils();


        public ActionResult Index()
        {


            return View();
        }

        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> files)
        {
            if (TempData["IsIlanId_Varolan_Ilan_3"] != null)
            {
                var klasorAdi = TempData["IsIlanId_Varolan_Ilan_3"];

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        // Some browsers send file names with full path.
                        // We are only interested in the file name.
                        var fileName = Path.GetFileName(file.FileName);

                        var physicalPath = Path.Combine(Server.MapPath("~/Content/uploads/Videolar/" + klasorAdi + "/" + fileName));

                        if (!Directory.Exists(Path.Combine(Server.MapPath("~/Content/uploads/Videolar/" + klasorAdi + "/"))))
                        {
                            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/uploads/Videolar/" + klasorAdi + "/")));
                        }

                        file.SaveAs(physicalPath);

                        string isId = TempData["IsIlanId_Varolan_Ilan_3"].ToString();
                        string uid = User.Identity.GetUserId();

                        Random random = new Random();
                        int rand = random.Next(1, 9999999);
                        FileInfo fi = new FileInfo(physicalPath);
                        string uzantisiz_file = Path.GetFileNameWithoutExtension(fi.Name);
                        string newfilename = Path.Combine(Server.MapPath("~/Content/uploads/Videolar/" + klasorAdi + "/" + uzantisiz_file + "___(" + rand.ToString() + ").jpg"));


                        using (var videoThumbnailer = new VideoThumbnailer(physicalPath))
                        using (var thumbnail = videoThumbnailer.GenerateThumbnail(300))
                            thumbnail.Save(newfilename, ImageFormat.Jpeg);

                        var ii = new IsIlanlariGaleri()
                        {
                            IsIlanId = isId,
                            Video = "/Content/uploads/Videolar/" + klasorAdi + "/" + fileName,
                            Fotograflar = "/Content/uploads/Videolar/" + klasorAdi + "/" + uzantisiz_file + "___(" + rand.ToString() + ").jpg",
                            userId = uid,
                            VideoDosyaAdi = fileName
                        };

                        db.IsIlanlariGaleris.Add(ii);
                        db.SaveChanges();
                    }
                }

            }

            // The Name of the Upload component is "files"

            TempData.Keep("IsIlanId_Varolan_Ilan_3");
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (TempData["IsIlanId_Varolan_Ilan_3"] != null)
            {
                var klasorAdi = TempData["IsIlanId_Varolan_Ilan_3"];

                if (fileNames != null)
                {
                    foreach (var fullName in fileNames)
                    {
                        var fileName = Path.GetFileName(fullName);

                        var physicalPath = Path.Combine(Server.MapPath("~/Content/uploads/Videolar/" + klasorAdi + "/" + fileName));

                        // TODO: Verify user permissions

                        if (System.IO.File.Exists(physicalPath))
                        {
                            var silinecekVideo = db.IsIlanlariGaleris.Where(x => x.Video == "~/Content/uploads/Videolar/" + klasorAdi + "/" + fileName).FirstOrDefault();
                            db.IsIlanlariGaleris.Remove(silinecekVideo);
                            db.SaveChanges();
                            // The files are not actually removed in this demo
                            System.IO.File.Delete(physicalPath);
                        }
                    }
                }
            }

            TempData.Keep("IsIlanId_Varolan_Ilan_3");
            return Content("");
        }

        public ActionResult VideoSil(int VideoId)
        {
            // The parameter of the Remove action must be called "fileNames"

            try
            {
                var silinecekVideo = db.IsIlanlariGaleris.Where(x => x.Id == VideoId).FirstOrDefault();

                if (silinecekVideo != null)
                {
                    var fileName = silinecekVideo.Video;

                    var physicalPath = Path.Combine(Server.MapPath(fileName));

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }

                    db.IsIlanlariGaleris.Remove(silinecekVideo);
                    db.SaveChanges();
                    List<IsIlanlariGaleri> Videolar = db.IsIlanlariGaleris.Where
                    (x => x.IsIlanId == silinecekVideo.IsIlanId & x.Video != null).ToList<IsIlanlariGaleri>();


                    return PartialView("VidolarPartialView", Videolar);
                }

                return new PartialViewResult();
            }
            catch (Exception)
            {

                return new PartialViewResult();
            }

            // Return an empty string to signify success

        }

        [HttpGet]
        public ActionResult VideolarPartialiRefreshEdenAction(string isIlanId_)
        {
            //ViewBag.videolar = new SelectList(db.IsIlanlariGaleris
            //    .Where(g => g.IsIlanId == isIlanId_ & g.Video != null), 
            //    "Id", "VideoDosyaAdi");

            //TempData.Keep("IsIlanId_Varolan_Ilan_3");
            var url = "";

            if (isIlanId_ == null)
            {
                url = TempData["IsIlanId_Varolan_Ilan_3"].ToString();
                TempData.Keep("IsIlanId_Varolan_Ilan_3");
            }
            else
                url = isIlanId_.Remove(isIlanId_.Length - 1);

            List<IsIlanlariGaleri> Videolar = db.IsIlanlariGaleris.Where(x => x.IsIlanId == url & x.Video != null).ToList<IsIlanlariGaleri>();



            return PartialView("VidolarPartialView", Videolar);
        }

        public ActionResult VidolarPartialView()
        {
            //List<IsIlanlariGaleri> Videolar = new List<IsIlanlariGaleri>();
            //var url = isIlanId.Remove(isIlanId.Length - 1);

            //List<IsIlanlariGaleri> Videolar = db.IsIlanlariGaleris.Where (x => x.IsIlanId == url & x.Video != null).ToList<IsIlanlariGaleri>();

            return PartialView();
        }

        public ActionResult Mesajlarim_()
        {
            string currentUserId = User.Identity.GetUserId();

            var sql = db.Mesajlars.Where(x => x.gonderenId == currentUserId || x.aliciId == currentUserId)
                .GroupBy(x => x.konuId).Select(x => x.OrderByDescending(v => v.tarih).FirstOrDefault()).ToList();

            return PartialView(sql.ToList());

        }

        public ActionResult MesajDetay(string konuId)
        {
            string currentUserId = User.Identity.GetUserId();


            //if (string.IsNullOrEmpty(aliciId))
            //{
            //    var qry = db.Mesajlars
            //       .Where(i => i.urunGuid == urunGuid)
            //       .OrderByDescending(m => m.id)
            //       .Take(1).FirstOrDefault();

            //    if (qry != null)
            //        aliciId = qry.gonderenId;
            //}

            string aliciId = "db8654cb-75b5-4e66-a7cc-3cbe7cea487c";

            if (aliciId != currentUserId)
            {
                var qry = db.Mesajlars
                   .Where(i => i.konuId == konuId)
                   .OrderByDescending(m => m.id)
                   .Take(1).FirstOrDefault();

                if (qry != null)
                {
                    aliciId = qry.aliciId;

                    db.Mesajlars.Where(i => i.aliciId == aliciId && i.konuId == konuId).ToList().ForEach(x => x.okundumu = true);

                    db.SaveChanges();
                }

            }
            else
            {
                db.Mesajlars.Where(i => i.aliciId == aliciId && i.konuId == konuId).ToList().ForEach(x => x.okundumu = true);

                db.SaveChanges();
            }

            return View(db.Mesajlars.Where(x => x.konuId == konuId && (x.gonderenId == currentUserId || x.aliciId == currentUserId)).ToList());
        }

        [ValidateInput(false)]
        public ActionResult MesajGonder(Mesajlar msg, FormCollection frm, HttpPostedFileBase yuklenendosya)
        {
            string currentUserId = User.Identity.GetUserId();


            string aliciId_ = "db8654cb-75b5-4e66-a7cc-3cbe7cea487c";

            var qry = db.Mesajlars
                .Where(i => i.aliciId == currentUserId || i.gonderenId == currentUserId && i.konuAcikMi == true)
                .OrderByDescending(c => c.tarih).FirstOrDefault();

            if (qry != null)
            {
                if (qry.konuAcikMi)
                    msg.konuId = qry.konuId;
                else
                    msg.konuId = Guid.NewGuid().ToString();
            }
            else
                msg.konuId = Guid.NewGuid().ToString();



            if (yuklenendosya != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(yuklenendosya.FileName);
                var uploadUrl = Server.MapPath("~/Content/Dosyalar");
                yuklenendosya.SaveAs(Path.Combine(uploadUrl, fileName));
                msg.dosyaNo = fileName;
            }
            else
                msg.dosyaNo = "";

            if (!String.IsNullOrEmpty(frm["urunadi"]) || !String.IsNullOrEmpty(frm["siparisId"]))
            {
                msg.urunadi = frm["urunadi"].ToString();
                msg.spId = frm["siparisId"].ToString();
            }


            msg.aliciId = aliciId_;
            msg.mesaj = frm["mesajicerik"].ToString();
            msg.tarih = DateTime.Now;
            msg.uyeadi = User.Identity.GetUserName();
            msg.gonderenId = User.Identity.GetUserId();

            msg.okundumu = true;
            msg.okundumuAdmin = false;
            msg.konuAcikMi = true;

            db.Mesajlars.Add(msg);
            db.SaveChanges();

            Utils u = new Utils();

            u.SendEMail("Destek Talebi", msg.mesaj, "mevamet@gmail.com");


            return Redirect("/MusteriPaneli/Mesajlarim_");
        }

        public ActionResult MesajSil(int id)
        {
            string currentUserId = User.Identity.GetUserId();

            Mesajlar fv = db.Mesajlars.Where(x => x.id == id && x.gonderenId == currentUserId).FirstOrDefault();

            db.Mesajlars.Remove(fv);

            db.SaveChanges();

            return RedirectToAction("Mesajlarim_");
        }

        public ActionResult SiparisIptal(string siparisId)
        {
            string uid = User.Identity.GetUserId();

            SiparisIptal sp = new SiparisIptal();

            sp.IptalEdenId = uid;
            sp.SiparisId = siparisId;
            sp.TalepKodu = ut.GenerateSiparisId(5);
            sp.iptalTarihi = DateTime.Now;

            db.SiparisIptals.Add(sp);
            db.SaveChanges();


            db.SiparisBilgileris.Where(x => x.SiparisId == siparisId).ToList()
                  .ForEach(x => x.siparisIptalTalebi = true);
            db.SaveChanges();

            TempData["SiparisIptalBilgisi"] = "Sipariş iptal talebiniz gönderilmiştir.";

            return RedirectToAction("SatinAldiklarim", "MusteriPaneli");
        }

        public ViewResult SatinAldiklarim()
        {
            string uid = User.Identity.GetUserId();

            return View(db.SiparisBilgileris.Where(x => x.SiparisVerenId == uid).ToList());
        }

        public ViewResult GelenSiparisler()
        {
            string uid = User.Identity.GetUserId();

            return View(db.SiparisBilgileris.Where(x => x.SiparisVerenId == uid).ToList());

        }

        //public ViewResult MusteriSiteleri()
        //{
        //    string uid = User.Identity.GetUserId();

        //    return View(db.MusteriSiteleris.Where(x => x.uyeId == uid).ToList());
        //}

        //public ViewResult MusteriSiteleriEkle()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult MusteriSiteleriEkle(MusteriSiteleri musterisiteleri)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.MusteriSiteleris.Add(musterisiteleri);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(musterisiteleri);
        //}       
        public ViewResult MusteriBankalari()
        {
            string uid = User.Identity.GetUserId();

            return View(db.BankaHesaplaris.Where(x => x.uyeId == uid).ToList());
        }

        public ActionResult FavorilerimeEkle(string uid)
        {
            FavoriUrunler fu = new FavoriUrunler();

            fu.KullaniciId = User.Identity.GetUserId();
            fu.isId = uid;

            db.FavoriUrunlers.Add(fu);
            db.SaveChanges();

            return Json(true);
        }

        public ViewResult FavoriUrunlerim()
        {
            var sql = db.Database.SqlQuery<FavoriUrunlerFotolarla>("exec stpr_favorilerim  @UYEID", new SqlParameter("UYEID", User.Identity.GetUserId())).ToList();

            return View(sql.ToList());
        }

        public ActionResult FavoriUrunSil(int uid)
        {
            FavoriUrunler fv = db.FavoriUrunlers.Find(uid);

            db.FavoriUrunlers.Remove(fv);
            db.SaveChanges();

            return RedirectToAction("FavoriUrunlerim");
        }

        public ActionResult NotificationMusteriPaneli()
        {
            //İptal şimdilik. Yeni satın alımlarda bildirim gi
            //var sql = db.Database.SqlQuery<MusteriPaneli_Counts>("exec stpr_Count @UyeId", new SqlParameter("UyeId", User.Identity.GetUserId())).FirstOrDefault();

            return PartialView();
        }

        [HttpPost]
        public ActionResult getEnAltKategoriIds(int anaKatId)
        {
            var enaltK = db.AltKategoriEkles.Where(a => a.AnaKategoriId == anaKatId).Select(a => "<option value='" + a.Id + "'>" + a.KategoriAdi + "</option>'");

            string chck = enaltK.FirstOrDefault();

            if (chck == null)
            {
                return Content(String.Join("", "<option value='0'>Bağlı kategori yok</option>"));
            }
            else
            {
                return Content(String.Join("", enaltK));
            }
        }

        public ActionResult BilgileriDegistir_PartialView(ManageMessageId? message)
        {
            string uid = User.Identity.GetUserId();

            BilgeriDegistir mnu = new BilgeriDegistir();

            var getdata = db.Users.Find(uid);

            mnu.Fotograf = getdata.Fotograf;
            mnu.Telefon = getdata.Telefon;
            mnu.EmailAdresi = getdata.Email;
            mnu.Hakkimda = getdata.Hakkimda;
            mnu.UserName = getdata.UserName;
            mnu.OrtalamaYanitSuresi = getdata.OrtalamaYanitSuresi;
            mnu.OncekiSiparisler = getdata.OncekiSiparisler;
            mnu.UyelikTarihi = getdata.UyelikTarihi;
            mnu.SonGorulmeTarihi = getdata.SonGorulmeTarihi;
            mnu.IptalEdilenler = getdata.IptalEdilenler;
            mnu.YorumSayisi = getdata.YorumSayisi;
            mnu.puan = getdata.puan_ortalamasi;
            mnu.kullaniciId = getdata.Id;

            ViewBag.ReturnUrl = Url.Action("BilgileriDegistir");

            ViewBag.StatusMessage =
                message == ManageMessageId.BilgilerGuncellendi ? "Bilgiler güncellendi." : "";


            return PartialView(mnu);
        }

        class Resmiyet
        {
            public int ID { get; set; }
            public string Adi { get; set; }
        }

        [HttpGet]
        public ActionResult BilgileriDegistir_Anasayfa(ManageMessageId? message, FormCollection frm)
        {
            string uid = User.Identity.GetUserId();

            BilgileriDegistir_Anasayfa mnu = new BilgileriDegistir_Anasayfa();

            var getdata = db.Users.Find(uid);

            ViewBag.IlleriGetir = new SelectList(db.iller, "il_id", "il_ad", getdata.Konum);

            mnu.Fotograf = getdata.Fotograf;
            mnu.Telefon = getdata.Telefon;
            mnu.EmailAdresi = getdata.Email;
            mnu.Hakkimda = getdata.Hakkimda;
            mnu.Sozlesme_icerigi = getdata.SozlesmeIcerigi;
            mnu.UserName = getdata.UserName;
            mnu.OrtalamaYanitSuresi = getdata.OrtalamaYanitSuresi;
            mnu.OncekiSiparisler = getdata.OncekiSiparisler;
            mnu.UyelikTarihi = getdata.UyelikTarihi;
            mnu.SonGorulmeTarihi = getdata.SonGorulmeTarihi;
            mnu.IptalEdilenler = getdata.IptalEdilenler;
            mnu.YorumSayisi = getdata.YorumSayisi;
            mnu.iban = getdata.iban;
            mnu.ad = getdata.ad;
            mnu.soyad = getdata.soyad;
            mnu.adres = getdata.adres;
            mnu.tckimlik = getdata.tckimlik;
          

           



            ViewBag.ReturnUrl = Url.Action("BilgileriDegistir_Anasayfa");

            ViewBag.StatusMessage =
                message == ManageMessageId.BilgilerGuncellendi ? "Bilgiler güncellendi." : "";


            return View(mnu);
        }

    
        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> BilgileriDegistir_Anasayfa(BilgileriDegistir_Anasayfa model, FormCollection frm,
            HttpPostedFileBase ProfilFotografi)
        {
            ViewBag.ReturnUrl = Url.Action("BilgileriDegistir_Anasayfa");

            string uid = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {

                var ss = db.Users.Find(uid);
                if (ProfilFotografi != null && ProfilFotografi.ContentLength > 0)
                {

                    var physicalPath = Path.Combine(Server.MapPath(ss.Fotograf));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }

                    Guid gd = Guid.NewGuid();
                    WebImage img = new WebImage(ProfilFotografi.InputStream);
                    if (img.Width > 100)
                         img.Resize(width: 250, height: 300, preserveAspectRatio: true, preventEnlarge: true);

                    img.Save(Path.Combine(Server.MapPath("/Content/uploads/Profil_fotograflari"), gd.ToString()));

                    ss.Fotograf = "/Content/uploads/Profil_fotograflari/" + gd.ToString() + "." + img.ImageFormat;                    

                }
                else
                    ss.Fotograf = ss.Fotograf;

                short kimdenId = Convert.ToInt16(frm["user_uzmanlik_kategorileri"]);

                ss.Email = model.EmailAdresi;
                ss.Telefon = model.Telefon;
                ss.Hakkimda = model.Hakkimda;
                ss.SozlesmeIcerigi = model.Sozlesme_icerigi;
                ss.Konum = Convert.ToInt16(frm["konum"]);
               

                db.Entry(ss).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                ViewBag.IlleriGetir = new SelectList(db.iller, "il_id", "il_ad", ss.Konum);

                return RedirectToAction("BilgileriDegistir_Anasayfa", new { Message = ManageMessageId.BilgilerGuncellendi });

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IsIlanlari()
        {
            System.Web.HttpContext.Current.Response.Cookies["RefreshFilter"].Expires = DateTime.Now.AddDays(-1);

            var sql = db.Database.SqlQuery<store_procedure_IsIlanlariListesi>("exec stpr_Portfolyom @UYEID",
                new SqlParameter("UyeId", User.Identity.GetUserId())).ToList();

            string userId = User.Identity.GetUserId();

            ViewBag.ibanKontrol = db.Users.Where(x => x.Id == userId).FirstOrDefault().iban;

            var kontrol = db.IsIlanlaris.Where(x => x.userId == userId & x.Trending == true).FirstOrDefault();

            if (kontrol != null)
                ViewBag.onecikarmaKontrol = kontrol.isId;
            else
                ViewBag.onecikarmaKontrol = null;

            return PartialView(sql.ToList());
        }

        public ActionResult Kullanicilar(string Id)
        {
            var sql = db.Database.SqlQuery<store_procedure_IsIlanlariListesi>("exec stpr_Portfolyom @UYEID", new SqlParameter("UyeId", Id)).ToList();

            return PartialView(sql.ToList());
        }


        public ActionResult Kullanicinin_Ozet_Bilgileri()
        {
            string uid = RouteData.Values["id"].ToString();

            BilgeriDegistir mnu = new BilgeriDegistir();

            var getdata = db.Users.Find(uid);

            mnu.Fotograf = getdata.Fotograf;
            mnu.Telefon = getdata.Telefon;
            mnu.EmailAdresi = getdata.Email;
            mnu.Hakkimda = getdata.Hakkimda;
            mnu.UserName = getdata.UserName;
            mnu.OncekiSiparisler = getdata.OncekiSiparisler;
            mnu.UyelikTarihi = getdata.UyelikTarihi;
            mnu.SonGorulmeTarihi = getdata.SonGorulmeTarihi;
            mnu.puan = getdata.puan_ortalamasi;
            mnu.YorumSayisi = getdata.YorumSayisi;
            mnu.kullaniciId = getdata.Id;

            ViewBag.ReturnUrl = Url.Action("BilgileriDegistir");

            return PartialView(mnu);
        }


        [HttpPost]
        public ActionResult IsIlanlariEkle_PartialView(FormCollection collection)
        {
            IsIlanlari isI = new IsIlanlari();

            isI.userId = User.Identity.GetUserId();

            db.IsIlanlaris.Add(isI);
            db.SaveChanges();

            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult IsIlanlari_Duzenle()
        
        {

            if (RouteData.Values["id"] != null) {

                string isIlanId = RouteData.Values["id"].ToString();

                IsIlanlari ilanlar = db.IsIlanlaris.Where(x => x.isId == isIlanId).FirstOrDefault();

                ViewBag.AnaKategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", ilanlar.AnaKategoriId);

                ViewBag.AltKategoriId = new SelectList(db.AltKategoriEkles.Where(x => x.AnaKategoriId == ilanlar.AnaKategoriId), "Id", "KategoriAdi", ilanlar.AltKategoriId);

                return View(ilanlar);

            }
            else {


                ViewBag.AnaKategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");

                ViewBag.AltKategoriId = new SelectList(db.AltKategoriEkles, "Id", "KategoriAdi");

                return View();
            }


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult IsIlanlari_Duzenle(IsIlanlari isI)
        {
            string baslik = isI.baslik.ToLower().FirstCharToUpper();

            if (RouteData.Values["id"] != null)
            {
                string isIlanId = RouteData.Values["id"].ToString();

                var ii = new IsIlanlari()
                {
                    isId = isIlanId,
                    AnaKategoriId = isI.AnaKategoriId,
                    AltKategoriId = isI.AltKategoriId,
                    baslik = ut.ToTemizle(baslik),
                    IlanOnizlemeUrl = isI.IlanOnizlemeUrl
                };

                db.IsIlanlaris.Attach(ii);

                db.Entry(ii).Property(x => x.AnaKategoriId).IsModified = true;
                db.Entry(ii).Property(x => x.AltKategoriId).IsModified = true;
                db.Entry(ii).Property(x => x.baslik).IsModified = true;

                db.SaveChanges();

                TempData["IsIlanId_Yeni_Ilan"] = ii.isId;

                return RedirectToAction("IsIlanlari_Duzenle_Devami", "MusteriPaneli", new { id = ii.isId });
            }
            else
            {
                isI.baslik = baslik;
                isI.isId = Guid.NewGuid().ToString();
                isI.Trending = false;
                isI.userId = User.Identity.GetUserId();
                isI.durum = true;
                isI.adminOnay = false;
                isI.Tarih = DateTime.Now;
                isI.IsIlanAciklamasi = "Aracınızın özelliklerini giriniz!";
              

                db.IsIlanlaris.Add(isI);
                db.SaveChanges();

                TempData["IsIlanId_Yeni_Ilan"] = isI.isId;

                return RedirectToAction("IsIlanlari_Duzenle_Devami", "MusteriPaneli");
            }
        }

        [Authorize(Roles = "Admin")]
        [RefreshDetectFilter]
        public ActionResult IsIlanlari_Duzenle_Devami()
        {
            if (RouteData.Values["IsRefreshed"] != null)
            {
                if ((bool)RouteData.Values["IsRefreshed"] == true)
                {
                    System.Web.HttpContext.Current.Response.Cookies["RefreshFilter"].Expires = DateTime.Now.AddDays(-1);

                    return RedirectToAction("IsIlanlari");
                }

            }

            if (TempData["IsIlanId_Yeni_Ilan"] != null)
            {
                string isIlanId = TempData["IsIlanId_Yeni_Ilan"].ToString();

                IsIlanlari ilanlar = db.IsIlanlaris.Where(x => x.isId == isIlanId).FirstOrDefault();

                ViewBag.videolar = new SelectList(db.IsIlanlariGaleris.Where(g => g.IsIlanId == isIlanId & g.Video != null), "Id", "VideoDosyaAdi");

                TempData["IsIlanId_Yeni_Ilan"] = ilanlar.isId;

                return View(ilanlar);

            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult IsAciklamas_IsIlanAciklama(FormCollection frm)
        {
            string uid = User.Identity.GetUserId();

            string isId;


            if (TempData["IsIlanId_Yeni_Ilan"] != null)
            {
                isId = TempData["IsIlanId_Yeni_Ilan"].ToString();

                var ii = new IsIlanlari() {

                    isId = isId,
                    IsIlanAciklamasi = frm["IsIlanAciklamasi"].ToString()

                };

                db.IsIlanlaris.Attach(ii);

                db.Entry(ii).Property(x => x.IsIlanAciklamasi).IsModified = true;
                db.Entry(ii).Property(x => x.IlanOnizlemeUrl).IsModified = true;

                TempData["IsIlanId_Yeni_Ilan"] = null;

                TempData["IsIlanId_Varolan_Ilan_2"] = ii.isId;

            }
            //else if (TempData["IsIlanId_Varolan_Ilan_1"] != null)
            //{
            //    isId = TempData["IsIlanId_Varolan_Ilan_1"].ToString();

            //    var ii = new IsIlanlari() { isId = isId, Gereksinimler = frm["Gereksinimler"].ToString() };

            //    db.IsIlanlaris.Attach(ii);

            //    db.Entry(ii).Property(x => x.Gereksinimler).IsModified = true;

            //    TempData["IsIlanId_Varolan_Ilan_1"] = null;

            //    TempData["IsIlanId_Varolan_Ilan_2"] = ii.isId;

            //}
            else if (TempData["IsIlanId_Varolan_Ilan_2"] != null)
            {
                isId = TempData["IsIlanId_Varolan_Ilan_2"].ToString();

                var ii = new IsIlanlari()
                {
                    isId = isId,                
                    Hot = Convert.ToBoolean(((string[])(frm.GetValue("Hot").RawValue))[0]),
                    Promoted = Convert.ToBoolean(((string[])(frm.GetValue("Promoted").RawValue))[0]),
                    Trending = Convert.ToBoolean(((string[])(frm.GetValue("Trending").RawValue))[0]),
                    mostPopular = Convert.ToBoolean(((string[])(frm.GetValue("mostPopular").RawValue))[0]),
                    mostSales = Convert.ToBoolean(((string[])(frm.GetValue("mostSales").RawValue))[0]),
                    Vitrin = Convert.ToBoolean(((string[])(frm.GetValue("Vitrin").RawValue))[0]),
                    TeslimSuresi = frm["TeslimSuresi"].ToString(),
                    kacKisilik = Convert.ToInt32(frm["kacKisilik"]),
                    indirimOrani = Convert.ToInt32(frm["indirimOrani"]),
                    fiyat = frm["fiyat"].ToString()
                };

                db.IsIlanlaris.Attach(ii);

                db.Entry(ii).Property(x => x.Vitrin).IsModified = true;
                db.Entry(ii).Property(x => x.mostPopular).IsModified = true;
                db.Entry(ii).Property(x => x.mostSales).IsModified = true;
                db.Entry(ii).Property(x => x.indirimOrani).IsModified = true;
                db.Entry(ii).Property(x => x.Promoted).IsModified = true;
                db.Entry(ii).Property(x => x.Hot).IsModified = true;
                db.Entry(ii).Property(x => x.Trending).IsModified = true;
                db.Entry(ii).Property(x => x.TeslimSuresi).IsModified = true;
                db.Entry(ii).Property(x => x.kacKisilik).IsModified = true;
                db.Entry(ii).Property(x => x.fiyat).IsModified = true;


                TempData["IsIlanId_Varolan_Ilan_2"] = null;
                TempData["IsIlanId_Varolan_Ilan_3"] = ii.isId;

            }
            else
            {
                return Json("İş ilan ekleme formunda hata oluştu.");
            }

            db.SaveChanges();

            return Json("1");
        }

        [HttpPost]
        public ActionResult ResimEkle_Metodu()
        {
            string uid = User.Identity.GetUserId();

            string isId;

            if (TempData["IsIlanId_Varolan_Ilan_3"] != null)
            {

                isId = TempData["IsIlanId_Varolan_Ilan_3"].ToString();

                List<string> Liste = ResimEkle();

                var ii = new IsIlanlariGaleri_stpr();

                if (Liste != null)
                {
                    ii = new IsIlanlariGaleri_stpr()
                    {
                        IsIlanId = isId,
                        Fotograflar = Liste
                    };
                }
                else
                {
                    ii = new IsIlanlariGaleri_stpr()
                    {
                        IsIlanId = isId,
                        Fotograflar = new List<string>()
                    };
                }


                var sql = db.Database.ExecuteSqlCommand("exec Is_Ilan_Fotograflari_Ekle @Fotograflar, @userId, @isIlanId",
                    new SqlParameter("Fotograflar", ii.StrFotograflar),
                    new SqlParameter("userId", uid),
                    new SqlParameter("isIlanId", isId)
                    );

                //db.IsIlanlaris.Attach(ii);

                //db.Entry(ii).Property(x => x.Fotograflar).IsModified = true;

                TempData["IsIlanId_Varolan_Ilan_3"] = null;

            }
            else
                return Json("0");


            return Json("1");
        }

        public List<string> ResimEkle()
        {
            List<string> Liste = new List<string>();

            string[] ss = System.Web.HttpContext.Current.Session["ListeEkle"] as string[];
            System.Web.HttpContext.Current.Session.Remove("ListeEkle");

            int vitrin = 0;
            string yol = "";
            Guid gd;
            int sayi = 0;
            string lstRes_ = "";

            string uid = User.Identity.GetUserId();

            if (!Directory.Exists(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid));
            }

            for (int i = 0; i < ss.Length; i++)
            {

                gd = Guid.NewGuid();

                if (ss[i].Contains("Vitrin"))
                {
                    vitrin = 1;
                    ss[i] = ss[i].Substring(ss[i].IndexOf("-") + 1);
                }

                else
                    vitrin = 0;


                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(ss[i])))
                {
                    using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                    {
                        if (!System.IO.File.Exists(Server.MapPath(yol + gd + ".jpg")))
                            bm2.Save(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg"), ImageFormat.Jpeg);
                    }
                }


                lstRes_ = "/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg";

                if (vitrin == 1)
                    Liste.Insert(0, lstRes_);

                else
                {
                    Liste.Insert(sayi, lstRes_);
                    sayi++;
                }


            }



            return Liste;
        }

        public List<string> ListeDuzenle;
        List<string> ListeSil;

        public List<string> ResimDuzenle()
        {
            ListeDuzenle = new List<string>();
            ListeSil = new List<string>();

            string[] ss = System.Web.HttpContext.Current.Session["ListeDuzenle"] as string[];
            System.Web.HttpContext.Current.Session.Remove("ListeDuzenle");

            int vitrin = 0;
            string yol = "";
            Guid gd;
            int sayi = 0;
            string lstRes_ = "";

            string uid = User.Identity.GetUserId();

            if (!Directory.Exists(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid));
            }

            for (int i = 0; i < ss.Length; i++)
            {

                string deger = "";

                gd = Guid.NewGuid();

                string name = ss[i].Substring(ss[i].LastIndexOf(",") + 1);

                string nameForGuid = ut.sonharfleriSil(name, 4);

                if (!System.IO.File.Exists(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + name)))
                {
                    gd = Guid.NewGuid();
                }
                else
                    gd = new Guid(nameForGuid);

                if (ss[i].Contains("Vitrin"))
                {
                    vitrin = 1;
                    ss[i] = ss[i].Substring(ss[i].IndexOf("-") + 1);
                }

                else
                    vitrin = 0;

                deger = ss[i].Substring(0, ss[i].LastIndexOf(","));

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(deger)))
                {
                    using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                    {
                        if (!System.IO.File.Exists(Server.MapPath(yol + gd + ".jpg")))
                            bm2.Save(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg"), ImageFormat.Jpeg);
                    }
                }



                lstRes_ = "/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg";

                if (vitrin == 1)
                    ListeDuzenle.Insert(0, lstRes_);
                else
                {
                    ListeDuzenle.Insert(sayi, lstRes_);
                    sayi++;
                }

            }

            string[] silinecek = System.Web.HttpContext.Current.Session["ListeSil"] as string[];
            System.Web.HttpContext.Current.Session.Remove("ListeSil");

            if (silinecek.Length > -1)
            {
                for (int i = 0; i < silinecek.Length; i++)
                {
                    yol = Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + silinecek[i]);
                    if (System.IO.File.Exists(yol))
                        System.IO.File.Delete(yol);
                    string dbYol = "/Content/uploads/Is_Ilanlari/" + uid + "/" + silinecek[i];
                    ListeSil.Add(dbYol);
                }

            }


            return ListeDuzenle;
        }

        [HttpPost]
        public ActionResult ResimDuzenle_Metodu()
        {
            string uid = User.Identity.GetUserId();

            string isId;

            if (TempData["IsIlanId_Varolan_Ilan_3"] != null)
            {

                isId = TempData["IsIlanId_Varolan_Ilan_3"].ToString();

                List<string> Liste = ResimDuzenle();

                var ii = new IsIlanlariGaleri_stpr();

                if (Liste != null)
                {
                    ii = new IsIlanlariGaleri_stpr()
                    {
                        IsIlanId = isId,
                        Fotograflar = Liste
                    };
                }
                else
                {
                    ii = new IsIlanlariGaleri_stpr()
                    {
                        IsIlanId = isId,
                        Fotograflar = new List<string>()
                    };
                }

                var sql = db.Database.ExecuteSqlCommand("exec Is_Ilan_Fotograflari_Duzenle @Fotograflar, @userId, @isIlanId, @SilinenFotograflar",
                    new SqlParameter("Fotograflar", ii.StrFotograflar),
                    new SqlParameter("SilinenFotograflar", ii.StrSilinenFotograflar),
                    new SqlParameter("userId", uid),
                    new SqlParameter("isIlanId", isId)
                    );

                //db.IsIlanlaris.Attach(ii);

                //db.Entry(ii).Property(x => x.Fotograflar).IsModified = true;

                TempData["IsIlanId_Varolan_Ilan_3"] = null;

                System.Web.HttpContext.Current.Response.Cookies["RefreshFilter"].Expires = DateTime.Now.AddDays(-1);

            }
            else
                return Json("0");


            return Json("1");
        }

        public class FotolariGetir_Gonderilecek_Modeller {

            public string FotoPath { get; set; }

            public string ilanName { get; set; }
        }

        //[WebMethod]
        //[ScriptMethod]
        public string FotolariGetir(string isIlanId)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            string s = "";

            var ilan = (from cek in db.IsIlanlariGaleris where cek.IsIlanId == isIlanId & cek.Video == null
                        select new FotolariGetir_Gonderilecek_Modeller
                        {
                            FotoPath = cek.Fotograflar,
                            ilanName = cek.Fotograflar
                        }).ToList().Select(x => new FotolariGetir_Gonderilecek_Modeller
                        {
                            FotoPath = x.FotoPath,
                            ilanName = x.ilanName.ToString().Substring(x.ilanName.ToString().LastIndexOf("/") + 1)

                        });

            s = serializer.Serialize(ilan);

            return serializer.Serialize(ilan);

        }

        public ActionResult IsIlanlari_OneCikarPasif(string isilanId)
        {
            var ii = new IsIlanlari()
            {
                isId = isilanId,
                Trending = false
            };

            db.IsIlanlaris.Attach(ii);
            db.Entry(ii).Property(x => x.Trending).IsModified = true;

            db.SaveChanges();

            return RedirectToAction("IsIlanlari");
        }

        public ActionResult IsIlanlari_OneCikarAktif(string isilanId)
        {
            var ii = new IsIlanlari()
            {
                isId = isilanId,
                Trending = true
            };

            db.IsIlanlaris.Attach(ii);
            db.Entry(ii).Property(x => x.Trending).IsModified = true;

            db.SaveChanges();

            return RedirectToAction("IsIlanlari");
        }

        public ActionResult IsIlanlari_AktifEt(string isilanId)
        {
            var ii = new IsIlanlari()
            {
                isId = isilanId,
                durum = true
            };

            db.IsIlanlaris.Attach(ii);
            db.Entry(ii).Property(x => x.durum).IsModified = true;

            db.SaveChanges();

            return RedirectToAction("IsIlanlari");
        }

        public ActionResult IsIlanlari_Durdur(string isilanId)
        {
            var ii = new IsIlanlari()
            {
                isId = isilanId,
                durum = false,
                Trending = false
            };

            db.IsIlanlaris.Attach(ii);
            db.Entry(ii).Property(x => x.durum).IsModified = true;
            db.Entry(ii).Property(x => x.Trending).IsModified = true;


            db.SaveChanges();

            return RedirectToAction("IsIlanlari");
        }

        public ActionResult IsIlanlari_IlanSilinme(string isilanId)
        {
            var ii = new IsIlanlari()
            {
                isId = isilanId,
                ilan_silinmesi = true
            };

            db.IsIlanlaris.Attach(ii);
            db.Entry(ii).Property(x => x.ilan_silinmesi).IsModified = true;

            db.SaveChanges();

            return RedirectToAction("IsIlanlari");
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Ilan_Vermek_Icın_Eksikleri_Gir(FormCollection frm)
        {

            string userId = User.Identity.GetUserId();

            ApplicationUser user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            user.iban = frm["ibanInput"];
            user.adres = frm["user_adres"];
            user.tckimlik = frm["user_tckimlikno"];
            user.soyad = frm["user_soyad"];
            user.ad = frm["user_ad"];
            user.Konum = Convert.ToInt16(frm["user_sehir"]);

            Options options = new Options();
            options.ApiKey = "sandbox-ShPCvKbmR4WFis53kkeYFe9fCzpND9yl";
            options.SecretKey = "sandbox-aK4Qo9GmI0Txv1BafLOtYeGRVsBy65aJ";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreateSubMerchantRequest request = new CreateSubMerchantRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = user.Id;
            request.SubMerchantExternalId = user.Id;
            request.SubMerchantType = SubMerchantType.PERSONAL.ToString();
            request.Address = user.adres;
            request.ContactName = user.ad;
            request.ContactSurname = user.soyad;
            request.Email = user.Email;
            request.GsmNumber = user.Telefon; /*"+905350000000"*/
            request.Name = user.UserName + "kişisinin hesabı";
            request.Iban = user.iban.Replace(" ", "");
            request.IdentityNumber = user.tckimlik;
            request.Currency = Currency.TRY.ToString();

            SubMerchant subMerchant = SubMerchant.Create(request, options);

            if (subMerchant.Status == "success")
            {
                //user.subMerchantKey = subMerchant.SubMerchantKey;
                //db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();

                return Json(subMerchant.Status);
            }
            else
            {
                return Json(subMerchant.ErrorMessage);
            }

        }

        public ActionResult Chat()
        {



            return View();
        }

        public ActionResult Portfolyo()
        {
            return View();
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            BilgilerGuncellendi,
            Error
        }

        public ActionResult denemetest()
        {
            return View();
        }
      
    }
}