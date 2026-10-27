using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FooterController : Controller
    {
        private HemenTanitimContext context = new HemenTanitimContext();    
        //
        // GET: /Footer/
        public ActionResult Index()
        {
            return View(context.Footers.Take(1).Single());
        }
        public ActionResult Edit(int aid)
        {
            Footer footer = context.Footers.Single(x => x.Id == aid);
            return View(footer);
        }

        [HttpPost]
        public ActionResult Edit(Footer footer)
        {
            context.Entry(footer).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

	}
}