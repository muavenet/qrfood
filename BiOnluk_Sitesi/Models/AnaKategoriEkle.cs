using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class AnaKategoriEkle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Kategori Adı")]        
        public string KategoriAdi { get; set; }

        public string resim { get; set; }
    }
 
}