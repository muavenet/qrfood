using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;
using System.IO;

namespace HemenTanitim.Controllers
{

    [Authorize(Roles = "Admin")]
   
    public class GenelBilgilersController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /GenelBilgilers/

        public ViewResult Index()
        {
            return View(context.GenelBilgilers.Take(1).Single());
        }

        //
        // GET: /GenelBilgilers/Details/5

        public ViewResult Details(int aid)
        {
            GenelBilgiler genelbilgiler = context.GenelBilgilers.Single(x => x.Id == aid);
            return View(genelbilgiler);
        }

        //
        // GET: /GenelBilgilers/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /GenelBilgilers/Create
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(GenelBilgiler genelbilgiler)
        {
            if (ModelState.IsValid)
            {
                context.GenelBilgilers.Add(genelbilgiler);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(genelbilgiler);
        }
        
        //
        // GET: /GenelBilgilers/Edit/5
 
        public ActionResult Edit(int aid)
        {
            GenelBilgiler genelbilgiler = context.GenelBilgilers.Single(x => x.Id == 1);
            return View(genelbilgiler);
        }

        //
        // POST: /GenelBilgilers/Edit/5
         [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(GenelBilgiler genelbilgiler, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {
                Guid gd = Guid.NewGuid();

                if (foto != null && foto.ContentLength > 0)
                {
                    string directory = Server.MapPath("/Content/uploads/Profil_fotograflari/");

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = System.IO.Path.GetExtension(foto.FileName).Substring(1);


                    foto.SaveAs(Path.Combine(directory, gd.ToString() + ".jpg"));

                    genelbilgiler.logo = "/Content/uploads/Profil_fotograflari/" + gd.ToString() + ".jpg";
                }

                else
                    genelbilgiler.logo = "/Content/uploads/Profil_fotograflari/" + genelbilgiler.logo;



                context.Entry(genelbilgiler).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Edit", new { aid = 1 });
            }
            return RedirectToAction("Edit", new { aid = 1 });
        }

        //
        // GET: /GenelBilgilers/Delete/5
 
        public ActionResult Delete(int aid)
        {
            GenelBilgiler genelbilgiler = context.GenelBilgilers.Single(x => x.Id == aid);
            return View(genelbilgiler);
        }

        //
        // POST: /GenelBilgilers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            GenelBilgiler genelbilgiler = context.GenelBilgilers.Single(x => x.Id == aid);
            context.GenelBilgilers.Remove(genelbilgiler);
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