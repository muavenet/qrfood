using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Fiyatlandirma
    {
            [Key]
            public int Id { get; set; }

            [Display(Name = "Basic Fiyat")]
            public string BasicFiyat { get; set; }

            public string BasisIcerik { get; set; }

            public int BasicGoruntulemeSirasi { get; set; }


            [Display(Name = "Medium Fiyat")]
            public string MediumFiyat { get; set; }

            public string mediumIcerik { get; set; }

            public int MediumGoruntulemeSirasi { get; set; }


            [Display(Name = "Ultimate Fiyat")]
            public string UltimateFiyat { get; set; }

            public string UltimateIcerik { get; set; }

            public int UltimateGoruntulemeSirasi { get; set; }


            public int goruntulemeSirasi { get; set; }
       
    }
}