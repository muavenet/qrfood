using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Iyzico.MVC.Helpers;
using System.IO;
using System.Data.Entity;
using System.Threading.Tasks;
using HemenTanitim.Helpers;

namespace HemenTanitim.Controllers
{
    [CustomAuthorize]
    public class SepetController : Controller
    {
        Utils ut = new Utils();
        HemenTanitimContext dbEntites = new HemenTanitimContext();
        IyzicoHelper iyzicoHelper = new IyzicoHelper();
        //
        // GET: /Sepet/
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            ViewBag.phoneControl = dbEntites.Users.Where(x => x.Id == userId).FirstOrDefault().PhoneNumber;

            FetchYapilacaklar();

            return View();
        }

        public JsonResult GetProducts()
        {
            return Json(FetchYapilacaklar(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult geturunDetay(string id)
        {
            return Json(urunDetayGetir(id), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult addUrun()
        //{
        //    return Json(UrunEkle(), JsonRequestBehavior.AllowGet);
        //}
        public JsonResult removeSepets(int id)
        {
            return Json(SilSepet(id), JsonRequestBehavior.AllowGet);
        }

        public List<Sepet> urunDetayGetir(string isId)
        {
            var data = (from item in dbEntites.IsIlanlaris where item.isId == isId
                        select new
                        {
                            ilanId = item.isId,                           
                            ilanAdi = item.baslik,
                            fiyatMin = item.fiyat,
                            aySayisi = item.kacKisilik,
                            ilanAciklama = item.IsIlanAciklamasi,
                            teslimat = item.aracHacmi

                        }).ToList().Select(c => new Sepet
                        {
                            IlanId = c.ilanId,
                            IlanAdi = c.ilanAdi,
                            Fiyat = Convert.ToDecimal(c.fiyatMin),
                            IlanAciklama = c.ilanAciklama,
                            teslimatSuresi = c.teslimat
                        });

            return data.ToList();
        }

        public JsonResult updateSepet(int id,int adet )
        {
            Sepet sp = dbEntites.Sepets.Find(id);

            sp.Adet = adet;

            dbEntites.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            dbEntites.SaveChanges();

            return Json(id, JsonRequestBehavior.AllowGet);
        }


        public JsonResult updateSepetNot(int id, string not)
        {
            Sepet sp = dbEntites.Sepets.Find(id);

            sp.Not = not;

            dbEntites.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            dbEntites.SaveChanges();

            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public bool SilSepet(int silinecekKayit)
        {
            Sepet sepet = dbEntites.Sepets.Find(silinecekKayit);
            dbEntites.Sepets.Remove(sepet);
            dbEntites.SaveChanges();
            return true;
        }

        public List<Sepet> FetchYapilacaklar()
        {
            string uid =  User.Identity.GetUserId();

            var data = (from item in dbEntites.Sepets
                        where item.SiparisVerenId == uid && item.SiparisEdildiMi == false
                        select new
                        {
                            id = item.Id,
                            ilanId = item.IlanId,
                            ilanAdi = item.IlanAdi,
                            fiyat = item.Fiyat,
                            aciklama = item.IlanAciklama,
                            teslimat = item.teslimatSuresi,
                            kullaniciId = item.SiparisVerenId,
                            urunSahibiId = item.IlanSahibiId,
                            ilanFoto = item.ilanFotosu,
                            kfoto = item.kullaniciFoto,
                            Adet = item.Adet,
                            Not = item.Not

                        }).ToList().Select(c => new Sepet
                        {

                            // ADET GELMİYOR
                            Id = c.id,
                            IlanId = c.ilanId,
                            IlanAdi = c.ilanAdi,
                            Fiyat = c.fiyat,
                            IlanAciklama = c.aciklama,
                            teslimatSuresi = c.teslimat,
                            SiparisVerenId = c.kullaniciId,
                            IlanSahibiId = c.urunSahibiId,
                            ilanFotosu = c.ilanFoto,
                            kullaniciFoto = c.kfoto,
                            Adet = c.Adet,
                            Not = c.Not


                        });

            return data.ToList();
        }

        //[HttpPost]
        //[ValidateInput(false)]
        public async Task<JsonResult> SiparisGir(/*FormCollection frm*/int id, int adet)
        {
            string newguid = ut.GenerateSiparisId(6);

            string uid = User.Identity.GetUserId();

            List<Sepet> sepetim = dbEntites.Sepets.Where(o => o.SiparisVerenId == uid).ToList();

            var UrunTutari = sepetim.Sum(x => x.Fiyat * x.Adet);        
        
            //Siparis veren bilgilerini aldık
            var SiparisVeren = dbEntites.Users.Find(uid);
            //("##0.00") ile iyzico ödeme tutarı yazımına uygun  
            string toplaSon = UrunTutari.ToString("##0.00").Replace(",", ".");
            //Sepetteki ürünleri getirdik

            //Ödeme formunu başlatma
            //Sepeti gönderirken liste olarak gönderiyoruz            
            string[] sonuc = iyzicoHelper.Should_Create_Payment(toplaSon, SiparisVeren, sepetim, newguid);
            //Ödeme formunu başlatma

            sepetim.ForEach(x => x.token = sonuc[1]);
            await dbEntites.SaveChangesAsync();

            //Geriye dönen değerler (NEREYE GİTTİĞİNİ BİLMİYORUM SANIRIM BOŞTA)
            var result = new { CheckoutFormContent = sonuc[0], token = sonuc[1], ConversationId = sonuc[2], siparisId = newguid };

            return Json(result);

        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Ilan_Vermek_Icın_Eksikleri_Gir(FormCollection frm)
        {
            string userId = User.Identity.GetUserId();

            ApplicationUser user = dbEntites.Users.Where(x => x.Id == userId).FirstOrDefault();

            user.PhoneNumber = frm["exampleInputTelefon"];
            dbEntites.Entry(user).State = System.Data.Entity.EntityState.Modified;
         
            try
            {
                dbEntites.SaveChanges();
                return Json("success");
            }
            catch (Exception ex)
            {

                return Json(ex);
            }
        }


    }
}