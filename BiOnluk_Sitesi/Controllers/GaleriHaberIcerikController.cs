using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class GaleriHaberIcerikController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();
        // GET: GaleriHaberIcerik
        public ActionResult Index(int GaleriBasliaidkId)
        {
            List<GaleriIcerikHaberi> ss = context.GaleriIcerikHaberis.Where(x => x.GaleriId == GaleriBasliaidkId).ToList();
            try
            {
                if (ss.Count > 0)
                    return View(ss);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View();
        }

        // GET: GaleriHaberIcerik/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GaleriHaberIcerik/Create
        public ActionResult Create(int GaleriId)
        {
            return View();
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        // POST: GaleriHaberIcerik/Create
        [HttpPost]
        public ActionResult Create(GaleriIcerikHaberi gicerik)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.GaleriIcerikHaberis.Add(gicerik);
                    context.SaveChanges();

                }
                return RedirectToAction("Index", new { GaleriBasliaidkId = gicerik.GaleriId } );
            }
            catch
            {
                return View(gicerik);
            }
        }

        // GET: GaleriHaberIcerik/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GaleriHaberIcerik/Edit/5
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

        // GET: GaleriHaberIcerik/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GaleriHaberIcerik/Delete/5
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
