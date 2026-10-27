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
    public class MusteriSitelerisController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /MusteriSiteleris/

        public ViewResult Index()
        {
            return View(context.MusteriSiteleris.ToList());
        }

        //
        // GET: /MusteriSiteleris/Details/5

        public ViewResult Details(Guid aid)
        {
            MusteriSiteleri musterisiteleri = context.MusteriSiteleris.Single(x => x.siteId == aid);
            return View(musterisiteleri);
        }

        //
        // GET: /MusteriSiteleris/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /MusteriSiteleris/Create

        [HttpPost]
        public ActionResult Create(MusteriSiteleri musterisiteleri)
        {
            if (ModelState.IsValid)
            {
                context.MusteriSiteleris.Add(musterisiteleri);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(musterisiteleri);
        }
        
        //
        // GET: /MusteriSiteleris/Edit/5

        public ActionResult Edit(Guid aid)
        {
            MusteriSiteleri musterisiteleri = context.MusteriSiteleris.Single(x => x.siteId == aid);
            return View(musterisiteleri);
        }

        //
        // POST: /MusteriSiteleris/Edit/5

        [HttpPost]
        public ActionResult Edit(MusteriSiteleri musterisiteleri)
        {
            if (ModelState.IsValid)
            {
                context.Entry(musterisiteleri).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(musterisiteleri);
        }

        //
        // GET: /MusteriSiteleris/Delete/5

        public ActionResult Delete(Guid aid)
        {
            MusteriSiteleri musterisiteleri = context.MusteriSiteleris.Single(x => x.siteId == aid);
            return View(musterisiteleri);
        }

        //
        // POST: /MusteriSiteleris/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid aid)
        {
            MusteriSiteleri musterisiteleri = context.MusteriSiteleris.Single(x => x.siteId == aid);
            context.MusteriSiteleris.Remove(musterisiteleri);
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