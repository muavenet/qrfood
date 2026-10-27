using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using HemenTanitim.Models;
using System.Text;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using WebMatrix.Data;
using System.Web.Security;
using System.Text.RegularExpressions;
using MvcPaging;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;
using HemenTanitim.Helpers;
using System.IO;
using AutoMapper;
using HemenTanitim.Models.ViewModels;
using Microsoft.AspNet.SignalR;
using HemenTanitim.Hubs;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;

namespace HemenTanitim.Controllers
{
    [CustomAuthorize]
    public class HomeController :  Controller
    {
      

        //public readonly static List<Models.ViewModels.UserViewModel> _Connections = new List<Models.ViewModels.UserViewModel>();

        //private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
      
        HemenTanitimContext db = new HemenTanitimContext();

        Utils ut = new Utils();

        [HttpPost]
        public ActionResult Upload(FormCollection frm)
        {

            if (Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[0];

                    // Some basic checks...
                    if (file != null && !FileValidator.ValidSize(file.ContentLength))
                        return Json("Doysa büyüklüğü maksimum 500KB olmalıdır.");
                    else if (FileValidator.ValidType(file.ContentType))
                        return Json("Girdiğiniz dosya geçersizdir!");
                    else
                    {
                        // Save file to Disk
                        var fileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Content/uploads/"), fileName);
                        file.SaveAs(filePath);

                        string htmlImage = string.Format(
                            "<a href=\"/Content/uploads/{0}\" target=\"_blank\">" +
                            "<img width='200'  src=\"/Content/uploads/{0}\" class=\"post-image\">" +
                            "</a>", fileName);

                        using (var db = new HemenTanitimContext())
                        {
                            // Get sender & chat room
                            var senderViewModel = ChatHub._Connections.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
                            var sender = db.Users.Where(u => u.UserName == senderViewModel.Username).FirstOrDefault();
                            var receiver = db.Users.Where(u => u.UserName == senderViewModel.fromUser).FirstOrDefault();

                            // Build message
                            Message msg = new Message()
                            {
                                Content = Regex.Replace(htmlImage, @"(?i)<(?!img|a|/a|/img).*?>", String.Empty),
                                saat = DateTime.Now.ToShortTimeString(),
                                tarih = DateTime.Now.ToShortDateString(),
                                fromUser = sender.UserName,
                                //toUser = frm["receiver_userName"].ToString(),
                                //receiverId = frm["receiverId"].ToString(),
                                //New code
                                toUser = Request.Form["receiver_userName"].ToString(),
                                receiverId = Request.Form["receiverId"].ToString(),
                                senderId = sender.Id
                            };

                            db.Messages.Add(msg);
                            db.SaveChanges();

                            // Send image-message to group
                            //var messageViewModel = Mapper.Map<Message, MessageViewModel>(msg);
                            var hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                            hub.Clients.All.newMessage(msg);// Çalışmıyorrrrrrrr
                        }

                        return Json("1");
                    }

                }
                catch (Exception ex)
                { return Json("Yüklerken bir hata oluştu" + ex.Message); }
            }

