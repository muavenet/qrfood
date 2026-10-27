using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{

    public class MobileController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();
        // GET: Mobile
        public ActionResult AnaKategoriler()
        {
            return View(db.AnaKategorisEkles.ToList());
        }
        [Authorize]
        public ActionResult Panel()
        {
            return View();
        }
        public ActionResult Arama()
        {
            return View();
        }
    }
}