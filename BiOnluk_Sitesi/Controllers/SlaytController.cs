using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;
using System.IO;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SlaytController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();

        // GET: /Slayt/
        public ActionResult Index()
        {
            return View(db.Fotolars.Where(z => z.Slayt == true).ToList());
        }

        // GET: /Slayt/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fotolar fotolar = db.Fotolars.Find(id);
            if (fotolar == null)
            {
                return HttpNotFound();
            }
            return View(fotolar);
        }

        // GET: /Slayt/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Slayt/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResimYolu,Baslik1,Baslik2,Icerik,KategoriSlayt_h248,KategoriSlayt_h430,SlaytKucukAnasayfa1,SlaytKucukAnasayfa2,BiziTercihEdenFirmalar,link,AnasayfaEnUzunBanner")] Fotolar fotos, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                Guid gd = Guid.NewGuid();


                if (photo != null && photo.ContentLength > 0)
                {
                    string directory = Server.MapPath("~/Content/uploads/");

                    //if (photo.ContentLength > 10240)
                    //{
                    //    ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
                    //    return View();
                    //}

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = System.IO.Path.GetExtension(photo.FileName).Substring(1);

                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        return View();
                    }
                    photo.SaveAs(Path.Combine(directory, gd.ToString() + ".jpg"));

                    fotos.ResimYolu = gd.ToString() + ".jpg";

                    fotos.Slayt = true;
                }                
                else
                    fotos.ResimYolu = gd.ToString();

                db.Fotolars.Add(fotos);
                db.SaveChanges();

                return RedirectToAction("Index");
            }



            return View(fotos);
        }

        // GET: /Slayt/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fotolar fotos = db.Fotolars.Find(id);
            if (fotos == null)
            {
                return HttpNotFound();
            }
            return View(fotos);
        }

        // POST: /Slayt/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ResimYolu,Baslik1,Baslik2,Icerik,KategoriSlayt_h248,KategoriSlayt_h430,SlaytKucukAnasayfa1,SlaytKucukAnasayfa2,BiziTercihEdenFirmalar,link,AnasayfaEnUzunBanner")] Fotolar fotos, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                Guid gd = Guid.NewGuid();

                if (photo != null && photo.ContentLength > 0)
                {
                    string directory = Server.MapPath("~/Content/uploads/");

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = System.IO.Path.GetExtension(photo.FileName).Substring(1);

                    //if (!supportedTypes.Contains(fileExt))
                    //{
                    //    ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                    //    return View();
                    //}
                    photo.SaveAs(Path.Combine(directory, gd.ToString() + ".jpg"));

                    fotos.ResimYolu = gd.ToString() + ".jpg";
                }

                else
                    fotos.ResimYolu = fotos.ResimYolu;

                fotos.Slayt = true;

                db.Entry(fotos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fotos);
        }
      
        // GET: /Slayt/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fotolar fotos = db.Fotolars.Find(id);
            if (fotos == null)
            {
                return HttpNotFound();
            }
            return View(fotos);
        }

        // POST: /Slayt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fotolar fotos = db.Fotolars.Find(id);
            db.Fotolars.Remove(fotos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult _GetBiziTercihEdenFirmalar()
        {
            ViewBag.biziTercihEdenFirmalar = new SelectList(db.Fotolars.Where(x => x.BiziTercihEdenFirmalar == true), "Id", "ResimYolu");

            return PartialView();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
