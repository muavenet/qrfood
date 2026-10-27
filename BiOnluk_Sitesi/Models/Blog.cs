using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Blog
    {
        private DateTime _date = DateTime.Now;

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(400)]
        public string Baslik { get; set; }

        public string Resim { get; set; }

        [Required]
        public string Icerik { get; set; }

 
        [StringLength(50)]
        public string Yazan { get; set; }
        public DateTime Tarih { get { return _date; } set { value = _date; } }

        [StringLength(55)]
        [Display(Name = "Title")]
        public string title { get; set; }

        [StringLength(155)]
        [Display(Name = "Meta description")]
        public string metaDescription { get; set; }

        [Display(Name = "Meta keywords")]
        public string keywords { get; set; }

        [Display(Name = "Tags")]
        public string tags { get; set; }
       
    }
}