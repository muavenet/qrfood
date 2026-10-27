using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using WebMatrix.Data;
using System.Threading.Tasks;


namespace HemenTanitim.Controllers
{
    public class FiyatlandirmaController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();
        HemenTanitim.Models.Utils ut = new Models.Utils();
        // GET: Fiyatlandirma
        public ActionResult Tablolar(int? page)
        {
            IEnumerable<Fiyatlandirma> modelList = new List<Fiyatlandirma>();

            modelList = db.Fiyatlandirmas.Take(15).OrderByDescending(x => x.Id).ThenBy(n => n.goruntulemeSirasi).ToList();

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

            return View(modelList.ToPagedList(pageNumber, 6));
        }

       

        public JsonResult yapilacakSiralamDegistir(int[] Ids)
        {
            int[] ids = Ids;

            foreach (var id in ids)
            {
                Fiyatlandirma yp = db.Fiyatlandirmas.Find(id);

                yp.BasicGoruntulemeSirasi = Array.FindIndex(ids, i => i == id);

                db.Entry(yp).State = EntityState.Modified;

                db.SaveChanges();
            }
            return Json(true);
        }
        public ActionResult GorevEkle(string yapilacak, string tarih, int? id)
        {
            Fiyatlandirma modelList;

            if (id > 0)
            {
                modelList = db.Fiyatlandirmas.Find(id);
                modelList.BasisIcerik = yapilacak;
                db.Entry(modelList).State = EntityState.Modified;
            }
            else
            {
                modelList = new Fiyatlandirma();
                modelList.BasisIcerik = yapilacak;
                db.Fiyatlandirmas.Add(modelList);
            }

            db.SaveChanges();

            TempData["FiyatlandirmaEklendi"] = "* Görev eklendi";

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Tablolar", "Fiyatlandirma");

            return Json(new { Url = redirectUrl });
        }
       
        public ActionResult GorevSil(int id)
        {
            Fiyatlandirma yp = db.Fiyatlandirmas.Find(id);

            db.Fiyatlandirmas.Remove(yp);
            db.SaveChanges();

            TempData["FiyatlandirmaEklendi"] = "*** Görev Silindi";

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "AdminDasboard");

            return Json(new { Url = redirectUrl });
        }

    }
}