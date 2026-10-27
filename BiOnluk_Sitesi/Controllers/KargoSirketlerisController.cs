using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KargoSirketlerisController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /KargoSirketleris/

        public ViewResult Index()
        {
            return View(context.KargoSirketleris.ToList());
        }

        //
        // GET: /KargoSirketleris/Details/5

        public ViewResult Details(int aid)
        {
            KargoSirketleri kargosirketleri = context.KargoSirketleris.Single(x => x.Id == aid);
            return View(kargosirketleri);
        }

        //
        // GET: /KargoSirketleris/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /KargoSirketleris/Create

        [HttpPost]
        public ActionResult Create(KargoSirketleri kargosirketleri)
        {
            if (ModelState.IsValid)
            {
                context.KargoSirketleris.Add(kargosirketleri);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(kargosirketleri);
        }
        
        //
        // GET: /KargoSirketleris/Edit/5
 
        public ActionResult Edit(int aid)
        {
            KargoSirketleri kargosirketleri = context.KargoSirketleris.Single(x => x.Id == aid);
            return View(kargosirketleri);
        }

        //
        // POST: /KargoSirketleris/Edit/5

        [HttpPost]
        public ActionResult Edit(KargoSirketleri kargosirketleri)
        {
            if (ModelState.IsValid)
            {
                context.Entry(kargosirketleri).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kargosirketleri);
        }

        //
        // GET: /KargoSirketleris/Delete/5
 
        public ActionResult Delete(int aid)
        {
            KargoSirketleri kargosirketleri = context.KargoSirketleris.Single(x => x.Id == aid);
            return View(kargosirketleri);
        }

        //
        // POST: /KargoSirketleris/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            KargoSirketleri kargosirketleri = context.KargoSirketleris.Single(x => x.Id == aid);
            context.KargoSirketleris.Remove(kargosirketleri);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}