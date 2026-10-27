using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Hotels
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Hotel Adı")]
        public string HotelAdi { get; set; }

        [Display(Name = "Oda Sayısı")]
        public int odaSayisi { get; set; }   
        
        [Display(Name = "Komisyon Oranı")]
        public decimal KomisyonOrani { get; set; }

        public string Lat { get;set; }

        public string log { get; set; }
    }

    public class Odalar
    {
        [Key]
        public int Id { get; set; }

        [StringLength(36)]
        public string odaGuidId { get; set; }
        public int HotelId { get; set; }

        [StringLength(50)]
        [Display(Name = "Oda No")]
        public string OdaNo { get; set; }
        public byte[] QrCode { get; set; }
    }
}