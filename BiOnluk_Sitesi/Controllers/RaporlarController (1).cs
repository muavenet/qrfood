using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HemenTanitim.Controllers
{
    public class RaporlarController : Controller
    {

        HemenTanitimContext db = new HemenTanitimContext();
        //
        // GET: /Raporlar/
        public ActionResult SayfaZiyaretcileri()
        {
            var sql = db.Urunlers.ToList().Select(x => new EnCokZiyaretEdenler { UrunAdi = x.UrunAdi, ZiyaretciSayisi = x.ZiyaretciSayisi });

             TempData["sayfaZiyaretcileri"] = sql;

             return View(sql);
        }

        public ActionResult EncokSiparisEdilenUrunler()
        {
            var sql = db.Database.SqlQuery<EncokSiparisAlanUrunler>("exec stpr_EnCokSiparisAlanUrunler").ToList();

            TempData["encokSiparisEdilenUrunler"] = sql;

            return View(sql);
        }

        public ActionResult siparisIstatistikleri()
        {
            var sql = db.Database.SqlQuery<SiparisIstatistikleri>("exec stpr_siparisIstatikleri").ToList();

            TempData["siparisIstatistikleri"] = sql;

            return View(sql);
        }

        public JsonResult siparisIstatistikleriGrafikTablosu()
        {
            var sql = db.Database.SqlQuery<SiparisIstatistikleri>("exec stpr_siparisIstatikleri").ToList();

            return Json(sql, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ciroYillik()
        {
            var sql = db.Database.SqlQuery<ciroYillik>("exec stpr_CiroYillik").ToList();

            TempData["ciroYillik"] = sql;

            TempData["ciroYillikkGrafik"] = sql;

            return View(sql);
        }

        public JsonResult ciroYillikGrafikTablosu()
        {

            return Json(TempData["ciroYillikkGrafik"], JsonRequestBehavior.AllowGet);
        }

        public ActionResult ciroAylik(string yil)
        {
            if(String.IsNullOrEmpty(yil))
            { yil = DateTime.Now.Year.ToString(); }

            var sql = db.Database.SqlQuery<ciroAylik>("exec stpr_CiroAylik @yil", new SqlParameter("yil", yil)).ToList();

            TempData["ciroAylik"] = sql;

            TempData["ciroAylikGrafik"] = sql;

            return View(sql);
        }

        public JsonResult ciroAylikGrafikTablosu()
        {
            return Json(TempData["ciroAylikGrafik"], JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult ExceleAktar(string paramaterName)
        {
            GridView gridvw = new GridView();
            gridvw.DataSource = TempData[paramaterName];
            gridvw.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename="+paramaterName+".xls");
            Response.ContentType = "application/excel";
            StringWriter swr = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(swr);
            gridvw.RenderControl(tw);
            Response.Write(swr.ToString());

            Response.End();

            return View();

        }
	}
}