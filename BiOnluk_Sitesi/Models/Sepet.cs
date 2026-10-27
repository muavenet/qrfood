using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class SepetAdetUrunAdi
    {
        public int Adet { get; set; }

        public int Id { get; set; }
        public string UrunAdi { get; set; }

        public decimal Fiyat { get; set; }

    }
    public class Sepet
    {
        [Key]
        public int Id { get; set; }
        public string SiparisVerenId { get; set; }
        public string IlanSahibiId { get; set; }
        public string IlanId { get; set; }
        public string IlanAdi { get; set; }
        public string IlanAciklama { get; set; }
        public string IlanGereksinimleri { get; set; }

        public string Not { get; set; }
        public int Adet { get; set; }
        public decimal Fiyat { get; set; }
        public int teslimatSuresi { get; set; }
        public int AySayisi { get; set; }

        [DefaultValue(false)]
        public bool SiparisEdildiMi{ get; set; }

        public string kullaniciFoto { get; set; }
        public string ilanFotosu { get; set; }
         public string token { get; set; }
    }
}