using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class FiltreController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();
        //
        // GET: /Filtre/



        public class DummyData
        {
            public string Value { get; set; }
            public string FullName { get { return Value + " is Full Name"; } }
        }

        public JsonResult Template_GetCustomers(string text)
        {
            Utils ut = new Utils();

            var context = new HemenTanitimContext();

            var result = context.Database.SqlQuery<AutoCompleteViewModel>("exec autoComplete_storeProcedure @arananKelime",
                new SqlParameter("arananKelime", text)).ToList();

            result.ForEach(x => x.AnaKategoriAdi = ut.ToSeoUrl_(x.AnaKategoriAdi));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //private static IEnumerable<AutoCompleteViewModel> GetCustomers()
        //{
        //    var context = new HemenTanitimContext();

        //    return context.Database.SqlQuery<AutoCompleteViewModel>("exec autoComplete_storeProcedure ").ToList();

        //}


        [ValidateInput(false)]
        public ActionResult _BasitArama()
        {
            return PartialView();
        }

        [ValidateInput(false)]
        public ActionResult BasitAramaSonuc(string AramaTerimi, int size, int AltKategoriId)
        {
            var sql = db.Database.SqlQuery<UrunListele2>("exec stpr_BasitArama @AramaTerimi,@AltKategoriId",
                                                         new SqlParameter("AramaTerimi", AramaTerimi),
                                                         new SqlParameter("AltKategoriId", AltKategoriId)
                                                         );
            //var sql = db.Database.SqlQuery<UrunListele2>("exec [stpr_BasitArama]  @AramaTerimi ",
            //  new SqlParameter("AramaTerimi", AramaTerimi)).Take(size).ToList();

            return View(sql);
        }

    
        public ActionResult _HizliAramaAltKategoriListesi()
        {
            var anakategoriler = db.AnaKategorisEkles.ToList();

            return PartialView(anakategoriler.ToList());
        }

        public ActionResult HizliAramaaltKategorileriGetir(Int16? anaKatId)
        {
            var altk = db.AltKategoriEkles.Where(c => c.AnaKategoriId == anaKatId);

            return Json(altk, JsonRequestBehavior.AllowGet);
        }
	}

    internal class AutoCompleteViewModel
    {
        public string baslik { get; set; }
        public string AltKategoriAdi { get; set; }
        public string AnaKategoriAdi { get; set; }
        public string ArananKelime { get; set; }
        public int AnaKategoriId { get; set; }
        public int AltKategoriId { get; set; }
    }
}