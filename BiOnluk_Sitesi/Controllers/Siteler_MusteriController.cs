using HemenTanitim.Models;
using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace HemenTanitim.Controllers
{
    [Authorize]
    public class Siteler_MusteriController : Controller
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

        public ActionResult Edit(Guid? aid)
        {
            string uid = User.Identity.GetUserId();

            MusteriSiteleri musterisiteleri = db.MusteriSiteleris.Single(x => x.siteId == aid);

            ViewBag.MusteriSiteleri = db.MusteriSiteleris.Where(g => g.uyeId == uid).ToList();

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", musterisiteleri.AnaKategoriId);

            ViewBag.AltKategoriId_ = new SelectList(db.AltKategoriEkles.Where(x => x.AnaKategoriId == musterisiteleri.AnaKategoriId), "Id", "KategoriAdi", musterisiteleri.AltKategoriId);

            return View(musterisiteleri);
        }

        //
        // POST: /MusteriSiteleris/Edit/5

        [HttpPost]
        public ActionResult Edit(MusteriSiteleri siteler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(siteler).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            return View(siteler);
        }


        public ActionResult Create(MusteriSiteleri siteler)
        {
            string uid = User.Identity.GetUserId();

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");           

            ViewBag.MusteriSiteleri = db.MusteriSiteleris.Where(g => g.uyeId == uid).ToList();

            return View();
        }

        // POST: /UrunIslemleri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(MusteriSiteleri siteler, FormCollection frm)
        {
            string uid = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                //urunler.Fotolar = Convert.ToString(collection["Fotolar"]);

                siteler.siteId = Guid.NewGuid();

                siteler.uyeId = uid;

                siteler.eklenmeTarihi = DateTime.Now; 

                db.MusteriSiteleris.Add(siteler);
                db.SaveChanges();

                return RedirectToAction("Create");

            }

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");

            ViewBag.MusteriSiteleri = db.MusteriSiteleris.Where(g => g.uyeId == uid).ToList();

            return View(siteler);
        }


        public ActionResult Delete(Guid aid)
        {
            string uid = User.Identity.GetUserId();

            MusteriSiteleri musterisiteleri = db.MusteriSiteleris.Single(x => x.siteId == aid);

            ViewBag.MusteriSiteleri = db.MusteriSiteleris.Where(g => g.uyeId == uid ).ToList();

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", musterisiteleri.AnaKategoriId);

            return View(musterisiteleri);
        }

        //
        // POST: /MusteriSiteleris/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid aid)
        {
            MusteriSiteleri musterisiteleri = db.MusteriSiteleris.Single(x => x.siteId == aid);
            db.MusteriSiteleris.Remove(musterisiteleri);
            db.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}