using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class CVController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();
        // GET: CV

            //[Authorize]
        public ActionResult Kullanici_Ozgecmisi(string uid)
        {
            //string uid = RouteData.Values["id"].ToString();

            BilgeriDegistir mnu = new BilgeriDegistir();

            var getdata = db.Users.Find(uid);

            mnu.ad = getdata.ad;
            mnu.soyad = getdata.soyad;
            mnu.Fotograf = getdata.Fotograf;
            mnu.EmailAdresi = getdata.Email;
            mnu.Hakkimda = getdata.Hakkimda;
            mnu.UserName = getdata.UserName;
            mnu.OncekiSiparisler = getdata.OncekiSiparisler;
            mnu.UyelikTarihi = getdata.UyelikTarihi;
            mnu.SonGorulmeTarihi = getdata.SonGorulmeTarihi;
            mnu.puan = getdata.puan_ortalamasi;
            mnu.YorumSayisi = getdata.YorumSayisi;
       
            mnu.Yorumlars = db.Yorumlars.Where(z => z.kullaniciId == getdata.Id).Take(5).ToList();
            mnu.Portfolyos = db.Portfolyos.Where(z => z.userId == getdata.Id).OrderByDescending(x => x.eklenmeTarihi).Take(15).ToList();
            return View(mnu);
        }
    }
}