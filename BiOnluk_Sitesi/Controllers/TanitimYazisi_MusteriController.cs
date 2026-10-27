using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace HemenTanitim.Controllers
{
    [Authorize]
    public class TanitimYazisi_MusteriController : Controller
    {
       //
        // GET: /Siteler_Musteri/
        private HemenTanitimContext db = new HemenTanitimContext();
       
        //
        // GET: /TanitimYazisi_Musteri/
        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult Edit(string aid)
        {
            string uid = User.Identity.GetUserId();

            Urunler urunler = db.Urunlers.Single(x => x.Guid == aid);

            //var sql = db.Database.SqlQuery<MusteriSiteleriSiteIdSiteUrl>("exec [stpr_SatisOlmayanSiteler] @uyeId", new SqlParameter("uyeId", uid)).ToList();

            ViewBag.MusteriSiteleri = new SelectList(db.MusteriSiteleris.Where(x => x.siteId == urunler.siteId), "siteId", "siteURL", urunler.siteId);

            return View(urunler);
        }

        //
        // POST: /MusteriSiteleris/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Urunler urunler, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
               
                Guid siteId = new Guid(frm["siteId"].ToString());

                var anaKategoriIdveAltKategoriId = db.MusteriSiteleris.Where(x => x.siteId == siteId).FirstOrDefault();

                urunler.siteId = siteId;
                urunler.AnaKategoriId = anaKategoriIdveAltKategoriId.AnaKategoriId;
                urunler.AltKategoriId = anaKategoriIdveAltKategoriId.AltKategoriId;

                db.Entry(urunler).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            return View(urunler);
        }


        public ActionResult Create(Urunler urunler)
        {
            string uid = User.Identity.GetUserId();

            ViewBag.MusteriSiteleri = new SelectList(db.MusteriSiteleris.Where(g => g.uyeId == uid), "siteId", "siteURL");
            

            return View();
        }

        // POST: /UrunIslemleri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Urunler urunler, FormCollection frm)
        {
            if (siteKontrol(urunler.siteId) == false) return View(urunler);

            string uid = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
              
                Guid siteId = new Guid(frm["siteId"].ToString());

                var anaKategoriIdveAltKategoriId = db.MusteriSiteleris.Where(x => x.siteId == siteId).FirstOrDefault();

                urunler.AnaKategoriId  = anaKategoriIdveAltKategoriId.AnaKategoriId;
                urunler.AltKategoriId = anaKategoriIdveAltKategoriId.AltKategoriId;
                urunler.siteUrl = anaKategoriIdveAltKategoriId.siteUrl;
                //urunler.Fotolar = Convert.ToString(collection["Fotolar"]);
                urunler.yayinSuresi = frm["yayinSuresi"].ToString();
                urunler.Guid = Guid.NewGuid().ToString();
                urunler.UyeId = uid;
                urunler.Tarih = DateTime.Now; 

                db.Urunlers.Add(urunler);

                db.SaveChanges();

                return RedirectToAction("Create");

            }

            ViewBag.MusteriSiteleri = new SelectList(db.MusteriSiteleris.Where(g => g.uyeId == uid), "siteId", "siteURL");

            return View(urunler);
        }


        public ActionResult Delete(string aid)
        {
            string uid = User.Identity.GetUserId();

            Urunler urunler = db.Urunlers.Single(x => x.Guid == aid);

            ViewBag.MusteriSiteleri = new SelectList(db.MusteriSiteleris.Where(g => g.uyeId == uid), "siteId", "siteURL");

            return View(urunler);
        }

        //
        // POST: /MusteriSiteleris/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string aid)
        {
            Urunler urunler = db.Urunlers.Single(x => x.Guid == aid);
            urunler.kaldir = true;
            db.SaveChanges();

            return RedirectToAction("Create");
        }
        public ActionResult _SatisSiteleri()
        {
            string uid = User.Identity.GetUserId();

            List<Urunler> ftr = new List<Urunler>();

            var urunler = (from item in db.Urunlers
                           where item.UyeId == uid && item.kaldir == false
                           select new
                           {
                               tar = item.Tarih,
                               guid = item.Guid,
                               urunadi = item.UrunAdi,
                               onay_ = item.Onay

                           }).ToList().Select(c => new Urunler
                           {
                               Tarih = c.tar,
                               Onay = c.onay_,
                               Guid = c.guid,
                               UrunAdi = c.urunadi
                           });

            ftr = urunler.ToList();

            return PartialView(urunler);
        }
        public bool siteKontrol(Guid siteId)
        {
            string uid = User.Identity.GetUserId();

            var siteKontrol = db.Urunlers.Where(x => x.siteId == siteId && x.kaldir == false).FirstOrDefault();

            if (siteKontrol != null)
            {

                ViewBag.Uyari = "<span class =\"alert alert-danger\"> Aynı siteyi tekrar satışa alamassınız.</span> <br /><br />";

                ViewBag.MusteriSiteleri = new SelectList(db.MusteriSiteleris.Where(g => g.uyeId == uid), "siteId", "siteURL");

                return false;
            }
            else
                return true;
        }
    }
}