using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
	public class IsIlanlariFiyatlandirmaTipleri
	{
		[Key]
		public int Id { get; set; }

		[StringLength(20)]
		public string Tip { get; set; }
	}
    public class Slider
    {
        public string baslik { get; set; }

        public string fiyat { get; set; }

        public string Fotograflar { get; set; }

        public string ANKADI { get; set; }

        public string ALTKADI { get; set; }

        public int kisiSayisi { get; set; }

    }
    public class IsIlanlariGaleri
    {
        [Key]
        public int Id { get; set; }

        [StringLength(36)]
        public string IsIlanId { get; set; }

        [StringLength(36)]
        public string userId { get; set; }
       
        public string Fotograflar { get; set; }

        public string Video { get; set; }

        public string VideoDosyaAdi { get; set; }

    }

    public class IsIlanlariGaleri_stpr
	{
        [StringLength(36)]
        public string IsIlanId { get; set; }

        [StringLength(36)]
        public string userId { get; set; }

        public IsIlanlariGaleri_stpr()
        {
            Fotograflar = new List<string>();
            SilenenFotoğraflar = new List<string>();
        }
     
        public List<string> Fotograflar { get; set; }

        public List<string> SilenenFotoğraflar { get; set; }

        public string StrFotograflar
        {
            get
            {
                string strFotograflar = "";
                for (int i = 0; i < Fotograflar.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Fotograflar[i]))
                    {
                        strFotograflar += Fotograflar[i];
                        if (i != Fotograflar.Count - 1)
                            strFotograflar += ",";
                    }
                }
                return strFotograflar;
            }


        }

        public string StrSilinenFotograflar
        {
            get
            {
                string strSilinenFotograflar = "";
                for (int i = 0; i < SilenenFotoğraflar.Count; i++)
                {
                    if (!string.IsNullOrEmpty(SilenenFotoğraflar[i]))
                    {
                        strSilinenFotograflar += SilenenFotoğraflar[i];
                        if (i != SilenenFotoğraflar.Count - 1)
                            strSilinenFotograflar += ",";
                    }
                }
                return strSilinenFotograflar;
            }


        }

    }

	public class IsIlanlari
	{
        [Key]
        [StringLength(36)]
        public string isId { get; set; }
		public int AnaKategoriId { get; set; }
		public int AltKategoriId { get; set; }

		[Display(Name = "Tarih")]
		public DateTime Tarih { get; set; }

		[Display(Name = "Durum")]
		public bool durum { get; set; }

        [DefaultValue(true)]
        public bool adminOnay { get; set; }

        [Display(Name = "İlan Silinmesi")]
        public bool ilan_silinmesi { get; set; }
		public bool Trending { get; set; }  
        public bool Hot { get; set; }
        public bool Promoted { get; set; }

        [StringLength(36)]
		public string userId { get; set; }
		
		[StringLength(200)]
		[Display(Name = "Başlık")]
		public string baslik { get; set; }
        
		[StringLength(1500)]
		[Display(Name = "İlan Özellikleri")]        
		public string IsIlanAciklamasi { get; set; }
       
        public int SatisOrani { get; set; }


        //Siparişe Başlaman İçin Gerekenler
        //Bu alana bir sipariş aldığında, siparişe başlamak için ihtiyaç duyduğun bilgileri yazabilirsin.
        //Örneğin; bir çeviri hizmeti veriyorsan, bu alanda alıcıya çevrilmesi gereken metni iletmesini isteyebilirsin.

		[Display(Name = "Fiyat ")]
		public string fiyat { get; set; }
		public int kacKisilik { get; set; }
        [Display(Name = "Araç hacmi")]
		public int aracHacmi { get; set; }
        [StringLength(25)]
        public string TeslimSuresi { get; set; }
        public int indirimOrani { get; set; }
        public bool mostPopular { get; set; }
        public bool mostSales { get; set; }
        public bool Vitrin { get; set; }

    }

	public class store_procedure_IsIlanlariListesi
	{
       
        public string isId { get; set; }    

        public string baslik { get; set; }
        public string iban { get; set; }
        public bool durum { get; set; }
        public bool adminOnay { get; set; }
	    public bool ilan_silinmesi { get; set; }
        public bool oneCikarma { get; set; }
        public string Fotograflar { get; set; }
		public string KullaniciAdi { get; set; }
        public string kullaniciId { get; set; }
        public string ProfilFotografi { get; set; }
	}

    
}