using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminIlanYonetimiController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();

        public ActionResult Index(string IlanTuru)
        {

            Dictionary<bool, string> d = new Dictionary<bool, string>();
            d.Add(false, "Onay Bekliyor");
            d.Add(true, "Onaylandı");

            ViewBag.Teslim = new SelectList(d, "Key", "Value");

            if (string.IsNullOrEmpty(IlanTuru))
            {
                var dd = db.IsIlanlaris.Where(x => x.adminOnay == false).OrderByDescending(x => x.Tarih).ToList();

                ViewBag.SiparisTuru = "Onay Bekliyor";

                return View(dd);
            }
            else if (IlanTuru == "Onaylandı")
            {
                var dd = db.IsIlanlaris.Where(x => x.adminOnay == true).OrderByDescending(x => x.Tarih).ToList();

                ViewBag.SiparisTuru = "Onaylandı";

                return View(dd);
            }
            else if (IlanTuru == "Onay Bekliyor")
            {
                var dd = db.IsIlanlaris.Where(x => x.adminOnay == false).OrderByDescending(x => x.Tarih).ToList();

                ViewBag.SiparisTuru = "Onay Bekliyor";

                return View(dd);
            }
            else
            {
                var dd = db.IsIlanlaris.OrderByDescending(x => x.Tarih).ToList();

                ViewBag.SiparisTuru = "Tüm İlanlar";

                return View(dd);
            }


        }

        public ActionResult OnayDurumuDegistir(bool selectedValue, string isId)
        {
            IsIlanlari sp = db.IsIlanlaris.Find(isId);

            sp.adminOnay = (bool)selectedValue;

            db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            //if (selectedValue == "Teslim edildi")
            //{
            //    Urunler urn = db.Urunlers.Find(sp.UrunId);

            //    db.Entry(urn).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}


            return Json(true);
        }

        public ActionResult Detay(string isId)
        {
            if (isId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IsIlanlari ilanlar = db.IsIlanlaris.Find(isId);

            if (ilanlar == null)
            {
                return HttpNotFound();
            }

            return View(ilanlar);
        }

        public ActionResult _Medyalar(string isIlanId)
        {
            var url = isIlanId;

            List<IsIlanlariGaleri> Videolar = db.IsIlanlariGaleris.Where(x => x.IsIlanId == url ).ToList<IsIlanlariGaleri>();

            return PartialView(Videolar);
        }
    }
}