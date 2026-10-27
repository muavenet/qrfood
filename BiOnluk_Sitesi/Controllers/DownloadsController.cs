using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class DownloadsController : Controller
    {       
        //public FileResult Download(string dosyaAdi)
        //{
        //    if (System.IO.File.Exists(Server.MapPath("~/Content/Dosyalar/" + dosyaAdi)))
        //    {
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Dosyalar/" + dosyaAdi));
        //        string fileName = dosyaAdi;
        //        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //    }
        //    else
        //        return null;
        //}

        [HttpPost]
        public ActionResult ajaxDosyaKontrol(string dosyaAdi)
        {
            if (System.IO.File.Exists(Server.MapPath("~/Content/Dosyalar/" + dosyaAdi)))
            {
                return Json(dosyaAdi);
            }
            else
                return Json(false);

            
        } 
        
        
        public FileResult DownloadFile(string dosyaAdi)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Dosyalar/" + dosyaAdi));
            string fileName = dosyaAdi;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            
        }
	}
}