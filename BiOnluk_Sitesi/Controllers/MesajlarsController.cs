using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;
using Microsoft.AspNet.Identity;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MesajlarsController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /Mesajlars/

        public ViewResult Index(string mesajturu)
        {
            string userId = User.Identity.GetUserId();

            if (!string.IsNullOrEmpty(mesajturu))
            {
                if (mesajturu == "0")
                {

                    var d1 = context.Mesajlars.Where(x => x.okundumuAdmin == false && x.aliciId == userId)
                                           .GroupBy(x => new { gonderenId = x.gonderenId }).Select(x => x.FirstOrDefault()).ToList();

                    ViewBag.SiparisTuru = "Cevaplanmayan mesajlar";

                    return View(d1);
                }

                else if (mesajturu == "1")
                {
                    var d2 = context.Mesajlars.Where(x => x.okundumuAdmin == true && x.aliciId == userId)
                        .GroupBy(x => new { gonderenId = x.gonderenId }).Select(x => x.FirstOrDefault()).ToList();

                    ViewBag.SiparisTuru = "Cevaplanan mesajlar";

                    return View(d2);
                }
                else
                {

                    var d3 = context.Mesajlars.GroupBy(x => new { gonderenId = x.gonderenId }).Select(x => x.FirstOrDefault()).ToList();

                    ViewBag.SiparisTuru = "Tüm mesajlar";

                    return View(d3);

                }

            }
         

            var dd = context.Mesajlars.Where(x => x.okundumuAdmin == false && x.aliciId == userId)
                                         .GroupBy(x => new { gonderenId = x.gonderenId }).Select(x => x.FirstOrDefault()).ToList();

            ViewBag.SiparisTuru = "Cevaplanmayan mesajlar";

            return View(dd);
        } 
      
        //
        // GET: /Mesajlars/Details/5

      

        //
        // GET: /Mesajlars/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Mesajlars/Create

        [HttpPost]
        public ActionResult Create(Mesajlar mesajlar)
        {
            if (ModelState.IsValid)
            {
                context.Mesajlars.Add(mesajlar);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(mesajlar);
        }
        
        //
        // GET: /Mesajlars/Edit/5
 
        public ActionResult Edit(int aid)
        {
            Mesajlar mesajlar = context.Mesajlars.Single(x => x.id == aid);
            return View(mesajlar);
        }

        //
        // POST: /Mesajlars/Edit/5

        [HttpPost]
        public ActionResult Edit(Mesajlar mesajlar)
        {
            if (ModelState.IsValid)
            {
                context.Entry(mesajlar).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mesajlar);
        }

        //
        // GET: /Mesajlars/Delete/5
 
        public ActionResult Delete(int aid)
        {
            Mesajlar mesajlar = context.Mesajlars.Single(x => x.id == aid);
            return View(mesajlar);
        }

        //
        // POST: /Mesajlars/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            Mesajlar mesajlar = context.Mesajlars.Single(x => x.id == aid);
            context.Mesajlars.Remove(mesajlar);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public ViewResult Details(string uyeid)
        {
            string userId = User.Identity.GetUserId();

            context.Mesajlars.Where(i => i.aliciId == userId && i.gonderenId == uyeid || i.gonderenId == userId && i.aliciId == uyeid).ToList()
             .ForEach(x => x.okundumuAdmin = true);
            context.SaveChanges();

            return View(context.Mesajlars.Where(x => x.gonderenId == uyeid || x.aliciId == uyeid).ToList());

        }

        [ValidateInput(false)]
        public ActionResult MesajGonder(Mesajlar msg, FormCollection frm)
        {

            string userId = User.Identity.GetUserId();
            string gonderenId = frm["uyeid"].ToString();
            string chckkonuAcikmi = frm["chckkonuAcikmi"];

            var qry = context.Mesajlars
            .Where(i => i.gonderenId == gonderenId && i.konuAcikMi == true)
            .OrderByDescending(x => x.tarih).FirstOrDefault();

            bool chk;

            if (chckkonuAcikmi == "on")
            {
                msg.mesaj = frm["mesajicerik"].ToString();
                chk = true;
            }                
            else
            {
                msg.mesaj = frm["mesajicerik"].ToString() + "<br/><strong><i> (Konu Kapatýldý)</i></strong>";
                chk = false;
            }
                

       


            msg.tarih = DateTime.Now;
            msg.uyeadi = "Site yöneticisi";
            msg.aliciId = gonderenId;
            msg.gonderenId = userId;
            msg.okundumu = false;
            msg.okundumuAdmin = true;
            msg.konuAcikMi = chk;
            msg.konuId = qry.konuId.ToString();
            msg.spId = qry.spId;
            msg.urunadi = qry.urunadi;

            context.Mesajlars.Add(msg);
            context.SaveChanges();



            return Redirect("/Mesajlars/Details?uyeid=" + msg.aliciId + "");
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