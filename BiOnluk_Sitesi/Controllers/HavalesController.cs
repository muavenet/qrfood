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
    public class HavalesController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /Havales/

        public ViewResult Index()
        {
            return View(context.Havales.ToList());
        }

        //
        // GET: /Havales/Details/5

        public ViewResult Details(int aid)
        {
            Havale havale = context.Havales.Single(x => x.id == aid);
            return View(havale);
        }

        //
        // GET: /Havales/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Havales/Create

        [HttpPost]
        public ActionResult Create(Havale havale)
        {
            if (ModelState.IsValid)
            {
                context.Havales.Add(havale);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(havale);
        }
        
        //
        // GET: /Havales/Edit/5
 
        public ActionResult Edit(int aid)
        {
            Havale havale = context.Havales.Single(x => x.id == aid);
            return View(havale);
        }

        //
        // POST: /Havales/Edit/5

        [HttpPost]
        public ActionResult Edit(Havale havale)
        {
            if (ModelState.IsValid)
            {
                context.Entry(havale).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(havale);
        }

        //
        // GET: /Havales/Delete/5
 
        public ActionResult Delete(int aid)
        {
            Havale havale = context.Havales.Single(x => x.id == aid);
            return View(havale);
        }

        //
        // POST: /Havales/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            Havale havale = context.Havales.Single(x => x.id == aid);
            context.Havales.Remove(havale);
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