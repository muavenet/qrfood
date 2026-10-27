using HemenTanitim.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
  
    public class PortfolyoController : Controller
    {

        HemenTanitimContext db = new HemenTanitimContext();
        Utils ut = new Utils();

        public ActionResult Portfolyo_m(int? Id)
        {
          
                ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");
        

            return View();
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Portfolyo_Create(Portfolyo_m portf)
        {
            string uid = User.Identity.GetUserId();

            Portfolyo pf = new Portfolyo();

            if (ModelState.IsValid)
            {
                //urunler.Fotolar = Convert.ToString(collection["Fotolar"]);

                var fileName = Path.GetFileName(portf.PostedFile.FileName);

                var physicalPath = Path.Combine(Server.MapPath("~/Content/uploads/Portfolyo/" + uid + "/" + fileName));

                if (!Directory.Exists(Path.Combine(Server.MapPath("~/Content/uploads/Portfolyo/" + uid + "/"))))
                {
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/uploads/Portfolyo/" + uid + "/")));
                }

                portf.PostedFile.SaveAs(physicalPath);


                pf.port_Id = Guid.NewGuid().ToString();
                pf.demoUrl = portf.demoUrl;
                pf.baslik = portf.baslik;
                pf.AnaKategoriId = portf.AnaKategoriId;
                pf.AltKategoriId = portf.AltKategoriId;
                pf.Fotograflar = "/Content/uploads/Portfolyo/" + uid + "/" + fileName;
                pf.userId = uid;
                pf.eklenmeTarihi = DateTime.Now;

                db.Portfolyos.Add(pf);
                db.SaveChanges();

                return RedirectToAction("Portfolyo_m");

            }

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi");

            ViewBag.MusteriSiteleri = db.MusteriSiteleris.Where(g => g.uyeId == uid).ToList();

            return View(portf);
        }

      
        public ActionResult Portfolyo_edit(string Id)
        {
            Portfolyo pf = db.Portfolyos.Where(z => z.port_Id == Id).Single();

            Portfolyo_m portf = new Portfolyo_m();

            ViewBag.Id = pf.Id;


            portf.port_Id = pf.port_Id;
            portf.Fotograflar = pf.Fotograflar;
            portf.demoUrl = pf.demoUrl;
            portf.baslik = pf.baslik;
            portf.AnaKategoriId = pf.AnaKategoriId;
            portf.eklenmeTarihi = pf.eklenmeTarihi;
            portf.userId = pf.userId;
           

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", pf.AnaKategoriId);

            ViewBag.AltKategoriId_ = new SelectList(db.AltKategoriEkles.Where(x => x.AnaKategoriId == pf.AnaKategoriId), "Id", "KategoriAdi", pf.AltKategoriId);

            return View(portf);
        }

        [HttpPost]
        public ActionResult Edit(Portfolyo_m pf, FormCollection frm)
        {
            Portfolyo portfolyo = new Portfolyo();

            if (pf.PostedFile == null)
                portfolyo.Fotograflar = pf.Fotograflar;
            else
            {


                var physicalPath_ = Path.Combine(Server.MapPath(portfolyo.Fotograflar));
                if (System.IO.File.Exists(physicalPath_))
                {
                    System.IO.File.Delete(physicalPath_);
                }


                var fileName = Path.GetFileName(pf.PostedFile.FileName);

                var physicalPath = Path.Combine(Server.MapPath("~/Content/uploads/Portfolyo/" + pf.userId + "/" + fileName));

                if (!Directory.Exists(Path.Combine(Server.MapPath("~/Content/uploads/Portfolyo/" + pf.userId + "/"))))
                {
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Content/uploads/Portfolyo/" + pf.userId + "/")));
                }

                pf.PostedFile.SaveAs(physicalPath);

                portfolyo.Fotograflar = "/Content/uploads/Portfolyo/" + pf.userId + "/" + fileName; ;

            }
            portfolyo.Id = Convert.ToInt32(frm["urunIds"]);
            portfolyo.port_Id = pf.port_Id;
            portfolyo.demoUrl = pf.demoUrl;
            portfolyo.baslik = pf.baslik;
            portfolyo.AnaKategoriId = pf.AnaKategoriId;
            portfolyo.AltKategoriId = pf.AltKategoriId;
            portfolyo.eklenmeTarihi = DateTime.Now;
            portfolyo.userId = pf.userId;

            if (ModelState.IsValid)
            {
                db.Entry(portfolyo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Portfolyo_m");
            }

            return RedirectToAction("Portfolyo_edit", new { Id = portfolyo.port_Id});
        }

        public ActionResult Delete(string Id)
        {
            Portfolyo portf = db.Portfolyos.Single(x => x.port_Id == Id);

            db.Portfolyos.Remove(portf);
            db.SaveChanges();

            var physicalPath = Path.Combine(Server.MapPath(portf.Fotograflar));
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            ViewBag.KategoriId = new SelectList(db.AnaKategorisEkles, "Id", "KategoriAdi", portf.AnaKategoriId);

            return View("Portfolyo_m");
        }

        public ActionResult Portfolyo_m_Liste()
        {
            string uid = User.Identity.GetUserId();

            return PartialView(db.Portfolyos.Where(x => x.userId == uid).ToList());
        }

        [Authorize]
        // GET: Portfolyo
        public ActionResult Portfolyom()
        {
           

            return View();
        }

        [HttpPost]
        public ActionResult ResimEkle_Metodu()
        {
            string uid = User.Identity.GetUserId();

            string isId;


                List<string> Liste = ResimEkle();

                var ii = new Portfolyo_stpr();

                if (Liste != null)
                {
                    ii = new Portfolyo_stpr()
                    {
                        userId = uid,
                        Fotograflar = Liste
                    };
                }
                else
                {
                    ii = new Portfolyo_stpr()
                    {
                        userId = uid,
                        Fotograflar = new List<string>()
                    };
                }



                var sql = db.Database.ExecuteSqlCommand("exec Is_Ilan_Fotograflari_Ekle @Fotograflar, @userId",
                    new SqlParameter("Fotograflar", ii.StrFotograflar),
                    new SqlParameter("userId", uid)
                    );

                //db.IsIlanlaris.Attach(ii);

                //db.Entry(ii).Property(x => x.Fotograflar).IsModified = true;
           
            return Json("1");
        }

        public List<string> ResimEkle()
        {
            List<string> Liste = new List<string>();

            string[] ss = System.Web.HttpContext.Current.Session["ListeEkle"] as string[];
            System.Web.HttpContext.Current.Session.Remove("ListeEkle");

            int vitrin = 0;
            string yol = "";
            Guid gd;
            int sayi = 0;
            string lstRes_ = "";

            string uid = User.Identity.GetUserId();

            if (!Directory.Exists(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid));
            }

            for (int i = 0; i < ss.Length; i++)
            {

                gd = Guid.NewGuid();

                if (ss[i].Contains("Vitrin"))
                {
                    vitrin = 1;
                    ss[i] = ss[i].Substring(ss[i].IndexOf("-") + 1);
                }

                else
                    vitrin = 0;




                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(ss[i])))
                {
                    using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                    {
                        if (!System.IO.File.Exists(Server.MapPath(yol + gd + ".jpg")))
                            bm2.Save(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg"), ImageFormat.Jpeg);
                    }
                }


                lstRes_ = "/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg";

                if (vitrin == 1)
                    Liste.Insert(0, lstRes_);

                else
                {
                    Liste.Insert(sayi, lstRes_);
                    sayi++;
                }


            }



            return Liste;
        }

        public List<string> ListeDuzenle;
        List<string> ListeSil;

        public List<string> ResimDuzenle()
        {
            ListeDuzenle = new List<string>();
            ListeSil = new List<string>();

            string[] ss = System.Web.HttpContext.Current.Session["ListeDuzenle"] as string[];
            System.Web.HttpContext.Current.Session.Remove("ListeDuzenle");

            int vitrin = 0;
            string yol = "";
            Guid gd;
            int sayi = 0;
            string lstRes_ = "";

            string uid = User.Identity.GetUserId();

            if (!Directory.Exists(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid));
            }

            for (int i = 0; i < ss.Length; i++)
            {

                string deger = "";

                gd = Guid.NewGuid();

                string name = ss[i].Substring(ss[i].LastIndexOf(",") + 1);

                string nameForGuid = ut.sonharfleriSil(name, 4);

                if (!System.IO.File.Exists(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + name)))
                {
                    gd = Guid.NewGuid();
                }
                else
                    gd = new Guid(nameForGuid);

                if (ss[i].Contains("Vitrin"))
                {
                    vitrin = 1;
                    ss[i] = ss[i].Substring(ss[i].IndexOf("-") + 1);
                }

                else
                    vitrin = 0;

                deger = ss[i].Substring(0, ss[i].LastIndexOf(","));

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Convert.FromBase64String(deger)))
                {
                    using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                    {
                        if (!System.IO.File.Exists(Server.MapPath(yol + gd + ".jpg")))
                            bm2.Save(Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg"), ImageFormat.Jpeg);
                    }
                }



                lstRes_ = "/Content/uploads/Is_Ilanlari/" + uid + "/" + gd + ".jpg";

                if (vitrin == 1)
                    ListeDuzenle.Insert(0, lstRes_);
                else
                {
                    ListeDuzenle.Insert(sayi, lstRes_);
                    sayi++;
                }

            }

            string[] silinecek = System.Web.HttpContext.Current.Session["ListeSil"] as string[];
            System.Web.HttpContext.Current.Session.Remove("ListeSil");

            if (silinecek.Length > -1)
            {
                for (int i = 0; i < silinecek.Length; i++)
                {
                    yol = Server.MapPath("/Content/uploads/Is_Ilanlari/" + uid + "/" + silinecek[i]);
                    if (System.IO.File.Exists(yol))
                        System.IO.File.Delete(yol);
                    string dbYol = "/Content/uploads/Is_Ilanlari/" + uid + "/" + silinecek[i];
                    ListeSil.Add(dbYol);
                }

            }


            return ListeDuzenle;
        }

        [HttpPost]
        public ActionResult ResimDuzenle_Metodu()
        {

            string uid = User.Identity.GetUserId();
                List<string> Liste = ResimDuzenle();

                var ii = new Portfolyo_stpr();

                if (Liste != null)
                {
                    ii = new Portfolyo_stpr()
                    {
                        userId = uid,
                        Fotograflar = Liste
                    };
                }
                else
                {
                    ii = new Portfolyo_stpr()
                    {
                        userId = uid,
                        Fotograflar = new List<string>()
                    };
                }

                var sql = db.Database.ExecuteSqlCommand("exec [Portfolyo_Duzenle] @Fotograflar, @userId, @SilinenFotograflar",
                    new SqlParameter("Fotograflar", ii.StrFotograflar),
                    new SqlParameter("SilinenFotograflar", ii.StrSilinenFotograflar),
                    new SqlParameter("userId", uid)
                    );

                //db.IsIlanlaris.Attach(ii);

                //db.Entry(ii).Property(x => x.Fotograflar).IsModified = true;

             

                System.Web.HttpContext.Current.Response.Cookies["RefreshFilter"].Expires = DateTime.Now.AddDays(-1);

            

            return Json("1");
        }

        public class FotolariGetir_Gonderilecek_Modeller
        {

            public string FotoPath { get; set; }

            public string ilanName { get; set; }
        }

        //[WebMethod]
        //[ScriptMethod]
        public string FotolariGetir()
        {
            string uid = User.Identity.GetUserId();

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            string s = "";

            var ilan = (from cek in db.Portfolyos
                        where cek.userId == uid
                        select new FotolariGetir_Gonderilecek_Modeller
                        {
                            FotoPath = cek.Fotograflar,
                            ilanName = cek.Fotograflar

                        }).ToList().Take(12).Select(x => new FotolariGetir_Gonderilecek_Modeller
                        {
                            FotoPath = x.FotoPath,
                            ilanName = x.ilanName.ToString().Substring(x.ilanName.ToString().LastIndexOf("/") + 1)

                        });

            s = serializer.Serialize(ilan);

            return serializer.Serialize(ilan);

        }
    }
}