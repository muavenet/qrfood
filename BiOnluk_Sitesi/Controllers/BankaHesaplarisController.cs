using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;
using HemenTanitim.Models;

namespace HemenTanitim.Controllers
{   
    public class BankaHesaplarisController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /BankaHesaplaris/

        public ViewResult Index()
        {
            return View(context.BankaHesaplaris.ToList());
        }

        //
        // GET: /BankaHesaplaris/Details/5

        public ViewResult Details(int aid)
        {
            BankaHesaplari bankahesaplari = context.BankaHesaplaris.Single(x => x.BId == aid);
            return View(bankahesaplari);
        }

        //
        // GET: /BankaHesaplaris/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /BankaHesaplaris/Create

        [HttpPost]
        public ActionResult Create(BankaHesaplari bankahesaplari)
        {
            if (ModelState.IsValid)
            {
                context.BankaHesaplaris.Add(bankahesaplari);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(bankahesaplari);
        }
        
        //
        // GET: /BankaHesaplaris/Edit/5
 
        public ActionResult Edit(int aid)
        {
            BankaHesaplari bankahesaplari = context.BankaHesaplaris.Single(x => x.BId == aid);
            return View(bankahesaplari);
        }

        //
        // POST: /BankaHesaplaris/Edit/5

        [HttpPost]
        public ActionResult Edit(BankaHesaplari bankahesaplari)
        {
            if (ModelState.IsValid)
            {
                context.Entry(bankahesaplari).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankahesaplari);
        }

        //
        // GET: /BankaHesaplaris/Delete/5
 
        public ActionResult Delete(int aid)
        {
            BankaHesaplari bankahesaplari = context.BankaHesaplaris.Single(x => x.BId == aid);
            return View(bankahesaplari);
        }

        //
        // POST: /BankaHesaplaris/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            BankaHesaplari bankahesaplari = context.BankaHesaplaris.Single(x => x.BId == aid);
            context.BankaHesaplaris.Remove(bankahesaplari);
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