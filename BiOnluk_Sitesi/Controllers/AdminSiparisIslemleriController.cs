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
using System.Net.Mail;
using System.Web.UI;
using System.Text;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Threading.Tasks;

namespace HemenTanitim.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminSiparisIslemleriController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();
        //
        // GET: /AdminSiparisIslemleri/
        public ActionResult Index(string siparisTuru, string tarih_bir, string tarih_iki)
        {

            if (tarih_bir != null)
            {

                var sql = db.Database.SqlQuery<SiparisBilgileri>("exec stpr_TariheGoreUrunDetayRaporu @ilktarih, @sontarih, @durum",
                             new SqlParameter("ilktarih", tarih_bir),
                             new SqlParameter("sontarih", tarih_iki),
                             new SqlParameter("durum", siparisTuru)
                ).ToList();

                return View(sql);
            }
            else
            {
                var list = new List<SiparisBilgileri>();
                return View(list); }

            //if (string.IsNullOrEmpty(siparisTuru) || siparisTuru == "1")
            //{
            //    var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Hazirlaniyor).OrderByDescending(x => x.Id).ToList();

            //    ViewBag.SiparisTuru = "Hazırlanıyor olan siparişler";

            //    return View(dd);
            //}        
            //else if (siparisTuru == "2")
            //{
            //    var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Yola_cikti).OrderByDescending(x => x.Id).ToList();

            //    ViewBag.SiparisTuru = "İşleme alınan siparişler";

            //    return View(dd);
            //}
            //else if (siparisTuru == "3")
            //{
            //    var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Teslim_Edildi).OrderByDescending(x => x.Id).ToList();

            //    ViewBag.SiparisTuru = "Teslim edilen siparişler";

            //    return View(dd);
            //}
            //else if (siparisTuru == "4")
            //{
            //    var dd = db.SiparisBilgileris.Where(x => x.Durumu == (byte)Utils.SiparisDurumu.Siparis_Iptal).OrderByDescending(x => x.Id).ToList();

            //    ViewBag.SiparisTuru = "İptal olan siparişler";

            //    return View(dd);
            //}
            //else
            //{
            //    var dd = db.SiparisBilgileris.OrderByDescending(x => x.Id).ToList();

            //    ViewBag.SiparisTuru = "Tüm siparişler";

            //    return View(dd);
            //}

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

        public ActionResult ExportFatura(int hotelId)
        {

            string dateof = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            List<SiparisBilgileri> spr_Bilgileri = db.SiparisBilgileris.Where(x => x.hotelId == hotelId && x.SiparisTarihi == dateof).ToList();

            return View(spr_Bilgileri);
        }

  
        public ActionResult OTOMATIK_MAIL_GONDER(int hotelId)
        {
            var hotelbilgileri = db.Hotels.Where(x => x.Id == hotelId).FirstOrDefault();
            string dateof = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            var spr_Bilgileri = db.SiparisBilgileris.Where(x => x.hotelId == hotelId && x.SiparisTarihi == dateof).ToList();


            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] {
                                 new DataColumn("İçerik"),
                                 new DataColumn("Oda No"),
                                new DataColumn("Tarih"),
                                new DataColumn("Tutar")});

            foreach (var item in spr_Bilgileri)
            {
                dt.Rows.Add(item.IlanAciklama, item.odaNo, item.SiparisTarihi, item.Tutar);            
            }

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    string companyName = hotelbilgileri.HotelAdi;
                  
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>TYP GÜNLÜK RAPORU</b></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td><b>Tarih :</b>");
                    sb.Append(DateTime.Now);
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td colspan = '2'><b>Hotel Adı Name :</b> ");
                    sb.Append(companyName);
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                        sb.Append(column.ColumnName);
                        sb.Append("</th>");
                    }
                    sb.Append("</tr>");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            sb.Append("<td>");
                            sb.Append(row[column]);
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");

                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();

                        MailMessage mm = new MailMessage("info@kemerburgazotolastik.com", "mevamet@gmail.com");
                        mm.Subject = "TYP Günlük Rapor";
                        mm.Body = "Rapor Ektedir";
                        mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Rapor_"+ dateof + ".pdf"));
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "mail.kemerburgazotolastik.com";
                        smtp.EnableSsl = false;
                        NetworkCredential NetworkCred = new NetworkCredential();
                        NetworkCred.UserName = "info@kemerburgazotolastik.com";
                        NetworkCred.Password = "KYkq19H6";
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
            }
            return View();
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