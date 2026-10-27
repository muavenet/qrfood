using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;

namespace HemenTanitim.Controllers
{   
    public class SiparisIptalsController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /SiparisIptals/

        public ViewResult Index()
        {

            var sql = context.Database.SqlQuery<SiparisIptalBilgileri>("exec stpr_iptalSiparisler").ToList();           

            return View(sql);
        }

        //
        // GET: /SiparisIptals/Details/5

        public ViewResult Details(int aid)
        {
            SiparisIptal siparisiptal = context.SiparisIptals.Single(x => x.Id == aid);
            return View(siparisiptal);
        }


        //
        // GET: /SiparisIptals/Delete/5
 
        public ActionResult Delete(int aid)
        {
            SiparisIptal siparisiptal = context.SiparisIptals.Single(x => x.Id == aid);
            return View(siparisiptal);
        }

        //
        // POST: /SiparisIptals/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            SiparisIptal siparisiptal = context.SiparisIptals.Single(x => x.Id == aid);
            context.SiparisIptals.Remove(siparisiptal);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}