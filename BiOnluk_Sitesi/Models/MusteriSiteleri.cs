using HemenTanitim.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class MusteriSiteleri
    {
        [Key]    
        public Guid siteId { get; set; }
       
        [StringLength(36)]
       
        public string uyeId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Lütfen daha kısa bir site adı giriniz" )]
        [Display(Name = "Site Adı")]
        public string siteAdi { get; set; }

        [Required]      
        [Url]
        [StringLength(250)]
        [Display(Name = "Site URL")]
        public string siteUrl { get; set; } 
       
        [Required]
        [Display(Name = "Aylık Gösterim")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Lütfen sadece rakam giriniz")]
        public int aylikGosterim { get; set; }
        public DateTime eklenmeTarihi { get; set; }
      
        public int AnaKategoriId { get; set; }

        public int AltKategoriId { get; set; }
        public AnaKategoriler AnaKategoriler { get; set; }
    }
    public class MusteriSiteleriSiteIdSiteUrl
    {
        public Guid siteId { get; set; }      
        public string siteUrl { get; set; }
    }
}