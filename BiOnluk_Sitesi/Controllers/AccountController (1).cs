using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using HemenTanitim.Models;
using System.Web.Security;
using System.Configuration;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Data.Entity;

namespace HemenTanitim.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        HemenTanitimContext context = new HemenTanitimContext();

		public class CaptchaResponse
		{
			[JsonProperty("success")]
			public bool Success { get; set; }

			[JsonProperty("error-codes")]
			public List<string> ErrorCodes { get; set; }
		}


		Utils bs = new Utils();
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new HemenTanitimContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            userManager.PasswordValidator = new PasswordValidator { RequiredLength = 4 };
            UserManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }


        [AllowAnonymous]
        public ActionResult Login(int hotelId, int odaId)
        {
            LoginViewModel model = new LoginViewModel();
            model.odaId = odaId;
            model.hotelId = hotelId;
            return View(model);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Verification(string email, int hotelId, int odaId)
        {
            LoginViewModel model = new LoginViewModel();
            model.UserName = email;
            model.hotelId = hotelId;
            model.odaId = odaId;
            model.odaNo = context.Odalars.Where(x => x.Id == odaId && x.HotelId == hotelId).FirstOrDefault().OdaNo;
            model.hotelAdi = context.Hotels.Where(x => x.Id == hotelId).FirstOrDefault().HotelAdi;
            model.Email = email;

            return View(model);
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
      
        public async Task<ActionResult> Verification(LoginViewModel model)
        {

            //var response = Request["g-recaptcha-response"];
            //const string secret = "6LeGoIQUAAAAACEDSPfhWyjuITP1hkM4_kEL4NA5";
            ////Kendi Secret keyinizle değiştirin.

            //var client = new WebClient();
            //var reply =
            //	client.DownloadString(
            //		string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            //var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //var recaptchaHelper = this.GetRecaptchaVerificationHelper();

            //if (String.IsNullOrEmpty(recaptchaHelper.Response))
            //{
            //	TempData["Hata"] = "Doğrulama kodunu giriniz.";
            //	return RedirectToAction("Login", new { returnUrl = returnUrl });
            //}

            //var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

            //if (recaptchaResult != RecaptchaVerificationResult.Success)
            //{
            //	TempData["Hata"] = "Doğrulama kodunu yanlış girdiniz.";
            //	return RedirectToAction("Login", new { returnUrl = returnUrl });
            //}


            //if (!captchaResponse.Success)
            //{
            //	TempData["Message"] = "Lütfen 456trdoğrulayınız.";
            //	return RedirectToAction("Login", new { returnUrl = returnUrl });
            //}

                
            //return RedirectToAction("Index");
          
                if (ModelState.IsValid)
                {

                    //ApplicationUser user = context.Users.Where(x => x.UserName == model.UserName & x.PasswordHash == hashedNewPassword).
                    //    FirstOrDefault();
                    string password = model.Password1 + model.Password2 + model.Password3 + model.Password4;

                    var user = await UserManager.FindAsync(model.UserName, password);

                    if (user != null)
                    {
                        user.odaId = model.odaId;
                        user.hotelId = model.hotelId;
                        user.hotelAdi = model.hotelAdi;
                        user.odaNo = model.odaNo;
                        user.ConfirmedPhoneNumber = true;
                        user.SonGorulmeTarihi = DateTime.Now;

                        await UserManager.UpdateAsync(user);

                        await SignInAsync(user, false);

                        return Redirect("~");
                    }

                    else
                    {
                        TempData["Hata"] = "* Code is mistaken.";
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
          
            return RedirectToAction("Login");       
        }


        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email; return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            ApplicationUser user = this.UserManager.FindById(Token);
            if (user != null)
            {
                if (user.Email == Email)
                {
                    await UserManager.UpdateAsync(user);
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home", new { ConfirmedEmail = user.Email });
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }

        } 

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        //[Authorize(Roles = "Admin")] 
  

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(string telefon, string email)
        {
          
            if (!String.IsNullOrEmpty(email))
            {
                try
                {
                    Utils ut = new Utils();
                    var phoneresult = context.Users.Any(z => z.UserName == email);

                    Random generator = new Random();
                    String generatedCode = generator.Next(0, 10000).ToString("D4");
                    String hashedNewPassword = UserManager.PasswordHasher.HashPassword(generatedCode);

                    if (phoneresult == false)
                    {

                      
                        await ut.SendEMail("Verification Code", "<h1><strong>Enter the code below to login.</strong></h1> <br /><h2>" + generatedCode + "</h2>", email);

                        var user = new ApplicationUser()
                        {
                            Telefon  = "",
                            Email = email,
                            UserName = email,
                            Id = Guid.NewGuid().ToString(),                            
                            UyelikTarihi = DateTime.Now,
                            SonGorulmeTarihi = DateTime.Now,
                            Fotograf = "/Content/uploads/c3b50e90e2d1b6467e4bf111551b61c5.png",
                            Konum = 34,
                            ConfirmedPhoneNumber = false,
                            PasswordHash = hashedNewPassword
                        };

                        var result = await UserManager.CreateAsync(user, generatedCode);
                        if (result.Succeeded)
                        {
                        }
                        else
                        {
                            AddErrors(result);

                        }
                        //if (result.Succeeded)
                        //{
                        //    Utils bc = new Utils();

                        //    bc.SendEMail("E-mail Aktivasyonu", string.Format("Sayın {0}<BR/>Sistemimize kayıt olduğunuz için teşekkür ederiz. Lütfen kaydınızı tamamlamak için aşağıdaki aktivasyon linkine tıklayınız: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme)), user.Email);

                        //    return RedirectToAction("Confirm", "Account", new { Email = user.Email });

                        //    //await SignInAsync(user, isPersistent: false);
                        //    //return RedirectToAction("Index", "Home");
                        //}
                        //else
                        //{
                        //    TempData["HataRegister"] = ((string[])(result.Errors))[0];
                        //}

                    }
                    else
                    {
                        await ut.SendEMail("Verification Code", "<h1><strong>Enter the code below to login.</strong></h1> <br /><h2>" + generatedCode + "</h2>", email);

                        ApplicationUser cUser = await UserManager.FindByNameAsync(email);
                        cUser.PasswordHash = hashedNewPassword;
                        cUser.ConfirmedPhoneNumber = false;
                        cUser.SonGorulmeTarihi = DateTime.Now;
                        var sonuc1 = await UserManager.UpdateAsync(cUser);

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                 
                }
                return Json(true);
            }
            else {
                return Json(false);
            }

            // If we got this far, something failed, redisplay form
       
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
           string uid = User.Identity.GetUserId();



             

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Şifreniz değiştirildi.."
                : message == ManageMessageId.SetPasswordSuccess ? "Şifre oluşturuldu."
                : message == ManageMessageId.RemoveLoginSuccess ? "Geçerli şifre silindi.."
                : message == ManageMessageId.Error ? "Bir hata meydana geldi."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            string uid = User.Identity.GetUserId();
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(uid, model.OldPassword, model.NewPassword);

                  
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];

                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }


                var user = new ApplicationUser() 
                { 
                    UserName = model.UserName, Email = model.Email 
                };

                var emailresult = UserManager.FindByEmail(model.Email);

                if (emailresult != null)
                {
                    TempData["hata"] = "Email adresi kullanılıyor";
                    return RedirectToAction("ExternalLoginCallback", new { returnUrl = returnUrl });
                }

                var result = await UserManager.CreateAsync(user);


                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);

                foreach (var item in result.Errors)
                {
                    TempData["hata"] = item;
                }

                
            }
          
            ViewBag.ReturnUrl = returnUrl;
            return RedirectToAction("ExternalLoginCallback", new {returnUrl =returnUrl });
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", new { returnUrl =  Request.UrlReferrer});
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string UserName)
        {
            //check user existance
            var user = UserManager.FindByName(UserName);
            if (user == null || user.Email == null)
            {
                TempData["Message"] = "Böyle bir kullanıcı yok veya email adresi tanımlı değildi.";
                return RedirectToAction("ForgotPassword");
            }
            else
            {
                //generate password token

                //create url with above token
                var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = UserName, rt = user.Id }, "http") + "'>Reset Password</a>";
                //get user emailid

                var emailid = (from i in context.Users
                               where i.UserName == UserName
                               select i.Email).FirstOrDefault();
                //send mail
                string subject = "Şifre yenileme";
                string body = "<b>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</b><br/>" + resetLink; //edit it
                try
                {
                    bs.SendEMail(subject, body, emailid);
                    TempData["Message"] = "Mail adresinize şifrenizi yenilemek için bir link gönderildi.";
                    
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Mail gönderilirken bir hata meydana geldi. Hata mesajı : " + ex.Message;
                 
                }

            }
            return RedirectToAction("ForgotPassword");
           
        }

   


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string un, string rt, ManageUserViewModel model)
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
            String userId = rt;
            String newPassword = model.NewPassword;
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            ApplicationUser cUser = await store.FindByIdAsync(userId);
            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            await store.UpdateAsync(cUser);

            await SignInAsync(cUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            BilgilerGuncellendi,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}