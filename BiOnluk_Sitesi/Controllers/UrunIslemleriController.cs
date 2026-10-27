using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HemenTanitim.Models;
using MvcFileUploader;
using MvcFileUploader.Models;
using ImageResizer;


namespace HemenTanitim.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class UrunIslemleriController : Controller
    {
        Fotolar fotos = new Fotolar();

        private HemenTanitimContext db = new HemenTanitimContext();

        // GET: /UrunIslemleri/
        public ActionResult Index(string siralama)
        {
            if (string.IsNullOrEmpty(siralama))
            {
                ViewBag.SiralamaTuru = "Tüm ürünler";
                return View(db.Urunlers.ToList());
            }
            else
            {
                ViewBag.SiralamaTuru = "Vitrin ürünleri";
                return View(db.Urunlers.Where(x => x.Trending== true).ToList());
            }
            
          
            
        }

        // GET: /UrunIslemleri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunlers.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // GET: /UrunIslemleri/Create
       
        public ActionResult Create(Urunler urunler)
        {
            Session["guidtut"] = Guid.NewGuid().ToString();

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");

            return View();
        }

        // POST: /UrunIslemleri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create([Bind(Include = "UrunId,UrunAdi,UrunIcerik,Fiyat,satiSayisi,DmozKaydi, UrunUrl, NewsKaydi,AlexaPuani,GoogleIndex,MozRank,Moz,DaTarih,MozPa,domainYasi,yayinYasi,AnaKategoriId,AltKategoriId,Vitrin,ZiyaretciSayisi,metaDescription,title")] Urunler urunler, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                //urunler.Fotolar = Convert.ToString(collection["Fotolar"]);

                urunler.Tarih = DateTime.Now;
                urunler.ZiyaretciSayisi = 0;
                urunler.Guid = Session["guidtut"].ToString();
                urunler.Onay = true;
                //Loop through each uploaded file

                db.Urunlers.Add(urunler);
                db.SaveChanges();

                Session["guidtut"] = null;
                return RedirectToAction("Index");

            }

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");

            return View(urunler);
        }

        //public byte[] ConvertToBytes(HttpPostedFileBase image)
        //{
        //    byte[] imageBytes = null;
        //    BinaryReader reader = new BinaryReader(image.InputStream);
        //    imageBytes = reader.ReadBytes((int)image.ContentLength);
        //    return imageBytes;
        //}


        // GET: /UrunIslemleri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Urunler urunler = db.Urunlers.Find(id);

            if (urunler == null)
            {
                return HttpNotFound();
            }

            Session["guidtut"] = urunler.Guid;

            ViewBag.UrunId = new SelectList(db.Fotolars.Where(g => g.UrunId == urunler.Guid), "UrunId", "ResimYolu", urunler.Guid);

            var musteriSiteleri = db.MusteriSiteleris.Where(x => x.siteId == urunler.siteId).FirstOrDefault();

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", musteriSiteleri.AnaKategoriId);

            ViewBag.AltKategoriId_ = new SelectList(db.AltKategoriEkles.Where(x => x.AnaKategoriId == musteriSiteleri.AnaKategoriId), "Id", "KategoriAdi", musteriSiteleri.AltKategoriId);

            return View(urunler);
        }

        // POST: /UrunIslemleri/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Urunler urunler, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                urunler.Onay = true;
                urunler.Guid = Session["guidtut"].ToString();
                urunler.Tarih = DateTime.Now;
                db.Entry(urunler).State = EntityState.Modified;
                db.SaveChanges();

                Session["guidtut"] = null;

                MusteriSiteleri ms = db.MusteriSiteleris.Find(urunler.siteId);

                ms.AnaKategoriId = Convert.ToInt32(frm["AnaKategoriId"]);
                ms.AltKategoriId =  Convert.ToInt32(frm["AltKategoriId"]);

                db.Entry(ms).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.UrunId = new SelectList(db.Fotolars.Where(g => g.UrunId == urunler.Guid), "UrunId", "ResimYolu", urunler.Guid);

            var musteriSiteleri = db.MusteriSiteleris.Where(x => x.siteId == urunler.siteId).FirstOrDefault();

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", musteriSiteleri.AnaKategoriId);

            ViewBag.AltKategoriId_ = new SelectList(db.AltKategoriEkles.Where(x => x.AnaKategoriId == musteriSiteleri.AnaKategoriId), "Id", "KategoriAdi", musteriSiteleri.AltKategoriId);

            return View(urunler);
        }

        // GET: /UrunIslemleri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunlers.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // POST: /UrunIslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Urunler urunler = db.Urunlers.Find(id);
            db.Urunlers.Remove(urunler);
            db.SaveChanges();
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

          [ValidateInput(false)]
        public ActionResult UploadFile([Bind(Include = "UrunId,ResimYolu,ContentType,Vitrin")] Fotolar fotolar, Urunler urunler) // optionally receive values specified with Html helper
        {
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();

            Guid resimyol = Guid.NewGuid();

            for (var i = 0; i < Request.Files.Count; i++)
            {
                var st = FileSaver.StoreFile(x =>
                {
                    x.File = Request.Files[i];

                    //note how we are adding an additional value to be posted with delete request
                    //and giving it the same value posted with upload
                    x.DeleteUrl = Url.Action("DeleteFile", new { entityId = fotolar.Id});
                    x.StorageDirectory = Server.MapPath("~/Content/uploads");
                    x.UrlPrefix = "/Content/uploads";// this is used to generate the relative url of the file              
                    //overriding defaults
                    x.FileName = resimyol.ToString() + ".jpg";// default is filename suffixed with filetimestamp
                    x.ThrowExceptions = true;//default is false, if false exception message is set in error property
                   
                });

                statuses.Add(st);

                fotolar.UrunId = Session["guidtut"].ToString();
                fotolar.ContentType = st.type;
                fotolar.ResimYolu = "/Content/uploads/" + resimyol.ToString() + ".jpg";
                fotolar.ThumbnailYolu = "/Content/thumbnails/" + resimyol.ToString() + ".jpg";
                fotolar.Vitrin = false;
                
            }
            foreach (string fileKey in System.Web.HttpContext.Current.Request.Files.Keys)
            {
                HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[fileKey];
                if (file.ContentLength <= 0) continue; //Skip unused file controls.

                //The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                //Destination paths can have variables like <guid> and <ext>, or 
                //even a santizied version of the original filename, like <filename:A-Za-z0-9>
                ImageResizer.ImageJob i = new ImageResizer.ImageJob(file, "~/Content/thumbnails/" + resimyol.ToString() + ".<ext>",
                    new ImageResizer.ResizeSettings(
                            "height=170;width=255;format=jpg;mode=max"));
                i.CreateParentDirectory = true; //Auto-create the uploads directory.
                i.Build();
            }

            db.Fotolars.Add(fotolar);
            db.SaveChanges();
            //statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            //todo: add additional code to generate thumbnail for videos, associate files with entities etc

            //adding thumbnail url for jquery file upload javascript plugin
            statuses.ForEach(x => x.thumbnailUrl = x.url); // uses ImageResizer httpmodule to resize images from this url

            //statuses.ForEach(x => x.thumbnailUrl = x.url ); // uses ImageResizer httpmodule to resize images from this url
            
            //setting custom download url instead of direct url to file which is default
            statuses.ForEach(x => x.url = Url.Action("DownloadFile", new { fileUrl = x.url, mimetype = x.type }));


            //server side error generation, generate some random error if entity id is 13
            if (urunler.UrunId == 13)
            {
                var rnd = new Random();
                statuses.ForEach(x =>
                {
                    //setting the error property removes the deleteUrl, thumbnailUrl and url property values
                    x.error = rnd.Next(0, 2) > 0 ? "We do not have any entity with unlucky Id : '13'" : String.Format("Your file size is {0} bytes which is un-acceptable", x.size);
                    //delete file by using FullPath property
                    if (System.IO.File.Exists(x.FullPath)) System.IO.File.Delete(x.FullPath);

                    
                });
            }

            var viewresult = Json(new { files = statuses });
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/plain";

            return viewresult;
        }
      

        //here i am receving the extra info injected
        [HttpPost] // should accept only post
        public ActionResult DeleteFile(int? entityId, string fileUrl)
        {
            var filePath = Server.MapPath(fileUrl);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            var viewresult = Json(new { error = String.Empty });
            //for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
                viewresult.ContentType = "text/plain";

            Fotolar fotolar = db.Fotolars.First(x => x.ResimYolu == fileUrl);
            db.Fotolars.Remove(fotolar);

            db.SaveChanges();


            return viewresult; // trigger success
        }

        [HttpPost, ActionName("Deletefoto")] // should accept only post
        public ActionResult DeleteFoto(string fileUrl)
        {
            var filePath = Server.MapPath(fileUrl);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
          
            Fotolar fotolar = db.Fotolars.First(x => x.ResimYolu == fileUrl);
            db.Fotolars.Remove(fotolar);

            db.SaveChanges();

            return Redirect(Request.UrlReferrer.AbsolutePath);
        }

        public ActionResult DownloadFile(string fileUrl, string mimetype)
        {
            var filePath = Server.MapPath("~" + fileUrl);

            if (System.IO.File.Exists(filePath))
                return File(filePath, mimetype);
            else
            {
                return new HttpNotFoundResult("File not found");
            }
        }

        public ActionResult ImageGetir(int id)
        {
            Fotolar image = db.Fotolars.Find(id);
            return File(image.ResimYolu, image.ContentType);
        }


        public ActionResult SecilenleriSil(int[] Id, string[] gd)
        {

            if (Id != null)
            {
                var urunler = from urun in db.Urunlers
                              where Id.Contains(urun.UrunId)
                              select urun;

                var fotolar = from foto in db.Fotolars
                              where gd.Contains(foto.UrunId)
                              select foto;


                foreach (var u in urunler)
                {
                    db.Urunlers.Remove(u);
                }

                db.SaveChanges();

                foreach (var u in fotolar)
                {
                    db.Fotolars.Remove(u);

                    System.IO.File.Delete(Server.MapPath(u.ResimYolu));

                    System.IO.File.Delete(Server.MapPath(u.ThumbnailYolu));
                }

                db.SaveChanges();
                    return Json("tamam");
            }
            else
                return Json("hata");
        
        }


        [HttpPost]
        public ActionResult getEnAltKategoriIds(int anaKatId)
        {
            var enaltK = db.AltKategoriEkles.Where(a => a.AnaKategoriId == anaKatId).Select(a => "<option value='" + a.Id + "'>" + a.KategoriAdi + "</option>'");

            string chck = enaltK.FirstOrDefault();

            if (chck == null)
            {
                return Content(String.Join("", "<option value='0'>Bağlı kategori yok</option>"));
            }
            else
            {
                return Content(String.Join("", enaltK));
            }
        }

    
        //public ActionResult Arama(string aramaString)
        //{

        //    var urunlerara = from m in db.Urunlers
        //                 select new UrunListele { UrunAdi = m.UrunAdi, ResimYolu = m.re};

        //    if (!String.IsNullOrEmpty(aramaString))
        //    {
        //        urunlerara = urunlerara.Where(s => s.UrunAdi.Contains(aramaString));
        //    }

        //    return View(urunlerara.ToList());
        //}
    }
}
