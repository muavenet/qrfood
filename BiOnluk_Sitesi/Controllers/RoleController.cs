using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HemenTanitim.Models;
using System.Web.Security;
using WebMatrix.Data;
using HemenTanitim.Controllers;


namespace OrnekCalisma.Controllers
{
   //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {       
        HemenTanitimContext context = new HemenTanitimContext();    

        Utils bc = new Utils();
        
        // GET: /Role/
        public ActionResult Index()
        {
            
            var roles = context.Roles.ToList();

            return View(roles);
        }

        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                
                ViewBag.ResultMessage = "Yetki tanımlandı";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            
            context.Roles.Remove(thisRole);
            
            context.SaveChanges();          

            return RedirectToAction("Index");
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
       
        public ActionResult ManageUserRoles(string UserName)
        {



            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var account = new AccountController();

                IList<string> ss = account.UserManager.GetRoles(user.Id);

                ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);

                ViewBag.hotelAdi = context.Hotels.Where(x => x.Id == user.hotelId).FirstOrDefault().HotelAdi;

                //var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

                RolListveCheckList();

                return View("ManageUserRoles", new UserYetkiViewModel { UserViewModels = ViewBag.CheckBoxList });
            }
            else
            {
                RolListveCheckList();

                return View("ManageUserRoles", new UserYetkiViewModel { UserViewModels = ViewBag.CheckBoxList });
            }

            // prepopulat roles for the view dropdown
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName, int Hotels)
        {

            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var account = new AccountController();
                account.UserManager.AddToRole(user.Id, RoleName);

            user.hotelId = Hotels;

            context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();


            ViewBag.ResultMessage = "Rol&Hotel tanımlandı !";

            RolListveCheckList();

            return View("ManageUserRoles", new UserYetkiViewModel { UserViewModels = ViewBag.CheckBoxList });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var account = new AccountController();

                IList<string> ss = account.UserManager.GetRoles(user.Id);

                ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);

                RolListveCheckList();
            }

            return View("ManageUserRoles", new UserYetkiViewModel { UserViewModels = ViewBag.CheckBoxList });
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var account = new AccountController();

            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (account.UserManager.IsInRole(user.Id, RoleName))
            {
                account.UserManager.RemoveFromRole(user.Id, RoleName);

                ViewBag.ResultMessage = "Seçilen yetki kullanıcıdan alındı !";
            }
            else
            {
                ViewBag.ResultMessage = "Seçilen yetki kullanıcıya ait değil !";
            }

            RolListveCheckList();

            return View("ManageUserRoles", new UserYetkiViewModel { UserViewModels = ViewBag.CheckBoxList });
        }

        public void RolListveCheckList()
        {
                ViewBag.Roles = bc.RolListesi();

                ViewBag.CheckBoxList = bc.RolListesiCheck();

             ViewBag.Hotels = bc.HotelListesiCek();
        }


        
        public ActionResult UserList()
        {
          
            IEnumerable<UserAndGroupAndPeople> ak = bc.KullaniciRolleriCek();           
         
            return View(ak);
        }

        public ActionResult MailGonder(string mailadresi)
        {

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult MailGonder(FormCollection frm)
        {
            if (!string.IsNullOrEmpty(frm["hdnMailAdresi"]))
                bc.SendEMail(frm["subject"], frm["message"], frm["hdnMailAdresi"]);
           
            TempData["MailGonderildi"] = "* Mailler başarıyla gönderildi";



            return RedirectToAction("UserList");
        }

	}
}