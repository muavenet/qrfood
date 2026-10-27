using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;
using System.IO;
using QRCoder;

namespace HemenTanitim.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class HotelsController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();

        // GET: Hotels
        public async Task<ActionResult> Index()
        {
            return View(await db.Hotels.ToListAsync());
        }

        // GET: Hotels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotels hotels = await db.Hotels.FindAsync(id);
            if (hotels == null)
            {
                return HttpNotFound();
            }
            return View(hotels);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HotelAdi,odaSayisi,KomisyonOrani")] Hotels hotels)
        {
            if (ModelState.IsValid)
            {
                db.Hotels.Add(hotels);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hotels);
        }

        // GET: Hotels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotels hotels = await db.Hotels.FindAsync(id);
            if (hotels == null)
            {
                return HttpNotFound();
            }
            return View(hotels);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,HotelAdi,odaSayisi,KomisyonOrani")] Hotels hotels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotels).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hotels);
        }

        // GET: Hotels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotels hotels = await db.Hotels.FindAsync(id);
            if (hotels == null)
            {
                return HttpNotFound();
            }
            return View(hotels);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Hotels hotels = await db.Hotels.FindAsync(id);
            db.Hotels.Remove(hotels);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public async Task<JsonResult> OdaEkle(int Id, int OdaSayisi)
        {

            QRCodeGenerator _qrCode = new QRCodeGenerator();

            if (!db.Odalars.Where(x => x.HotelId == Id).Any())
            {
                List<Odalar> od = new List<Odalar>();

                for (int i = 1; i < OdaSayisi;)
                {
                    //https://github.com/codebude/QRCoder/
                    QRCodeData _qrCodeData = _qrCode.CreateQrCode
                        (Request.ServerVariables["SERVER_NAME"] + "/" + Id + "/" +  i, QRCodeGenerator.ECCLevel.Q);                 
                    PdfByteQRCode qrCode = new PdfByteQRCode(_qrCodeData);
                    byte[] qrCodeAsPdfByteArr = qrCode.GetGraphic(20);

                    od.Add(new Odalar {OdaNo = i.ToString(), 
                        HotelId = Id,
                        QrCode = qrCodeAsPdfByteArr
                    });

                    i++;

                }
                db.Odalars.AddRange(od);
                await db.SaveChangesAsync();

                return Json(Id, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Id, JsonRequestBehavior.AllowGet);
            }
        }

        
    }
}
