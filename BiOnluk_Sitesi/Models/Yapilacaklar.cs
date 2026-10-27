using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Yapilacaklar
    {
       [Key]
        public int Id { get; set; }

        [Display(Name="Yapılacak")]
        public string yapilacak { get; set; }
        
        [Display(Name = "Durum")]
        public bool durum { get; set; }

        [Display(Name = "Tarih")]
        public DateTime tarih { get; set; }

        public int goruntulemeSirasi { get; set; }
    }

  
}