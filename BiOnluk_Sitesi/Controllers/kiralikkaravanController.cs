using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class kiralikkaravanController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();
        // GET: kiralikkaravan
        public ActionResult Index()
        {
            string urunAdi = "";
            string sehir = "";
            string ilce = "";

            if (RouteData.Values["urunAdi"] != null & RouteData.Values["il"] != null & RouteData.Values["ilce"] != null)
            {
                urunAdi = RouteData.Values["urunAdi"].ToString().Replace("-", " "); ;
                sehir = RouteData.Values["il"].ToString().Replace("-", " "); ;
                ilce = RouteData.Values["ilce"].ToString().Replace("-", " "); ;

                var sql = db.Database.SqlQuery<RouteModel_Tum>("exec stpr_RouteModel @urunAdi,@sehir,@ilce ",
                          new SqlParameter("@urunAdi", urunAdi), new SqlParameter("@sehir", sehir), new SqlParameter("@ilce", ilce)).FirstOrDefault();

                return View(sql);
            }
            else if (RouteData.Values["urunAdi"] != null & RouteData.Values["il"] != null)
            {
                urunAdi = RouteData.Values["urunAdi"].ToString().Replace("-", " "); ;
                sehir = RouteData.Values["il"].ToString().Replace("-", " "); ;

                var sql = db.Database.SqlQuery<RouteModel_Tum>("exec stpr_RouteModel @urunAdi,@sehir,@ilce ",
                          new SqlParameter("@urunAdi", urunAdi), new SqlParameter("@sehir", sehir), new SqlParameter("@ilce", "")).FirstOrDefault();

                return View(sql);
            }

            else if (RouteData.Values["urunAdi"] != null)
            {
                urunAdi = RouteData.Values["urunAdi"].ToString().Replace("-", " "); ;

                var sql = db.Database.SqlQuery<RouteModel_Tum>("exec stpr_RouteModel @urunAdi, @sehir, @ilce ",
                          new SqlParameter("urunAdi", urunAdi),
                          new SqlParameter("sehir", ""),
                          new SqlParameter("ilce", "")).FirstOrDefault();


                return View(sql);
            }
            else
            {
                RouteModel_Tum rt = new RouteModel_Tum();

                rt.Kelime_adi = null;

                return View(rt);
            }
        }

        public ActionResult _AnahtarKelimeler()
        {
            return PartialView(db.AnahtarKelimelers.ToList());
        }
        public ActionResult _Iller()
        {
            var illeriGetir = db.iller.OrderBy(r => r.il_id).ToList();

            return PartialView(illeriGetir.ToList());
        }
        public ActionResult _Ilceler()
        {
            string urunAdi = "";
            string sehir = "";
            string ilce = "";

            if (RouteData.Values["kelimeadi"] != null & RouteData.Values["il"] != null)
            {
                urunAdi = RouteData.Values["kelimeadi"].ToString().ToLower().Replace("-", " ");
                sehir = RouteData.Values["il"].ToString().ToLower().Replace("-", " ");


                var sql = db.Database.SqlQuery<RouteModel_Tum>("exec stpr_ilceCek @urunAdi,@sehir ",
                          new SqlParameter("@urunAdi", urunAdi), new SqlParameter("@sehir", sehir)).ToList();

                return PartialView(sql.ToList());
            }

            else
                return PartialView();
        }
    }
}
