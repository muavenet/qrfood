using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Yorumlar
    {
        [Key]
        public int Id { get; set; }
        public string isId { get; set; }
        public string isBaslik { get; set; }

        public string kullaniciId { get; set; }

        [Display(Name="Kullanıcı Adı")]
        public string kullaniciAdi { get; set; }

        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [Display(Name = "Açıklama")]
        public string KullaniciFoto { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime GirisTarihi { get; set; }
        public bool Onay { get; set; }

        public int puan { get; set; }

        //public virtual IsIlanlariListele_IlanDetay Ilanlar { get; set; }

    }
}