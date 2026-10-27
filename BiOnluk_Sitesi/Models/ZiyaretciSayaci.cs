using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class ZiyaretciSayaci
    {
        [Key]
       
        public int Id { get; set; }       

        [StringLength(50)]
        public string ilAdi { get; set; }

        public int ilKodu { get; set; }
        public int? ziyaretciSayisi { get; set; }
    }
}