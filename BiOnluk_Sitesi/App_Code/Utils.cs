using HemenTanitim.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Mvc;
using WebMatrix.Data;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Threading.Tasks;

namespace HemenTanitim.Models
{
    public class Utils
    {
            private HemenTanitimContext db = new HemenTanitimContext();

            public string sonharfleriSil(string value, int silinecekHarfSayisi)
            {
                string deger = string.Concat(value.Reverse().Skip(silinecekHarfSayisi).Reverse());

                return deger;
            }

            public async Task<bool> SendEMail(string subject, string body, string to)
            {
                var mailbilgileri = db.MailAyarlaris.Take(1).Single();
            
             
                MailMessage newMessage = new MailMessage(mailbilgileri.mailAdresi, to, subject, body.ToString());
                newMessage.IsBodyHtml = true;
                newMessage.From = new MailAddress("info@kemerburgazotolastik.com", "TYP FOOD DELIVERY");
                SmtpClient newSmtp = new SmtpClient(mailbilgileri.smptClient, mailbilgileri.port);
                newSmtp.Credentials = new System.Net.NetworkCredential(mailbilgileri.mailAdresi, mailbilgileri.mailSifresi);

                //newMessage.To.Add(new MailAddress(emailid));
                newSmtp.Send(newMessage);

                return true;

            }

            public string GenerateRandomPassword(int length)
            {
                string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
                char[] chars = new char[length];
                Random rd = new Random();
                for (int i = 0; i < length; i++)
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
                return new string(chars);
            }  

            public string GenerateSiparisId(int length)
            {
                string allowedChars = "0123456789";
                char[] chars = new char[length];
                Random rd = new Random();
                for (int i = 0; i < length; i++)
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
                return new string(chars);
            }

            public List<UserAndGroupAndPeople> KullaniciRolleriCek()
            {
                IEnumerable<UserAndGroupAndPeople> ak = new List<UserAndGroupAndPeople>();

                var db = Database.Open("DB150408011842Entities");

                var sql = db.Query("SELECT e.UserName,e.Id as UserId, e.Email, t.Name as Role FROM AspNetUsers e LEFT JOIN AspNetUserRoles et on e.Id = et.UserId LEFT JOIN AspNetRoles t on et.RoleId = t.Id").ToList();

                ak = sql.Select(x => new UserAndGroupAndPeople()
                {
                    Id = x.UserId,
                    UserName = x.UserName,
                    Role = x.Role,
                  
                    Email = x.Email
                });


                return ak.ToList();
            }

            public List<SelectListItem> RolListesi()
            {

                var rollerListesi = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

                return rollerListesi;

            }

            public List<UserViewModel> RolListesiCheck()
            {
                List<UserYetkiViewModel> uyvm = new List<UserYetkiViewModel>();

                var rollerListesi = db.Users.OrderBy(r => r.UserName).ToList().Select(rr => new UserViewModel { Id = rr.Id, Name = rr.UserName }).ToList();

                return rollerListesi;

            }

        public List<SelectListItem> HotelListesiCek()
        {

            var rollerListesi = db.Hotels.OrderBy(r => r.HotelAdi).ToList().Select(rr => new SelectListItem { Value = Convert.ToString(rr.Id), Text = rr.HotelAdi }).ToList();

            return rollerListesi;

        }

        public string ToSeoUrl_( string url)
            {

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace("ş", "s");
                url = url.Replace("Ş", "s");
                url = url.Replace("İ", "i");
                url = url.Replace("I", "ı");
                url = url.Replace("ö", "o");
                url = url.Replace("Ö", "o");
                url = url.Replace("ü", "u");
                url = url.Replace("Ü", "u");
                url = url.Replace("Ç", "");
                url = url.Replace("ç", "c");
                url = url.Replace("ğ", "g");
                url = url.Replace("Ğ", "g");
                url = url.Replace(" ", "-");
                url = url.Replace("---", "-");
                url = url.Replace("?", "");
                url = url.Replace("/", "");
                url = url.Replace(".", "");
                url = url.Replace("'", "");
                url = url.Replace("#", "");
                url = url.Replace("%", "");
                url = url.Replace("&", "");
                url = url.Replace("*", "");
                url = url.Replace("!", "");
                url = url.Replace("@", "");
                url = url.Replace("+", "");
                url = url.Replace(":", "");

                url = url.ToLower();
                url = url.Trim();

                // tüm harfleri küçült
                string encodedUrl = (url ?? "").ToLower();

                // & ile " " yer değiştirme
                encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

                // " " karakterlerini silme
                encodedUrl = encodedUrl.Replace("'", "");

                // geçersiz karakterleri sil
                //encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

                // tekrar edenleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

                // karakterlerin arasına tire koy
                encodedUrl = encodedUrl.Trim('-');

                return encodedUrl;
            }
            return "";
        }


        public string ToTemizle(string url)
        {

            if (!string.IsNullOrEmpty(url))
            {
           
               

                // tüm harfleri küçült
                string encodedUrl = (url ?? "");

               
                // geçersiz karakterleri sil
                encodedUrl = Regex.Replace(encodedUrl, @"[^\w\d]", " ");

                
                

                return encodedUrl;
            }
            return "";
        }

        public enum OdemeTipi
        {
            Kredi_Karti = 1, 
            Kapida_Odeme = 2
        }
        
        public enum SiparisDurumu
        {
            Hazirlaniyor = 1,
            Yola_cikti = 2,
            Teslim_Edildi = 3,
            Siparis_Iptal = 4
        }
    }


}