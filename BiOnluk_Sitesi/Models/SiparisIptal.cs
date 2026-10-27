using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class SiparisIptal
    {
        [Key]
        public int Id { get; set; }
        public string TalepKodu { get; set; }
        public string SiparisId { get; set; }

        [StringLength(36)]
        public string IptalEdenId { get; set; }

        public DateTime iptalTarihi { get; set; }      

    }

    public class SiparisIptalBilgileri
    {
        public int Id { get; set; }

        [Display(Name="Talep kodu")]
        public string TalepKodu { get; set; }

        [Display(Name = "Sipariş No")]
        public string SiparisId { get; set; }

        [Display(Name = "İptal Tarihi")]
        public DateTime iptalTarihi { get; set; }

        [Display(Name = "Siparis Tarihi")]
        public DateTime SiparisTarihi { get; set; }

        [Display(Name = "Toplam ödeme")]
        public decimal Tutar { get; set; }

        [Display(Name = "Ödeme tipi")]
        public string OdemeTipi { get; set; }

        [Display(Name = "Kullanıcı adı")]
        public string UserName { get; set; }
        
        [Display(Name = "Durum")]
        public string TeslimDurumu { get; set; }

    
    }
}