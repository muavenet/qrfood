using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HemenTanitim.Controllers
{
    public class KategoriListesiGetirController : Controller
    {
        //
        // GET: /KategoriListesiGetir/

        private HemenTanitimContext db = new HemenTanitimContext();
        
        [ChildActionOnly]
        public ActionResult Index()
        {

            var anakategoriler = db.AnaKategorisEkles.OrderBy(x=> x.siralama).ToList();

            return PartialView(anakategoriler.ToList());
        }

        public ActionResult AltKategoriListesi()
        {
            int katId = Convert.ToInt32(RouteData.Values["AnaKategoriId"].ToString());

            var altkategoriler = db.AltKategoriEkles.Where(x => x.AnaKategoriId == katId).OrderBy(x => x.siralama).ToList();

            return PartialView(altkategoriler.ToList());
        }

        
	}
}