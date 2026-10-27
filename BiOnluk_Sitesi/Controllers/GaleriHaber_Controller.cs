using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class GaleriHaber_Controller : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();
        // GET: GaleriHaber
        public ActionResult Index()
        {
            return View(context.GaleriBasliks.ToList());
        }

        // GET: GaleriHaber/Details/5
   
        // GET: GaleriHaber/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GaleriHaber/Create
        [HttpPost]
        public ActionResult Create(GaleriBaslik baslik)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.GaleriBasliks.Add(baslik);
                    context.SaveChanges();
                
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(baslik);
            }
        }

        // GET: GaleriHaber/Edit/5
        public ActionResult Edit(int aid)
        {
            GaleriBaslik gBaslik = context.GaleriBasliks.Single(x => x.Id == aid);
            return View(gBaslik);
        }

        // POST: GaleriHaber/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, GaleriBaslik baslik)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Entry(baslik).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                  
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(baslik);
            }
        }

    
       

        // POST: GaleriHaber/Delete/5
        [HttpPost]
        public ActionResult Delete(int aid)
        {
            try
            {
                GaleriBaslik gBaslik = context.GaleriBasliks.Single(x => x.Id == aid);
                context.GaleriBasliks.Remove(gBaslik);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