            return Json("Dosya seçilmedi");

        }
        #region User Typing Notification
        [HttpPost]
        public string UserIsTyping(string userName)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(userName)) return result;
            var hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            hub.Clients.All.otherUserIsTyping(userName);
            return result;
        }
        [HttpPost]
        public string MarkMessageAsRead(string userId, int messageId)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(userId) || messageId == 0) return result;

            var message = db.Messages.Where(m => m.Id == messageId && m.receiverId == userId).FirstOrDefault();
            message.ReadOn = DateTime.Now;
            db.SaveChanges();

            return result;
        }
        #endregion
        public ActionResult Index()
        {

            var user = db.Users.Find(User.Identity.GetUserId());
            ApplicationUser ap = new ApplicationUser();

            ap.hotelAdi = user.hotelAdi;
            ap.odaNo = user.odaNo;

            //var tupleModel = new Tuple<List<VitrinUrunleri>, List<Fotolar>, List<Fotolar>>(VitrinUrunler(), slaytList(), slaytListKucukler());            

            return View(ap);
            
        }       
        public List<Fotolar> slaytList()
        {
            var res = db.Fotolars.Where(z => z.Slayt == true & z.KategoriSlayt_h248 == false & z.SlaytKucukAnasayfa1 == false & z.SlaytKucukAnasayfa2 == false & z.KategoriSlayt_h430 == false & z.BiziTercihEdenFirmalar == false).ToList();
            return res;
        }
        public List<Fotolar> slaytListKucukler()
        {
            var res = db.Fotolars.Where(z => z.SlaytKucukAnasayfa1 == true | z.SlaytKucukAnasayfa2 == true).ToList();
            return res;
        }
        public List<VitrinUrunleri> VitrinUrunler()
        { 
            var res = db.Urunlers          
             .Join(db.Fotolars, x => x.Guid , x=> x.UrunId , ( x , y ) => new { x , y} )
          
            .GroupBy(a=>a.x.Guid, (key,items)=> items.FirstOrDefault())
            .OrderByDescending(x => x.y.Id)
            .Select( a => new {
                
                      UrunAdi = a.x.UrunAdi,
                      ThumbnailYolu = a.y.ThumbnailYolu,
                      Id = a.x.UrunId,
                      Fiyat = a.x.Fiyat,
                      Vitrin = a.x.Trending,
                      SlaytResimMi = a.y.Slayt,
                      SlaytResimYol = a.y.ResimYolu
                     
                   })
                   .AsEnumerable().Select(x => new VitrinUrunleri
                   {
                       UrunAdi =x.UrunAdi,
                       ThumbYolu = x.ThumbnailYolu,
                       Id = x.Id,
                       Fiyat = x.Fiyat,
                       Vitrin = x.Vitrin,
                       SlaytResimYolu= x.SlaytResimYol,
                       SlaytResimMi = x.SlaytResimMi
                       

                   }).ToList();

            return res;
        }
        public ActionResult Hakkimizda()
        {
            return View(db.Hakkimizdas.Where(x => x.Id == 1).FirstOrDefault());
        }

        [ValidateInput(false)]
        [HttpGet]
        public ActionResult Iletisim(string Message)
        {
            if (Message != null)
            {
                if (Message == "0")
                {
                    ViewBag.Message = "Mail gönderilemedi.";
                }

                if (Message == "1")
                {
                    ViewBag.Message = "Mailiniz alınmıştır. Teşekkür ederiz.";
                }
            }

            else
            {
                ViewBag.Message = null;
            }

            return View(db.GenelBilgilers.Take(1).Single());
        }        
        [HttpPost]
        public ActionResult Iletisim(string ad, string soyad, string kimden, string konu, string body)
        {
            if (ModelState.IsValid)
            {
                var kime = db.MailAyarlaris.Select(s => new { kime = s.mailgelecekAdres }).Single();

                StringBuilder st = new StringBuilder();
                st.AppendLine("<table cellpadding='0' cellspacing='0' style='width:100%; border:1px solid #BCBCBC; border-radius:5px;'>");
                st.AppendLine("<tr><td style='background-color:#4585aa; color:white; padding-left:15px;'><h3>" + ad + " " + soyad + " yeni bir mail gönderdi.</h3></tr><td>");
                st.AppendLine("<tr><td style='color:black; padding-left:15px;'><br><br>Merhaba , " + ad + " " + soyad + "<br><br>Kimden : " + kimden + " <br><br> ");
                st.AppendLine(body.ToString() + "<br><br></td></tr>");

                ut.SendEMail(konu, st.ToString(), kime.kime);
               
                ModelState.Clear();

                try
                {
                    return RedirectToAction("Iletisim", "Home", new { Message = "1" });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Iletisim", "Home", new { Message = "0" });
                }
            }
            else
            {
                return View();
            }
        }    
        public ViewResult Karsilastirmalarim()
        {
            var sql = db.Database.SqlQuery<KarsilastirmaUrunlerFotolarla>("exec stpr_karsilastirmalarim  @UYEID", new SqlParameter("UYEID", User.Identity.GetUserId())).ToList();

            return View(sql.ToList());
        }
        public ActionResult KarsilastirmaEkle(int uid)
        {
            Karsilastirmalar fu = new Karsilastirmalar();

            fu.KullaniciId = User.Identity.GetUserId();
            fu.UrunId = uid;

            db.Karsilastirmalars.Add(fu);
            db.SaveChanges();

            return Json(true);
        }
        public ActionResult KarsilastirmaSil(int uid)
        {
            Karsilastirmalar fv = db.Karsilastirmalars.Find(uid);

            db.Karsilastirmalars.Remove(fv);
            db.SaveChanges();

            return RedirectToAction("Karsilastirmalarim");
        }
        public ActionResult _Sepet()
        {
            return PartialView();

        }
        public ActionResult GetMessages()
        {
            return View();
        }

       


        public ActionResult SepettenKaldir(int uid)
        {
            Sepet sp = db.Sepets.Find(uid);

            db.Sepets.Remove(sp);
            db.SaveChanges();

            return Json(true);
        }      
        public ActionResult _GenelBilgiler()
        {

            return PartialView(db.GenelBilgilers.Take(1).Single());

        }
        public ActionResult _GenelBilgilerTop()
        {
            return PartialView(db.GenelBilgilers.Take(1).Single());
        }
        public ActionResult _AnasayfaChat()
        {
            return PartialView();
        }
        public ActionResult YorumGonder(string adsoyad, string Aciklama, string isId)
        {

            Yorumlar yorumlar = new Yorumlar();

            yorumlar.kullaniciAdi = adsoyad;
            yorumlar.Aciklama = Aciklama;
            yorumlar.isId = isId;
            yorumlar.GirisTarihi = DateTime.Now;
            yorumlar.Onay = false;
            db.Yorumlars.Add(yorumlar);
            db.SaveChanges();

            return Json(true);

        }
        public ActionResult Blog(int? page)
        {
            int pageSize = 3;

            int pageNumber = (page ?? 1);

            return View(db.Blogs.OrderByDescending(x => x.Id).ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult BlogDetail(int id)
        {
            return View(db.Blogs.Where(x => x.Id == id).FirstOrDefault());
        }
        public ActionResult Haberler(int? Id, int? GaleriId)
        {
            if (Id != null)
            { 
                if(db.GaleriIcerikHaberis.Any(x => x.GaleriId == GaleriId & x.Id == Id))
                    return View(db.GaleriIcerikHaberis.Where(x => x.GaleriId == GaleriId & x.Id == Id).FirstOrDefault());
                else
                    return View(db.GaleriIcerikHaberis.Where(x => x.GaleriId == 1).Take(1).FirstOrDefault());
            }               
            else
                 return View(db.GaleriIcerikHaberis.Where(x => x.GaleriId == 1).Take(1).FirstOrDefault());
        
        }
    }
}
