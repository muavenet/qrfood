using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class GenelBilgiler
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        [Display(Name = "Title")]
        public string title { get; set; }

        [StringLength(150)]
        [Display(Name = "Meta description")]
        public string metaDescription { get; set; }
      
        [Display(Name = "Meta keywords")]
        public string metaKeywords { get; set; }

        [Display(Name="Twitter kullanıcı adınız")]
        public string TwitterKullaniciAdi { get; set; }


        [StringLength(150)]
        [Display(Name = "Firma Resmi adı")]
        public string firmaResmiAdi { get; set; }

        [StringLength(150)]
        [Display(Name = "Firma adı")]
        public string firmaAdi { get; set; }

        [StringLength(150)]
        [Display(Name = "Yetkili kişi adı")]
        public string yetkiliAdi { get; set; }

        [StringLength(25)]
        [Display(Name = "E-mail")]
        public string mail { get; set; }

        [StringLength(25)]
        [Display(Name = "E-mail 2")]
        public string mail2 { get; set; }

        [StringLength(25)]
        [Display(Name = "Telefon")]
        public string telefon { get; set; }

        [StringLength(25)]
        [Display(Name = "Fax")]
        public string fax { get; set; }

        [StringLength(25)]
        [Display(Name = "Canlı destek numarası")]
        public string canliDestekNumarasi { get; set; }

        [StringLength(300)]
        [Display(Name = "Adres")]
        public string adres { get; set; }

        [StringLength(100)]
        [Display(Name="Logo")]
        public string logo { get; set; }

        [StringLength(300)]
        [Display(Name = "Slogan")]
        public string slogan { get; set; }

        [StringLength(50)]
        [Display(Name = "Facebook Adresi")]
        public string facebookAdresi { get; set; }

        [StringLength(50)]
        [Display(Name = "Twitter Adresi")]
        public string tweetAdresi { get; set; }

        [StringLength(50)]
        [Display(Name = "LinkedIn Adresi")]
        public string linkedAdresi { get; set; }

        [StringLength(50)]
        [Display(Name = "Haftaiçi Çalışma saatleri")]
        public string haftaIciCalismaSaatleri { get; set; }

        [StringLength(50)]
        [Display(Name = "Cumartesi çalışma saati")]
        public string cumartesiIciCalismaSaatleri { get; set; }

        [StringLength(50)]
        [Display(Name = "Pazar çalışma saati")]
        public string pazarIciCalismaSaatleri { get; set; }

        [StringLength(15)]
        [Display(Name = "Vergi No")]
        public string vergiNo { get; set; }

        [StringLength(100)]
        [Display(Name = "Vergi Dairesi")]
        public string vergiDairesi { get; set; }
    
        [Display(Name = "Harita Konumu")]
        public string haritaKonu { get; set; }

         [Display(Name = "Google Analystics Id")]
        public string GoogleAnalysticId { get; set; }

    }

    public class MailAyarlari
    {
        [Key]
        public int id { get; set; }

        [Display(Name="Mail kullanıcı adı")]
        public string mailAdresi { get; set; }

        [Display(Name = "Mail şifresi")]
        public string mailSifresi { get; set; }

        [Display(Name = "Mailin geleceği adres")]
        public string mailgelecekAdres { get; set; }

        [Display(Name="Smtp adresi")]
        public string smptClient { get; set; }

        [Display(Name = "Port numarası")]
        public int port { get; set; }
       
    }

    public class Hakkimizda
    {
        public int Id { get; set; }

        [Display(Name = "Hakkımızda içerik")]
        public string Icerik { get; set; }

        [Display(Name = "Hakkımızda başlık")]
        public string Baslik { get; set; }
        public string Logo { get; set; }
    }
}