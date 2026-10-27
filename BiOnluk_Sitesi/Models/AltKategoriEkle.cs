using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HemenTanitim.Models
{
    public class KategoriListe
    {
        public int ID { get; set; }

        public string AnaKategoriAdi { get; set; }

        public string AltKategoriAdi { get; set; }
    }


    public class AltKategoriEkle
    {
        [Key]
        public int Id { get; set; }        
        [Required]
        [Display(Name = "Kategori Adı")]
        public string KategoriAdi { get; set; }
        public int AnaKategoriId { get; set; }

        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [StringLength(155)]
        [Display(Name = "Meta description")]
        public string metaDescripton { get; set; }

        
        [Display(Name = "Meta keywords")]
        public string metaKeywords { get; set; }

        [StringLength(155)]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Tags")]
        public string tags { get; set; }

        public int siralama { get; set; }

        public AnaKategoriler AnaKategoriler{ get; set; }
    }

    public class AnaKategoriler
    {       
        public int Id { get; set; }
        public string KategoriAdi { get; set; }
        public string metaDescripton { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public int siralama { get; set; }

        public string resim { get; set; }
        public virtual ICollection<AltKategoriEkle> AltKategoriler { get; set; }
    }




    public class UstKategoriler
    {
       
        public int ID { get; set; }
        public int AnaKategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public bool AnakategoriMi { get; set; }
       
    }
   
}