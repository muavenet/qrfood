using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Il
    {
        [Key]
        public Int16 il_id { get; set; }

        [StringLength(16)]
        public string il_ad { get; set; }
    }

    public class Ilce
    {
        [Key]
        public Int16 ilce_id { get; set; }
        public Int16 il_id { get; set; }
        public string ilce_ad { get; set; }
    }

    public class mahalle
    {
        [Key]
        public int mahalle_id { get; set; }
        public Int16? semt_id { get; set; }
              [StringLength(40)]
        public string mahalle_ad { get; set; }
              public Int16 pk_id { get; set; }
    }
    public class Semt
    {
        [Key]
        public Int16 semt_id { get; set; }
        public Int16? ilce_id { get; set; }

        [StringLength(32)]
        public string semt_ad { get; set; }
    }

    public class PostaKodu
    {
        [Key]
        public Int16 pk_id { get; set; }

        [StringLength(6)]
        public Char kod { get; set; }
    }
}