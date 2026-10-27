using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class KargoSirketleri
    {
        private DateTime _date = DateTime.Now;
        public int Id { get; set; }

           [Display(Name = "Firma adı")]
        public string kargoSirketAdi { get; set; }

          [Display(Name = "Fiyat")]
        public decimal fiyat { get; set; }

            [Display(Name = "Son güncelleme")]
        public DateTime sonGuncelleme { get { return _date; } set { value = _date; } }
    }
}