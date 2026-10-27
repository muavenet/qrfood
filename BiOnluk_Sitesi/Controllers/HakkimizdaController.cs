using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paket1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HakkimizdaController : Controller
    {
        //
        // GET: /Hakkimizda/
        private HemenTanitimContext context = new HemenTanitimContext();
        //
        // GET: /Footer/
        public ActionResult Index()
        {
            return View(context.Hakkimizdas.Take(1).Single());
        }

        [ValidateInput(false)]
        public ActionResult Edit(int aid)
        {
            Hakkimizda hakkimizda = context.Hakkimizdas.Single(x => x.Id == aid);
            return View(hakkimizda);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Hakkimizda hakkimizda)
        {
            context.Entry(hakkimizda).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Index");
        }
	}
}