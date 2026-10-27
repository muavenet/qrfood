using HemenTanitim.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class FaturalarController : Controller
    {
        //
        // GET: /Faturalar/
        HemenTanitimContext db = new HemenTanitimContext();

        public ActionResult PrintInvoice(string invoiceId)
        {
            return new ActionAsPdf(
                           "SiparisFaturasi", new { id = invoiceId }) { FileName = "Invoice.pdf" };
        }
     
        public ActionResult SiparisFaturasi(string id)
        {
            List<FaturaKesimBilgileri> ftr = new List<FaturaKesimBilgileri>();

            var sql = db.Database.SqlQuery<FaturaKesimBilgileri>("exec stpr_Fatura @siparisId", new SqlParameter("siparisId", id)).ToList();

            ftr = sql.ToList();

            return View(ftr);
        }
	}
}