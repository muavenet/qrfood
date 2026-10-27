using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
   
    public class AnaKategoriListesi
    {
       [Key]
        public int Id { get; set; }
        public int KategoriAdi { get; set; }
        public virtual AltKategoriListesi AltKategori { get; set; }
   }

    public class AltKategoriListesi
    {
        [Key]
        public int Id { get; set; }
        public string KategoriAdi { get; set; }
        public virtual ICollection<AnaKategoriListesi> AnaKateogorilers { get; set; }
    }
}