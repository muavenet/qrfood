using ImageResizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    public class FileUploderController : Controller
    {
        //
        // GET: /FileUploder/
        public ActionResult Index()
        {
            //Instructions resizeCropSettings = new Instructions("width=100&height=90&format=jpg");

            //string yol = Server.MapPath(Request.ApplicationPath + "Content/uploads/");

            //ImageBuilder.Current.Build(yol + st.SavedFileName, st.SavedFileName, resizeCropSettings);


            //Loop through each uploaded file
            foreach (string fileKey in System.Web.HttpContext.Current.Request.Files.Keys)
            {
                HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[fileKey];
                if (file.ContentLength <= 0) continue; //Skip unused file controls.

                //The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
                //Destination paths can have variables like <guid> and <ext>, or 
                //even a santizied version of the original filename, like <filename:A-Za-z0-9>
                ImageResizer.ImageJob i = new ImageResizer.ImageJob(file, "~/uploads/<guid>.<ext>", new ImageResizer.ResizeSettings(
                            "width=2000;height=2000;format=jpg;mode=max"));
                i.CreateParentDirectory = true; //Auto-create the uploads directory.
                i.Build();
            }

            return View();
        }
	}
}