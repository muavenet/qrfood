using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using Iyzico.MVC.Helpers;
using System.Threading.Tasks;
using System.Web.Routing;
using Iyzipay.Request;
using Iyzipay.Model;
using System.Data.SqlClient;

using System.Configuration;

namespace HemenTanitim.Controllers
{
    [Authorize]
    public class SiparisiTamamlaController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();
        Utils ut = new Utils();
        IyzicoHelper iyzicoHelper = new IyzicoHelper();
       

        //
        // GET: /SiparisiTamamla/
        public ActionResult Index()
        {

            //ViewBag.iller = new SelectList(db.iller.ToList(), "il_id", "il_ad").ToList();

            //ViewBag.KargoSirketleri = db.KargoSirketleris.ToList();

            ViewBag.Havale = db.Havales.ToList();

            string uid = User.Identity.GetUserId();

            ViewBag.UrunTutari = db.Sepets.Where(x => x.SiparisVerenId == uid).Sum(x => x.Fiyat * x.Adet).ToString("##0.00").Replace(",", ".");
            
            return View();

        }

        readonly string _connString = ConfigurationManager.ConnectionStrings["DB150408011842Entities"].ConnectionString;
        public JsonResult GetSiparisler(string SiparisId)
        {
            var durum = 0;
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(@"Select [Durumu] from dbo.SiparisBilgileri WHERE [SiparisId] = '" + SiparisId + "'", connection))

                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);

                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        durum = (byte)reader["Durumu"];
                    }
                }

                return Json(durum, JsonRequestBehavior.AllowGet);
            }
        }



        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SiparisOnayla.Show();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<JsonResult> SiparisGir(FormCollection frm)
        {
            string siparisId = frm["siparisId"].ToString();

            SiparisBilgileri spr = db.SiparisBilgileris.Where(z => z.SiparisId == siparisId).FirstOrDefault();

            if (Request.Files["yuklenendosya"].ContentLength > 0)
            {
                HttpPostedFileBase file = Request.Files["yuklenendosya"];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var uploadUrl = Server.MapPath("~/Content/Dosyalar");
                file.SaveAs(Path.Combine(uploadUrl, fileName));
            }

            spr.SiparisDurumu = 0;
            db.Entry(spr).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(frm["TanitimIcerik"].ToString());

        }

       
        //[HttpPost]
        //[ValidateInput(false)]
        //public async Task<JsonResult> SiparisTeslimEdildi(FormCollection frm)
        //{
        //     string siparisId = frm["siparisId"].ToString();

        //    SiparisBilgileri spr = db.SiparisBilgileris.Where(z => z.SiparisId == siparisId).FirstOrDefault();

        //    ApplicationUser user_bilgileri = db.Users.Where(x => x.Id == spr.hotelId).FirstOrDefault();

        //    user_bilgileri = db.Users.Where(x => x.Id == spr.SiparisVerenId).FirstOrDefault();

        //    Yorumlar yrm = new Yorumlar();
        //    yrm.Aciklama = frm["YorumIcerik"].ToString();
        //    yrm.GirisTarihi = DateTime.Now;
        //    yrm.Onay = false;
        //    yrm.puan = Convert.ToInt16(frm["rating_value"]);
        //    yrm.KullaniciFoto = user_bilgileri.Fotograf;
        //    yrm.kullaniciAdi = user_bilgileri.UserName;
        //    yrm.kullaniciId = user_bilgileri.Id;
        //    yrm.isId = frm["siparisId"].ToString();

        //    db.Entry(yrm).State = EntityState.Added;
        //    await db.SaveChangesAsync();

        //    //iyzicoHelper.Should_Create_Marketplace_Payment_Onay(spr.conversationId);

        //    return Json(""/*frm["YorumIcerik"].ToString()*/);

        //}

        public ActionResult siparisOnayla(string siparisId_)
        {
            SiparisBilgileri sp = db.SiparisBilgileris.Where(x => x.SiparisId == siparisId_).FirstOrDefault();

            return View(sp);
        }

        [AllowAnonymous]
        public ActionResult sprRedirect(string siparisId_)
        {
            return RedirectToAction("OdemeSayfasi", "SiparisiTamamla", new { siparisId = siparisId_, Havale = false });
        }

        public async Task<ActionResult> OdemeSayfasi(string siparisId, bool? Havale)
        {      

            string id = User.Identity.GetUserId();

            var user_info = db.Users.Where(x => x.Id == id).FirstOrDefault();

            List<Sepet> sepetim = db.Sepets.Where(o => o.SiparisVerenId == user_info.Id).ToList();

            var sepetToken = sepetim.First().token;

            var UrunTutari = sepetim.Sum(x => x.Fiyat * x.Adet);

            SiparisBilgileri spr = new SiparisBilgileri();

            if (Havale == false)
            {

                string gd = Guid.NewGuid().ToString();

                if (iyzicoHelper.Should_Retrieve_Checkout_Form_Result(gd, sepetToken)) //metodu çağırdık, eğer metod geriye true döndüyse ödeme alındı
                {
                    foreach (var item in sepetim)
                    {
                        spr.IlanAciklama = spr.IlanAciklama + "<br /> <strong> Product :" + item.IlanAdi + "</strong><br />" +
                            "Quantity : " + item.Adet + "<br />" + "Note : " + item.Not + " " + "<br />";
                    }

                    spr.SiparisDurumu = 1;
                    spr.token = sepetToken;
                    spr.SiparisVerenId = user_info.Id;
                    spr.Tutar = UrunTutari;
                    spr.SiparisTarihi = DateTime.Today.ToString("yyyy-MM-dd");
                    spr.SiparisId = gd;
                    spr.hotelId = user_info.hotelId;
                    spr.odaNo = user_info.odaNo;
                    spr.Durumu = Convert.ToByte((byte)Utils.SiparisDurumu.Hazirlaniyor);
                    spr.OdemeTipi = Convert.ToByte((byte)Utils.OdemeTipi.Kredi_Karti);

                    db.SiparisBilgileris.Add(spr);
                    await db.SaveChangesAsync();

                    db.Sepets.RemoveRange(sepetim);
                    await db.SaveChangesAsync();

                    return RedirectToAction("siparisOnayla", "SiparisiTamamla", new { siparisId_ = gd });
                }
                else
                {
                    //ödeme alınamadı, gelen hatayı ekrana yazdıralım

                    string error = "";

                    SiparisBilgileri spr1 = db.SiparisBilgileris.Where(x => x.SiparisId == siparisId).FirstOrDefault();

                    //spr.OdemeTipi = "Hatalı ödeme";
                    spr1.IlanAciklama = "Hata : " + error;
                    spr1.SiparisDurumu = 0;

                    db.Entry(spr1).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("IyzicoError", new { err = error });
                }
            }
            else {

                foreach (var item in sepetim)
                {
                    spr.IlanAciklama = spr.IlanAciklama + "<br /> <strong> Product :" + item.IlanAdi + "</strong><br />" + "Quantity : " + item.Adet + "<br />" + "Note : " + item.Not + " " + "<br />";
                }

                spr.SiparisId = Guid.NewGuid().ToString();
                spr.SiparisDurumu = 1;
                spr.token = sepetToken;
                spr.SiparisVerenId = user_info.Id;
                spr.Tutar = UrunTutari;
                spr.SiparisTarihi = DateTime.Today.ToString("yyyy-MM-dd");
                spr.hotelId = user_info.hotelId;
                spr.odaNo = user_info.odaNo;
                spr.Durumu = Convert.ToByte((byte)Utils.SiparisDurumu.Hazirlaniyor);
                spr.OdemeTipi = Convert.ToByte((byte)Utils.OdemeTipi.Kapida_Odeme);

                db.SiparisBilgileris.Add(spr);
                await db.SaveChangesAsync();

                db.Sepets.RemoveRange(sepetim);
                await db.SaveChangesAsync();


                return Json(Url.Action("siparisOnayla", "SiparisiTamamla", new { siparisId_ = spr.SiparisId }));

            }
          

        }

        public ActionResult OdemeSonucSayfasi()
        {

            string uid = User.Identity.GetUserId();

            IEnumerable<Sepet> myEntities = db.Sepets.Where(e => e.SiparisVerenId == uid);

            db.Sepets.RemoveRange(myEntities);
            db.SaveChanges();

            return View();
        }

        public ActionResult HesapOzeti(int SiparisId)
        {
            SiparisBilgileri sp = db.SiparisBilgileris.Find(SiparisId);

            return Json(sp);
        }

        public JsonResult FaturaBilgileriEkle(FormCollection frm)
        {
            Fatura ftr = new Fatura();

            ftr.adsirketUnvan = frm["FaturaadsirketUnvan"].ToString();
            ftr.SoyadUnvan = frm["FaturaSoyadUnvan"].ToString();
            ftr.Sehir = frm["FaturaIl"].ToString();
            ftr.Ilce = frm["FaturaIlceler"].ToString();
            ftr.Semt = frm["FaturaSemtler"].ToString();
            ftr.Mahalle = frm["FaturaMahalleler"].ToString();
            ftr.Adres = frm["FaturaAdres"].ToString();
            ftr.PostaKodu = frm["FaturaPostaKodu"].ToString();
            ftr.SabitTelefon = frm["FaturaSabTel"].ToString();
            ftr.CepTelefonu = frm["FaturaCepTel"].ToString();
            ftr.VergiDairesi = frm["FaturaVergiDairesi"].ToString();
            ftr.TCVergiNo = frm["FaturaVergiTcNo"].Length > 0 ? Convert.ToInt32(frm["FaturaVergiTcNo"]) : 0;
            ftr.UserId = User.Identity.GetUserId();

            if (TempData.ContainsKey("FaturaEklenmis"))
            {
                ftr.Id = Convert.ToInt32(TempData["FaturaEklenmis"]);
                db.Entry(ftr).State = EntityState.Modified;
            }
            else
            {
                db.Faturas.Add(ftr);
            }
            db.SaveChanges();

            TempData["FaturaEklenmis"] = ftr.Id;

            return Json(true);
        }

        public ActionResult IyzicoError(string err)
        {
            ViewBag.Error = err;

            return View();
        }



    }
}