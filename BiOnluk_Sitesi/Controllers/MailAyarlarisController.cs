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
    public class MailAyarlarisController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /MailAyarlaris/

        public ViewResult Index()
        {
            return View(context.MailAyarlaris.Take(1).Single());
        }

        //
        // GET: /MailAyarlaris/Details/5

        public ViewResult Details(int aid)
        {
            MailAyarlari mailayarlari = context.MailAyarlaris.Single(x => x.id == aid);
            return View(mailayarlari);
        }

        //
        // GET: /MailAyarlaris/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /MailAyarlaris/Create

        [HttpPost]
        public ActionResult Create(MailAyarlari mailayarlari)
        {
            if (ModelState.IsValid)
            {
                context.MailAyarlaris.Add(mailayarlari);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(mailayarlari);
        }
        
        //
        // GET: /MailAyarlaris/Edit/5
 
        public ActionResult Edit(int aid)
        {
            MailAyarlari mailayarlari = context.MailAyarlaris.Single(x => x.id == aid);
            return View(mailayarlari);
        }

        //
        // POST: /MailAyarlaris/Edit/5

        [HttpPost]
        public ActionResult Edit(MailAyarlari mailayarlari)
        {
            if (ModelState.IsValid)
            {
                context.Entry(mailayarlari).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mailayarlari);
        }

        //
        // GET: /MailAyarlaris/Delete/5
 
        public ActionResult Delete(int aid)
        {
            MailAyarlari mailayarlari = context.MailAyarlaris.Single(x => x.id == aid);
            return View(mailayarlari);
        }

        //
        // POST: /MailAyarlaris/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            MailAyarlari mailayarlari = context.MailAyarlaris.Single(x => x.id == aid);
            context.MailAyarlaris.Remove(mailayarlari);
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