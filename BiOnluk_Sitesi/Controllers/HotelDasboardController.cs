using HemenTanitim.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class HotelDasboardController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();

        protected int hotelId()
        {   
            string Id = User.Identity.GetUserId();           
            return db.Users.Where(x => x.Id == Id).FirstOrDefault().hotelId;
        }
        // GET: HotelDasboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _UstMenu()
        {
            var sql = db.Database.SqlQuery<UstMenuModel>("exec stpr_ustmenuHotel @hotelId", new SqlParameter("hotelId", hotelId())).FirstOrDefault();

            return PartialView(sql);
        }

        public ActionResult _AylikSatisGrafigi()
        {
            return PartialView();
        }

        //[OutputCache(Duration = 86400, VaryByParam = "none")]
        public JsonResult AylikSatisGrafigi()
        {
            var sql = db.Database.SqlQuery<AylikSatisOranlari>("exec stpr_AylikSatisOranlariHotel @hotelId", new SqlParameter("hotelId", hotelId())).ToList();

            return Json(sql, JsonRequestBehavior.AllowGet);
        }

        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult _GunlukSatisGrafigi()
        {
            return PartialView();
        }

        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult GunlukSatisGrafigi()
        {
            var sql = db.Database.SqlQuery<GunlukSatisOranlari>("exec [stpr_gunluksatisHotel] @hotelId", new SqlParameter("hotelId", hotelId())).ToList();

            return Json(sql, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Siparisler(string siparisTuru, string tarih_bir, string tarih_iki)
        {
            int hotel_id = hotelId();

            if (tarih_bir != null)
            {

                var sql = db.Database.SqlQuery<SiparisBilgileri>("exec stpr_HotelTariheGoreUrunDetayRaporu @hotelId, @ilktarih, @sontarih, @durum",
                             new SqlParameter("hotelId", hotel_id),
                             new SqlParameter("ilktarih", tarih_bir),
                             new SqlParameter("sontarih", tarih_iki),
                             new SqlParameter("durum", siparisTuru)
                ).ToList();

                ViewBag.toplam = sql.Sum(x => x.Tutar);

                return View(sql);
            }
            else
            {
                var list = new List<SiparisBilgileri>();
                return View(list);
            }

        }

    }
}