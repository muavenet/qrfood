using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Mesajlar
    {
        [Key]
        public int id { get; set; }

        public string spId{ get; set; }
        public string urunadi { get; set; }
        public string aliciId { get; set; }
        public string gonderenId { get; set; }

        [Display(Name="Üye adı")]
        public string uyeadi { get; set; }

        [Display(Name = "Mesaj")]
        public string mesaj { get; set; }

        [Display(Name = "Tarih")]
        public DateTime tarih { get; set; }
        public string dosyaNo { get; set; }
        public bool okundumu { get; set; }
        public bool okundumuAdmin { get; set; }

        public bool konuAcikMi { get; set; }
        public string konuId { get; set; }
    }
}