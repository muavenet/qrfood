using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class RouteModel_Tum
    {
        public string Kelime_adi { get; set; }
        public string il_ad { get; set; }
        public string ilce_ad { get; set; }
        public string Icerik { get; set; }
    }

    public class RouteModel_İki
    {
        public string Kelime_adi { get; set; }
        public string il_ad { get; set; }
    }

    public class RouteModel_Tek
    {
        public string Kelime_adi { get; set; }

    }
}