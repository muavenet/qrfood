using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;
using System.IO;
using Newtonsoft.Json;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class YorumlarsController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();

        //
        // GET: /Yorumlars/

        public ViewResult Index(string yorumturu)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();

            d.Add("False", "Onaylanmadý");
            d.Add("True", "Onaylandý");

            ViewBag.Onay = new SelectList(d, "Key", "Value");

            if (string.IsNullOrEmpty(yorumturu))
            {
                var dd = db.Yorumlars.Where(x => x.Onay == false).ToList();

                ViewBag.YorumTuru = "Onay bekleyen yorumlar";

                return View(dd);
            }
            else if (yorumturu == "Onaylý")
            {
                var dd = db.Yorumlars.Where(x => x.Onay == true).ToList();

                ViewBag.YorumTuru = "Onaylý yorumlar";

                return View(dd);
            }
            else
            {
                ViewBag.YorumTuru = "Tüm yorumlar";

                return View(db.Yorumlars.ToList());
            }
        
        }

        //
        // GET: /Yorumlars/Details/5

        public ViewResult Details(int aid)
        {
            Yorumlar yorumlar = db.Yorumlars.Single(x => x.Id == aid);
            return View(yorumlar);
        }

        //
        // GET: /Yorumlars/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Yorumlars/Create

        [HttpPost]
        public ActionResult Create(Yorumlar yorumlar)
        {
            if (ModelState.IsValid)
            {
                db.Yorumlars.Add(yorumlar);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(yorumlar);
        }
        
        //
        // GET: /Yorumlars/Edit/5
 
        public ActionResult Edit(int aid)
        {
            Yorumlar yorumlar = db.Yorumlars.Single(x => x.Id == aid);
            return View(yorumlar);
        }

        //
        // POST: /Yorumlars/Edit/5

        [HttpPost]
        public ActionResult Edit(Yorumlar yorumlar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yorumlar).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yorumlar);
        }

        //
        // GET: /Yorumlars/Delete/5
 
        public ActionResult Delete(int aid)
        {
            Yorumlar yorumlar = db.Yorumlars.Single(x => x.Id == aid);
            return View(yorumlar);
        }

        //
        // POST: /Yorumlars/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            Yorumlar yorumlar = db.Yorumlars.Single(x => x.Id == aid);
            db.Yorumlars.Remove(yorumlar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult OnayDurumuDegistir(bool selectedValue, int Id)
        {
            Yorumlar sp = db.Yorumlars.Find(Id);

            sp.Onay = selectedValue;

            db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(true);
        }

       

       
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}