using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class SiteGorunum
    {
        [Key]
        public int id { get; set; }
        public string className { get; set; }
    }
}