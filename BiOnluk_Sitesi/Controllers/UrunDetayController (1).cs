using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HemenTanitim.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Data.SqlClient;


namespace HemenTanitim.Controllers
{
    public class UrunDetayController : Controller
    {
        Fotolar fotos = new Fotolar();
        private HemenTanitimContext db = new HemenTanitimContext();


        //GET: /UrunDetay/


        [HttpGet, ValidateInput(false)]
        public ActionResult Index(string urunAdi, string isId)
        {

            if (isId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ilanlar = db.Database.SqlQuery<IsIlanlariListele_IlanDetay>("exec [stpr_UrunDetay]  @isId",
                                                new SqlParameter("isId", isId)).FirstOrDefault();

            if (ilanlar == null)
            {
                return HttpNotFound();
            }

            return View(ilanlar);


        }

        public JsonResult ZiyaretciKaydet(int? aid)
        {
            if (ModelState.IsValid)
            {
                Urunler urunler = db.Urunlers.Find(aid);

                int? ziyaretci = urunler.ZiyaretciSayisi;
                ziyaretci++;
                urunler.ZiyaretciSayisi = ziyaretci;

                db.Entry(urunler).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BenzerUrunler(int? ID)
        {
            if (ModelState.IsValid)
            {
                Urunler urunler = db.Urunlers.Find(ID);

                int? ziyaretci = urunler.ZiyaretciSayisi;

                ziyaretci++;

                urunler.ZiyaretciSayisi = ziyaretci;

                db.Entry(urunler).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json("true");
        }

        [ValidateInput(false)]

        public JsonResult SepeteAt(string ilanAdi, string ilan_Id, string spFiyat, string spAciklama, string teslimatSuresi, string ilanSahibiId,
            string ilanGereksinimleri, string spilanResim, string spilanKullaniciFoto, int hdAdet)
        {
            if (ilan_Id != "")
            {
                string KullaniciId = User.Identity.GetUserId();


                //var sepetSil = db.Sepets.Where(s => s.SiparisVerenId == KullaniciId).ToList();
                //if (sepetSil.Count > 0) {
                //    db.Sepets.RemoveRange(sepetSil);
                //    db.SaveChanges();
                //}


                Sepet sepet = new Sepet();

                decimal fiyat_ = Convert.ToDecimal(spFiyat);

                sepet.IlanGereksinimleri = ilanGereksinimleri;
                sepet.teslimatSuresi = Convert.ToInt32(teslimatSuresi);
                sepet.IlanSahibiId = ilanSahibiId;
                sepet.Fiyat = fiyat_;
                sepet.Adet = hdAdet;
                sepet.IlanId = ilan_Id;
                sepet.IlanAdi = ilanAdi;
                sepet.IlanAciklama = spAciklama; ;
                sepet.kullaniciFoto = spilanKullaniciFoto;
                sepet.ilanFotosu = spilanResim;




                if (User.Identity.IsAuthenticated)
                {
                    sepet.SiparisVerenId = KullaniciId;
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);


                    //HttpCookie anonimKullaniciSepetCookie = Request.Cookies["kullaniciSepet"];

                    //if (anonimKullaniciSepetCookie != null)
                    //{

                    //    sepet.KullaniciId = anonimKullaniciSepetCookie.Value;
                    //}
                    //else
                    //{
                    //    Guid gd = Guid.NewGuid();

                    //    HttpCookie newCookie = new HttpCookie("kullaniciSepet", gd.ToString());
                    //    newCookie.Expires = DateTime.Today.AddMonths(12);
                    //    Response.Cookies.Add(newCookie);

                    //    sepet.KullaniciId = gd.ToString();

                    //}


                }

                db.Sepets.Add(sepet);
                db.SaveChanges();



            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _BilgileriOzetle()
        {

            string isId = RouteData.Values["isId"].ToString();

            string uid = db.IsIlanlaris.Where(x => x.isId == isId).FirstOrDefault().userId;

            BilgeriDegistir mnu = new BilgeriDegistir();

            var getdata = db.Users.Find(uid);
            int konumInt = Convert.ToInt32(getdata.Konum);


            mnu.puan = getdata.puan_ortalamasi;
            mnu.Fotograf = getdata.Fotograf;
            //mnu.Telefon = getdata.Telefon;
            //mnu.EmailAdresi = getdata.Email;
            mnu.Hakkimda = getdata.Hakkimda;
            mnu.UserName = getdata.UserName;
            mnu.OrtalamaYanitSuresi = getdata.OrtalamaYanitSuresi;
            mnu.OncekiSiparisler = getdata.OncekiSiparisler;
            mnu.UyelikTarihi = getdata.UyelikTarihi;
            mnu.SonGorulmeTarihi = getdata.SonGorulmeTarihi;
            mnu.IptalEdilenler = getdata.IptalEdilenler;
            mnu.YorumSayisi = getdata.YorumSayisi;
            mnu.kullaniciId = getdata.Id;
            mnu.Telefon = getdata.Telefon;
            mnu.konum = db.iller.Where(x => x.il_id == konumInt).First().il_ad;

            return PartialView(mnu);
        }

        public ActionResult _SliderPartial()
        {
            string isId = RouteData.Values["isId"].ToString();

            var sql = db.Database.SqlQuery<Slider>("exec stpr_Slider @isId", new SqlParameter("isId", isId));
            return View(sql);
        }

        public ActionResult _YorumlarPartial()
        {
            string isId = RouteData.Values["isId"].ToString();

            var yorumlar = db.Yorumlars.Where(x => x.isId == isId && x.Onay == true).ToList();

            return PartialView(yorumlar);
        }

        public ActionResult _IlanOdemeyeGit()
        {
            string isId = RouteData.Values["isId"].ToString();

            var ilanlar = db.Database.SqlQuery<IsIlanlariListele_IlanDetay>("exec [stpr_UrunDetay]  @isId", new SqlParameter("isId", isId)).FirstOrDefault();

            return PartialView(ilanlar);
        }

        [ValidateInput(false)]
        public ActionResult MesajGonder(string toKullaniciAdi, string receiverId, string isIlanId)
        {
            string fromKullaniciAdi = User.Identity.GetUserName();

            var profil_foto = db.Users.Where(x => x.UserName == fromKullaniciAdi).Select(x => x.Fotograf).FirstOrDefault();

            Message msg = new Message();

            msg.Content = "Sohbet Başlatıldı";
            msg.tarih = DateTime.Now.ToShortDateString();
            msg.fromUser = fromKullaniciAdi;
            msg.toUser = toKullaniciAdi;
            msg.senderId = User.Identity.GetUserId();
            msg.receiverId = receiverId;
            msg.saat = DateTime.Now.ToShortTimeString();
            msg.profil_fotosu = profil_foto;
            msg.ReadOn = DateTime.Now;
            db.Messages.Add(msg);
            db.SaveChanges();

            return Redirect("/MusteriPaneli/Chat/");
        }

        public ActionResult _Sepet()
        {
            string KullaniciId = User.Identity.GetUserId();
            SepetCount sptC = new SepetCount();
            sptC.Count = db.Sepets.Where(x => x.IlanSahibiId == KullaniciId).Count();
            return PartialView(sptC);

        }
    }
}