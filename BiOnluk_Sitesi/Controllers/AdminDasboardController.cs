using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using WebMatrix.Data;
using System.Threading.Tasks;

namespace HemenTanitim.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminDasboardController : Controller
    {
        HemenTanitimContext db = new HemenTanitimContext();
        HemenTanitim.Models.Utils ut = new Models.Utils();
        //
        // GET: /AdminDasboard/
        public ActionResult Index()
        {
            

            //return View(context.DashboardSiralamas.OrderBy(x => x.gosterimSiralamasi).ToList());

            return View();
        }

        public ActionResult _QuickEmail()
        {
            return PartialView();
        }

        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> _QuickEmail(FormCollection frm)
        {
            var userlist =  db.Users.Select(x => x.Email).ToList();

            foreach (var item in userlist)
            {
                if (!string.IsNullOrEmpty(item))
                 ut.SendEMail(frm["subject"], frm["message"], item);
            }

            TempData["MailGonderildi"] = "* Mailler başarıyla gönderildi";

            

            return  RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult _Chat()
        {
            IEnumerable<MesajKaydetme> modelList = new List<MesajKaydetme>();

            modelList = db.MesajKaydetmes.OrderByDescending(x => x.Id).Take(300).OrderBy(x => x.Id).ToList();

            return PartialView(modelList.ToList());
        }

       
        public void dasboardStilDegistir(string ifade)
        {
            SiteGorunum st = db.SiteGorunums.Find(1);

            st.className = ifade;

            db.Entry(st).State = EntityState.Modified;

            db.SaveChanges();
        }

        public JsonResult dasboardSiralamDegistir(int[] Ids)
        {
            int[] ids = Ids;

            //foreach (var id in ids)
            //{
            //    DashboardSiralama ds = db.DashboardSiralamas.Find(id);

            //    ds.gosterimSiralamasi = Array.FindIndex(ids, i => i == id);

            //    db.Entry(ds).State = EntityState.Modified;

            //    db.SaveChanges();
            //}

            return Json(true);
        }


        [HttpGet]
        public ActionResult _Yapilacaklar(int? page)
        {
            IEnumerable<Yapilacaklar> modelList = new List<Yapilacaklar>();

            modelList = db.Yapilacaklars.Take(300).OrderByDescending(x => x.durum == false).ThenBy(n => n.goruntulemeSirasi).ToList();

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

            return PartialView(modelList.ToPagedList(pageNumber, 6));
        }

        public JsonResult yapilacakSiralamDegistir(int[] Ids)
        {
            int[] ids = Ids;

            foreach (var id in ids)
            {
                Yapilacaklar yp = db.Yapilacaklars.Find(id);

                yp.goruntulemeSirasi = Array.FindIndex(ids, i => i == id);

                db.Entry(yp).State = EntityState.Modified;

                db.SaveChanges();
            }
            return Json(true);
        }
        public ActionResult GorevEkle(string yapilacak, string tarih, int? id)
        {
            Yapilacaklar modelList;

            DateTime dt = Convert.ToDateTime(tarih);

            if (id > 0)
            {
                modelList = db.Yapilacaklars.Find(id);
                modelList.tarih = dt;
                modelList.yapilacak = yapilacak;
                db.Entry(modelList).State = EntityState.Modified;
            }
            else
            {
                modelList = new Yapilacaklar();
                modelList.tarih = dt;
                modelList.yapilacak = yapilacak;
                db.Yapilacaklars.Add(modelList);
            }                

            db.SaveChanges();

            TempData["YapilacaklarEklendi"] = "* Görev eklendi";

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "AdminDasboard");

            return Json(new { Url = redirectUrl });
        }
        public ActionResult GorevTamamlandi(int Id, bool durum)
        {


            Yapilacaklar modelList = db.Yapilacaklars.Find(Id);

            modelList.durum = durum;

            db.Entry(modelList).State = EntityState.Modified;

            db.SaveChanges();
            
            return Json(true);
        }
        public ActionResult GorevSil(int id)
        {
            Yapilacaklar yp = db.Yapilacaklars.Find(id);

            db.Yapilacaklars.Remove(yp);
            db.SaveChanges();

            TempData["YapilacaklarEklendi"] = "*** Görev Silindi";

            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "AdminDasboard");

            return Json(new { Url = redirectUrl });
        }

        public JsonResult MesajKayitAltinaAl(string name, string message)
        {
            MesajKaydetme msg = new MesajKaydetme();

            msg.tarih = DateTime.Now.ToLongDateString() +" "+ DateTime.Now.ToShortTimeString();
            msg.mesaj = message;
            msg.userName = name;

            db.MesajKaydetmes.Add(msg);
            db.SaveChanges();


            return Json(true);
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult _AylikSatisGrafigi()
        {
           return PartialView();
        }

          [OutputCache(Duration = 86400, VaryByParam = "none")]
        public JsonResult AylikSatisGrafigi()
        {
            var sql = db.Database.SqlQuery<AylikSatisOranlari>("exec stpr_AylikSatisOranlari").ToList();

            return Json(sql, JsonRequestBehavior.AllowGet);
        }

      [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult _GunlukSatisGrafigi()
        {           
            return PartialView();
        }

       [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult GunlukSatisGrafigi()
        {
            var sql = db.Database.SqlQuery<GunlukSatisOranlari>("exec stpr_gunluksatis").ToList();

            return Json(sql, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult _ZiyaretciHaritasi()
        {
            return PartialView();
        }

          [OutputCache(Duration = 604800, VaryByParam = "none")]
        public JsonResult ZiyaretciHaritasiGetir()
        {
            IEnumerable<ZiyaretciSayaci> ak = new List<ZiyaretciSayaci>();

            var dbs = WebMatrix.Data.Database.Open("DB150408011842Entities");

            var sql = dbs.Query("select ilKodu,ziyaretciSayisi from ZiyaretciSayaci").ToList();

            ak = sql.Select(x => new ZiyaretciSayaci()
            {
                ilKodu = x.ilKodu,
                ziyaretciSayisi = x.ziyaretciSayisi
            });

            return Json(ak, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ZiyaretciSayisiArtir(string ilAdi)
        {
            if (ModelState.IsValid)
            {
                ZiyaretciSayaci zs = db.ZiyaretciSayaci.SingleOrDefault(x=> x.ilAdi == ilAdi);

                int? ziyaretci = zs.ziyaretciSayisi;

                ziyaretci++;

                zs.ziyaretciSayisi = ziyaretci;

                db.Entry(zs).State = EntityState.Modified;

                db.SaveChanges();
            }

            return Json("true");
        }
        public ActionResult _UstMenu()
        {
            var sql = db.Database.SqlQuery<UstMenuModel>("exec stpr_ustmenu").ToList();

            return PartialView(sql.ToList());
        }

        public ActionResult Test()
        {
            return View();
        }

      
    }
}