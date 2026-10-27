
using HemenTanitim.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HemenTanitim.Models
{
    public class ApplicationUser : IdentityUser
    {

        //CHAT İLE İLGİLİ KISIM


        public string Avatar { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        //CHAT İLE İLGİLİ KISIM
   
      
        public decimal puan_ortalamasi { get; set; }

        public Int16? Konum { get; set; }

		public DateTime SonGorulmeTarihi { get; set; }

		public DateTime UyelikTarihi { get; set; }

		public string Fotograf{ get; set; }

		public string OrtalamaYanitSuresi { get; set; }

        public int YorumSayisi { get; set; } 
	
		public int OncekiSiparisler { get; set; }
		
		public int IptalEdilenler { get; set; }

        public string Hakkimda { get; set; }

        public string SozlesmeIcerigi { get; set; }

        public bool HotelMi { get; set; }

        [StringLength(75)]
        public string Email { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool ConfirmedPhoneNumber { get; set; }
        public string adres { get; set; }
        public string ad { get; internal set; }
        public string soyad { get; internal set; }
        public string iban { get; internal set; }
        public string tckimlik { get; internal set; }
        public string hotelAdi { get; set; }
        public int hotelId { get; set; }
        public string odaGuidId { get; set; }
        public string odaNo { get; set; }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager() : base(new UserStore<ApplicationUser>(new HemenTanitimContext()))
        {
            PasswordValidator = new MinimumLengthValidator(4);
        }
    }


    public class HemenTanitimContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Message> Messages { get; set; }

        //public HemenTanitimContext()
        //    : base("DefaultConnection", throwIfV1Schema: false)
        //{
        //}


        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<eTicaret.Models.eTicaretContext>());

        public HemenTanitimContext()
            : base("DB150408011842Entities", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static HemenTanitimContext Create()
        {
            return new HemenTanitimContext();
        }

        public DbSet<AltKategoriEkle> AltKategoriEkles { get; set; }
        public DbSet<AnaKategoriler> AnaKategorisEkles { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<AltKategoriEkle>();
            modelBuilder.Entity<AnaKategoriler>();

            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Urunler> Urunlers { get; set; }
        public DbSet<Portfolyo> Portfolyos { get; set; }
        public DbSet<Fotolar> Fotolars { get; set; }
        public DbSet<Yapilacaklar> Yapilacaklars { get; set; }
        public DbSet<SiparisBilgileri> SiparisBilgileris { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<Yorumlar> Yorumlars { get; set; }
        public DbSet<Sepet> Sepets { get; set; }
        public DbSet<Il> iller { get; set; }
        public DbSet<Ilce> ilceler { get; set; }
        public DbSet<mahalle> mahalleler { get; set; }
        public DbSet<Semt> Semtler { get; set; }
        public DbSet<PostaKodu> PostaKodlari { get; set; }
        public DbSet<Kargo> Kargo { get; set; }
        public DbSet<KargoSirketleri> KargoSirketleris { get; set; }
        public DbSet<FavoriUrunler> FavoriUrunlers { get; set; }
        public DbSet<Karsilastirmalar> Karsilastirmalars { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<MesajKaydetme> MesajKaydetmes { get; set; }
        public DbSet<ZiyaretciSayaci> ZiyaretciSayaci { get; set; }
        public DbSet<GenelBilgiler> GenelBilgilers { get; set; }
        public DbSet<Havale> Havales { get; set; }
        public DbSet<Hakkimizda> Hakkimizdas { get; set; }
        public DbSet<MailAyarlari> MailAyarlaris { get; set; }
        public DbSet<DashboardSiralama> DashboardSiralamas { get; set; }
        public DbSet<SiteGorunum> SiteGorunums { get; set; }
        public DbSet<Footer> Footers { get; set; }
        public DbSet<Mesajlar> Mesajlars { get; set; }
        public DbSet<SiparisIptal> SiparisIptals { get; set; }
        public DbSet<MusteriSiteleri> MusteriSiteleris { get; set; }
		public DbSet<IsIlanlari> IsIlanlaris { get; set; }
		public DbSet<IsIlanlariGaleri> IsIlanlariGaleris { get; set; }
	    public DbSet<AnahtarKelimeler> AnahtarKelimelers { get; set; }
		public DbSet<HemenTanitim.Models.BankaHesaplari> BankaHesaplaris { get; set; }
        public DbSet<Fiyatlandirma> Fiyatlandirmas { get; set; }
        public DbSet<GaleriBaslik> GaleriBasliks { get; set; }
        public DbSet<GaleriIcerikHaberi> GaleriIcerikHaberis { get; set; }
        public DbSet<Hotels> Hotels { get; set; }
        public DbSet<Odalar> Odalars { get; set; }

    }
}