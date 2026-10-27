using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnaKategoriEkleController : Controller
    {

        private HemenTanitimContext db = new HemenTanitimContext();

        // GET: /AnaKategoriEkle/
        public ActionResult Index()
        {
            return View(db.AnaKategorisEkles.ToList());
        }

        // GET: /AnaKategoriEkle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnaKategoriler anakategori = db.AnaKategorisEkles.Find(id);
            if (anakategori == null)
            {
                return HttpNotFound();
            }
            return View(anakategori);
        }

        // GET: /AnaKategoriEkle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /AnaKategoriEkle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,KategoriAdi,link")] AnaKategoriler anakategori, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {

                Guid gd = Guid.NewGuid();

                foto = Request.Files["foto"];

                if (foto != null && foto.ContentLength > 0)
                {
                    string directory = Server.MapPath("~/Content/uploads/");

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = System.IO.Path.GetExtension(foto.FileName).Substring(1);

                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        return View();
                    }
                    foto.SaveAs(Path.Combine(directory, gd.ToString() + ".jpg"));

                    anakategori.resim = gd.ToString() + ".jpg";

                }
                else
                    anakategori.resim = gd.ToString();

                db.AnaKategorisEkles.Add(anakategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anakategori);
        }

        // GET: /AnaKategoriEkle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnaKategoriler anakategori = db.AnaKategorisEkles.Find(id);
            if (anakategori == null)
            {
                return HttpNotFound();
            }
            return View(anakategori);
        }

        // POST: /AnaKategoriEkle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,KategoriAdi,link")] AnaKategoriler anakategori, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {
                Guid gd = Guid.NewGuid();

                if (foto != null && foto.ContentLength > 0)
                {
                    string directory = Server.MapPath("~/Content/uploads/");

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = System.IO.Path.GetExtension(foto.FileName).Substring(1);


                    foto.SaveAs(Path.Combine(directory, gd.ToString() + ".jpg"));

                    anakategori.resim = gd.ToString() + ".jpg";
                }

                else
                    anakategori.resim = anakategori.resim;

                db.Entry(anakategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anakategori);
        }

        // GET: /AnaKategoriEkle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnaKategoriler anakategori = db.AnaKategorisEkles.Find(id);
            if (anakategori == null)
            {
                return HttpNotFound();
            }
            return View(anakategori);
        }

        // POST: /AnaKategoriEkle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnaKategoriler anakategori = db.AnaKategorisEkles.Find(id);
            db.AnaKategorisEkles.Remove(anakategori);
            db.SaveChanges();

            db.AltKategoriEkles.RemoveRange(db.AltKategoriEkles.Where(c => c.AnaKategoriId == id));
            db.SaveChanges();

            return RedirectToAction("Index");
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
