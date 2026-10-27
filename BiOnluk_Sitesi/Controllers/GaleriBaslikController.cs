using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class GaleriBaslikController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();
        // GET: GaleriBaslik
        public ActionResult Index()
        {
            return View(context.GaleriBasliks.ToList());
        }

        // GET: GaleriBaslik/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GaleriBaslik/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GaleriBaslik/Create
        [HttpPost]
        public ActionResult Create(GaleriBaslik collection)
        {
            try
            {
                context.GaleriBasliks.Add(collection);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GaleriBaslik/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GaleriBaslik/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GaleriBaslik/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GaleriBaslik/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
