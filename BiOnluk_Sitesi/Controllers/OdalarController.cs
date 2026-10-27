using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;

using QRCoder;

namespace HemenTanitim.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class OdalarController : Controller
    {
        private HemenTanitimContext db = new HemenTanitimContext();

        // GET: Odalars
        public ActionResult Index()
        {
            return View(db.Odalars.ToList());
        }

        // GET: Odalars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Odalar odalar = db.Odalars.Find(id);
            if (odalar == null)
            {
                return HttpNotFound();
            }
            return View(odalar);
        }

        // GET: Odalars/Create
        public ActionResult Create(int id)
        {
            Odalar odalar = new Odalar();
            odalar.HotelId = id;

            return View(odalar);
        }

        // POST: Odalars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HotelId,OdaNo")] Odalar odalar)
        {
            if (ModelState.IsValid)
            {

                var odaGuid = Guid.NewGuid().ToString();

                var hotelgetir = db.Hotels.Find(odalar.HotelId);
                hotelgetir.odaSayisi = hotelgetir.odaSayisi + 1;

                db.Entry(hotelgetir).State = EntityState.Modified;
                await db.SaveChangesAsync();

                QRCodeGenerator _qrCode = new QRCodeGenerator();
                QRCodeData _qrCodeData = _qrCode.CreateQrCode
                    ("https://" + Request.ServerVariables["SERVER_NAME"] + "/Account/Login/" + odalar.HotelId + "/" + odaGuid, QRCodeGenerator.ECCLevel.Q);
                PdfByteQRCode qrCode = new PdfByteQRCode(_qrCodeData);
                byte[] qrCodeAsPdfByteArr = qrCode.GetGraphic(20);

                odalar.odaGuidId = odaGuid;
                odalar.QrCode = qrCodeAsPdfByteArr;


                db.Odalars.Add(odalar);
                await db.SaveChangesAsync();

                return RedirectToAction("Edit", new { id = odalar.HotelId });
            }

            return View(odalar);
        }

        // GET: Odalars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var odalar = db.Odalars.Where(z => z.HotelId == id).ToList();
            if (odalar == null)
            {
                return HttpNotFound();
            }
            return View(odalar);
        }

        // POST: Odalars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<JsonResult> OdaGuncelle(string odaNo, int Id)
        {
            Odalar odalar;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            if (ModelState.IsValid)
            {
                odalar = db.Odalars.Find(Id);   

                QRCodeData _qrCodeData = _qrCode.CreateQrCode
                    ("https://" + Request.ServerVariables["SERVER_NAME"] + "/Account/Login/" + odalar.HotelId + "/" + odalar.odaGuidId, QRCodeGenerator.ECCLevel.Q);
                PdfByteQRCode qrCode = new PdfByteQRCode(_qrCodeData);
                byte[] qrCodeAsPdfByteArr = qrCode.GetGraphic(20);

                odalar.QrCode = qrCodeAsPdfByteArr;
                odalar.OdaNo = odaNo;

                db.Entry(odalar).State = EntityState.Modified;
                db.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return Json("HATA");
        }

        // POST: Odalars/Delete/5
        

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Odalar odalar = db.Odalars.Find(id);
            db.Odalars.Remove(odalar);
            await db.SaveChangesAsync();

            var hotelgetir = db.Hotels.Find(odalar.HotelId);
            hotelgetir.odaSayisi = db.Odalars.Where(x => x.HotelId == odalar.HotelId).Count();
            db.Entry(hotelgetir).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = odalar.HotelId });
        }

        public ActionResult FileDownload(int id)
        {
            var odalar = db.Odalars.Find(id);

            MemoryStream ms = new MemoryStream(odalar.QrCode);
            return new FileStreamResult(ms, "application/pdf");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
