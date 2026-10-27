using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HemenTanitim.Controllers
{
    public class TelerikController : Controller
    {
        // GET: Telerik
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult Basic_Usage_Submit(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                TempData["UploadedFiles"] = Basic_Usage_Get_File_Info(files);

                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/uploads/Videolar/"), fileName);

                    // The files are not actually saved in this demo
                   file.SaveAs(physicalPath);
                }
            }

            return RedirectToAction("Result");
        }

     
        public ActionResult Result()
        {
            return View();
        }

        private IEnumerable<string> Basic_Usage_Get_File_Info(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }

        public ActionResult CustomDropZone()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ResimEkle_Metodu()
        {

            List<string> Liste = ResimEkle();

            //Ilan ilan = new Ilan();

            //if (Liste != null) ilan.Fotograflar = Liste; else ilan.Fotograflar = new List<string>();

            return PartialView();
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

            int kId = 1;

            if (!Directory.Exists(Server.MapPath("/Content/uploads/Is_Ilanlari/" + kId )))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/Content/uploads/Is_Ilanlari/" + kId ));
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
                            bm2.Save(Server.MapPath("/Content/uploads/Is_Ilanlari/" + kId + "/" + gd + ".jpg"), ImageFormat.Jpeg);
                    }
                }


                lstRes_ = "Content/Images/IlanResim/" + gd + ".jpg";

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
    }
}
