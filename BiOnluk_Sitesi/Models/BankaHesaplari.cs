using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class BankaHesaplari
    {
        [Key]
        public int BId { get; set; }

        [StringLength(36)]
        public string uyeId { get; set; }

           [StringLength(55)]
        public string bankaAdi { get; set; }

          [StringLength(15)]
        public string tcNo { get; set; }

        [StringLength(50)]
        public string subeAdi { get; set; }

        [StringLength(20)]
        public string subeKodu { get; set; }

        [StringLength(30)]
        public string hesapNumarasi { get; set; }

        [StringLength(30)]
        public string iban { get; set; }

        [StringLength(100)]
       public string hesapSahibiAdSoyad { get; set; }
    }
}