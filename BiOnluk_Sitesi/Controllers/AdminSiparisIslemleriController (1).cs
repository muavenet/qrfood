using CrystalDecisions.CrystalReports.Engine;
using HemenTanitim.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;

using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace HemenTanitim.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminSiparisIslemleriController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();
        //
        // GET: /AdminSiparisIslemleri/
        public ActionResult Index(string siparisTuru)
        {

            if (string.IsNullOrEmpty(siparisTuru))
            {
                var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Hazirlaniyor).OrderByDescending(x => x.Id).ToList();

                ViewBag.SiparisTuru = "Hazırlanıyor olan siparişler";

                return View(dd);
            }        
            else if (siparisTuru == "Yola Çıktı")
            {
                var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Yola_cikti).OrderByDescending(x => x.Id).ToList();

                ViewBag.SiparisTuru = "İşleme alınan siparişler";

                return View(dd);
            }
            else if (siparisTuru == "Teslim Edildi")
            {
                var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Teslim_Edildi).OrderByDescending(x => x.Id).ToList();

                ViewBag.SiparisTuru = "Teslim edilen siparişler";

                return View(dd);
            }
            else if (siparisTuru == "Sipariş İptal")
            {
                var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Siparis_Iptal).OrderByDescending(x => x.Id).ToList();

                ViewBag.SiparisTuru = "İptal olan siparişler";

                return View(dd);
            }
            else
            {
                var dd = db.SiparisBilgileris.OrderByDescending(x => x.Id).ToList();

                ViewBag.SiparisTuru = "Tüm siparişler";

                return View(dd);
            }

        }

        public ActionResult TeslimDurumuDegistir(string selectedValue, string Id)
        {
            SiparisBilgileri sp = db.SiparisBilgileris.Where(x => x.SiparisId == Id).SingleOrDefault();

            sp.Durumu = Convert.ToByte(selectedValue);

            db.Entry(sp).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true);
        }
        public JsonResult Okundu( string Id)
        {
            SiparisBilgileri sp = db.SiparisBilgileris.Where(x => x.SiparisId == Id).SingleOrDefault();
            sp.Okundumu = true; 
            db.Entry(sp).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true);
        }

        public ActionResult Detay(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiparisBilgileri siparisler = db.SiparisBilgileris.Where(x => x.SiparisId == id).FirstOrDefault();

            ViewBag.HotelAdi = db.Hotels.Where(x => x.Id == siparisler.hotelId).FirstOrDefault().HotelAdi;

            siparisler.Okundumu = true;
            db.Entry(siparisler).State = EntityState.Modified;
            db.SaveChanges();

            if (siparisler == null)
            {
                return HttpNotFound();
            }

            return View(siparisler);
        }

       
        public ActionResult ExportFatura(string id)
        {
            List<FaturaKesimBilgileri> ftr = new List<FaturaKesimBilgileri>();

            var sql = db.Database.SqlQuery<FaturaKesimBilgileri>("exec stpr_Fatura @siparisId", new SqlParameter("siparisId", id)).ToList();

            ftr = sql.ToList();

                //ReportClass rptH = new ReportClass();
                //rptH.FileName = Server.MapPath("~/Reports/FaturaKes.rpt");
                //rptH.Load();
                //rptH.SetDataSource(ftr);
                //Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //return File(stream, "application/pdf");   



              ReportDocument rd = new ReportDocument();

              rd.Load(Path.Combine(Server.MapPath("~/Reports"), "FaturaKes.rpt"));
              rd.SetDataSource(ftr);

              Response.Buffer = false;
              Response.ClearContent();
              Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Fatura.pdf");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult _UyeBilgileri(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var getuser = db.Users.Find(id);

            return PartialView(getuser);
        }

        public ActionResult _FaturaBilgileri(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var getfatura = db.Faturas.Find(id);

            return PartialView(getfatura);
        }

        public ActionResult _KargoBilgileri(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var getKargo = db.Kargo.Find(id);

            return PartialView(getKargo);
        }


        readonly string _connString = ConfigurationManager.ConnectionStrings["DB150408011842Entities"].ConnectionString;

        public ActionResult RealTimeSiparisler()
        {
            return View();
        }

       
        public JsonResult GetSiparisler()
        {
            var messages = new List<SiparisOzet>();

            using (var connection = new SqlConnection(_connString))
            {
                string dateof = DateTime.Today.ToString("yyyy-MM-dd");

                connection.Open();

                using (SqlCommand command = new SqlCommand(@"Select [OkunduMu],[OdemeTipi],[SiparisId],[Tutar] from dbo.SiparisBilgileri WHERE [SiparisTarihi] = '" + dateof + "' order by Id desc", connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);

                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new SiparisOzet {
                            Okundumu= (bool)reader["OkunduMu"],
                            siparisId = (string)reader["SiparisId"], 
                            IlanAdi = "YENİ SİPARİŞ",
                            OdemeTipi = (byte)reader["OdemeTipi"],
                            Tutar = (decimal)reader["Tutar"] });
                    }

                }

                return Json(messages, JsonRequestBehavior.AllowGet);
            }
        }

     

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SiparisHub.Show();
            }
        }
    }
}