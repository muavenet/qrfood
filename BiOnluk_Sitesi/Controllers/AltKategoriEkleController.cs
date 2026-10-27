using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HemenTanitim.Models;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AltKategoriEkleController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();

        // GET: /AltKategoriEkle/
        public ActionResult Index()
        {
            //var altkategoriekles = db.AltKategoriEkles.Include(a => a.AnaKategoriler);

            var altkategoriekles = db.AltKategoriEkles.Join(db.AnaKategorisEkles, x => x.AnaKategoriId, x => x.Id, (x, y) => new { x, y })
           .OrderByDescending(a => a.y.Id)          
           .Select(a => new
           {
               ID = a.x.Id,
               AltKategoriAdi = a.x.KategoriAdi,
               AnaKategoriAdi = a.y.KategoriAdi
           })
           .AsEnumerable().Select(x => new KategoriListe
                  {
                      AnaKategoriAdi = x.AnaKategoriAdi,
                      AltKategoriAdi = x.AltKategoriAdi,
                      ID = x.ID

                  }).ToList();
                  



            return View(altkategoriekles);
        }

        // GET: /AltKategoriEkle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AltKategoriEkle altkategoriekle = db.AltKategoriEkles.Find(id);
            if (altkategoriekle == null)
            {
                return HttpNotFound();
            }
            return View(altkategoriekle);
        }

        // GET: /AltKategoriEkle/Create
        public ActionResult Create()
        {
            ViewBag.AnaKategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");
            return View();
        }

        // POST: /AltKategoriEkle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,KategoriAdi,AnaKategoriId,Aciklama,metaDescripton,metaKeywords,title,tags")] AltKategoriEkle altkategoriekle)
        {
            if (ModelState.IsValid)
            {
                db.AltKategoriEkles.Add(altkategoriekle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnaKategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", altkategoriekle.AnaKategoriId);
            return View(altkategoriekle);
        }

        // GET: /AltKategoriEkle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AltKategoriEkle altkategoriekle = db.AltKategoriEkles.Find(id);
            if (altkategoriekle == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnaKategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", altkategoriekle.AnaKategoriId);
            return View(altkategoriekle);
        }

        // POST: /AltKategoriEkle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,KategoriAdi,AnaKategoriId,Aciklama,metaDescripton,metaKeywords,title,tags")] AltKategoriEkle altkategoriekle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(altkategoriekle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnaKategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", altkategoriekle.AnaKategoriId);
            return View(altkategoriekle);
        }

        // GET: /AltKategoriEkle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AltKategoriEkle altkategoriekle = db.AltKategoriEkles.Find(id);
            if (altkategoriekle == null)
            {
                return HttpNotFound();
            }
            return View(altkategoriekle);
        }

        // POST: /AltKategoriEkle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AltKategoriEkle altkategoriekle = db.AltKategoriEkles.Find(id);
            db.AltKategoriEkles.Remove(altkategoriekle);
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
