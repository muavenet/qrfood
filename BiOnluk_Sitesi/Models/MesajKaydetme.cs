using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class MesajKaydetme
    {
        [Key]
        public int Id { get; set; }

        [Display(Name= "Kullanıcı adı")]
        [StringLength(25)]
        public string userName { get; set; }

        [Display(Name = "Mesaj")]
        public string mesaj { get; set; }
       
        [StringLength(35)]
        [Display(Name = "Tarih")]
        public string tarih { get; set; }
    }
}