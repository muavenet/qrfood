using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HemenTanitim.Models;
using Microsoft.AspNet.Identity;
using System.IO;
namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogsController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();

        //
        // GET: /Blogs/

        public ViewResult Index()
        {
            return View(context.Blogs.ToList());
        }

        //
        // GET: /Blogs/Details/5

        public ViewResult Details(int aid)
        {
            Blog blog = context.Blogs.Single(x => x.Id == aid);
            return View(blog);
        }

        //
        // GET: /Blogs/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Blogs/Create
         [ValidateInput(false)]
         [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Blog blog, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {
                Guid gd = Guid.NewGuid();

                foto = Request.Files["foto"];

                if (foto != null && foto.ContentLength > 0)
                {
                    string directory = Server.MapPath("~/Content/uploads/");

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = System.IO.Path.GetExtension(foto.FileName).Substring(1);

                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                        return View();
                    }
                    foto.SaveAs(Path.Combine(directory, gd.ToString() + ".jpg"));

                    blog.Resim = gd.ToString() + ".jpg";

                }
                else
                    blog.Resim = gd.ToString();


                blog.Yazan = User.Identity.GetUserName();
                context.Blogs.Add(blog);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(blog);
        }
        
        //
        // GET: /Blogs/Edit/5
 
        public ActionResult Edit(int aid)
        {
            Blog blog = context.Blogs.Single(x => x.Id == aid);
            return View(blog);
        }

        //
        // POST: /Blogs/Edit/5
         [ValidateInput(false)]
         [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(Blog blog, HttpPostedFileBase foto)
        {
            if (ModelState.IsValid)
            {

                Guid gd = Guid.NewGuid();

                if (foto != null && foto.ContentLength > 0)
                {
                    string directory = Server.MapPath("~/Content/uploads/");

                    var supportedTypes = new[] { "jpg", "jpeg", "png" };

                    var fileExt = System.IO.Path.GetExtension(foto.FileName).Substring(1);

                  
                    foto.SaveAs(Path.Combine(directory, gd.ToString() + ".jpg"));

                    blog.Resim = gd.ToString() + ".jpg";
                }

                else
                    blog.Resim = blog.Resim;

                blog.Yazan = User.Identity.GetUserName();
                context.Entry(blog).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        //
        // GET: /Blogs/Delete/5
 
        public ActionResult Delete(int aid)
        {
            Blog blog = context.Blogs.Single(x => x.Id == aid);
            return View(blog);
        }

        //
        // POST: /Blogs/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int aid)
        {
            Blog blog = context.Blogs.Single(x => x.Id == aid);
            context.Blogs.Remove(blog);
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