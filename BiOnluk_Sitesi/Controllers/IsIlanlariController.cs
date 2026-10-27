using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace HemenTanitim.Controllers
{
    public class IsIlanlariController : Controller
    {
		HemenTanitimContext db = new HemenTanitimContext();
		Utils ut = new Utils();


		[HttpPost]
		public ActionResult getEnAltKategoriIds(int anaKatId)
		{
			var enaltK = db.AltKategoriEkles.Where(a => a.AnaKategoriId == anaKatId).Select(a => "<option value='" + a.Id + "'>" + a.KategoriAdi + "</option>'");

			string chck = enaltK.FirstOrDefault();

			if (chck == null)
			{
				return Content(String.Join("", "<option value='0'>Bağlı kategori yok</option>"));
			}
			else
			{
				return Content(String.Join("", enaltK));
			}
		}

		// GET: IsIlanlari		

		public ActionResult IsIlanlari_PartialView()
		{
			var sql =
			db.Database.SqlQuery<store_procedure_IsIlanlariListesi>("exec stpr_Portfolyom @UYEID", new SqlParameter("UyeId", User.Identity.GetUserId())).ToList();

			return PartialView(sql.ToList());
		}

		
		public ActionResult IsIlanlariDuzenleStep1_PartialView()
		{

			ViewBag.AnaKategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");


			ViewBag.AltKategoriId = new SelectList(db.AltKategoriEkles, "Id", "KategoriAdi");


			//ViewBag.enAltKategoriId = new SelectList(db.AltKategoriEkles.Where(x => x.Id == urunler.KategoriId), "Id", "KategoriAdi", urunler.enAltKategoriId);

			return PartialView();
		}

		[HttpPost]
		public ActionResult IsIlanlariDuzenleStep1_PartialView(IsIlanlari isI)
		{

			isI.Trending = false;
			isI.userId = User.Identity.GetUserId();			
			isI.durum = false;
			isI.Tarih = DateTime.Now;
			isI.IsIlanAciklamasi = "Girilmedi !";
			

			db.IsIlanlaris.Add(isI);
			db.SaveChanges();

			return View();
		}

		public ActionResult IsIlanlariDuzenleStepDevami_Partial()
		{
			return PartialView();
		}

		public ActionResult IsIlanlariDuzenleStep3()
		{
			IsIlanlari Ii = new IsIlanlari();

			Ii.userId = User.Identity.GetUserId();


			db.IsIlanlaris.Add(Ii);
			db.SaveChanges();

			return View();
		}

		public ActionResult IsIlanlariDuzenleStep4()
		{
			IsIlanlari Ii = new IsIlanlari();

			Ii.userId = User.Identity.GetUserId();


			db.IsIlanlaris.Add(Ii);
			db.SaveChanges();

			return View();
		}

		public ActionResult IsIlanlariDuzenleStep5()
		{
			IsIlanlari Ii = new IsIlanlari();

			Ii.userId = User.Identity.GetUserId();


			db.IsIlanlaris.Add(Ii);
			db.SaveChanges();

			return View();
		}
		// GET: IsIlanlari/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IsIlanlari/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IsIlanlari/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: IsIlanlari/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IsIlanlari/Edit/5
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

        // GET: IsIlanlari/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IsIlanlari/Delete/5
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
