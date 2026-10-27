using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Footer
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name="1. Kategori")]
        public string Kategori1 { get; set; }

        [Display(Name = "1. Kategori link")]
        public string Kategori1Link { get; set; }

        [StringLength(100)]
        [Display(Name = "2. Kategori")]
        public string Kategori2 { get; set; }

        [Display(Name = "2. Kategori link")]
        public string Kategori2Link { get; set; }

        [StringLength(100)]
        [Display(Name = "3. Kategori")]
        public string Kategori3 { get; set; }


        [Display(Name = "3. Kategori link")]
        public string Kategori3Link { get; set; }

        [StringLength(100)]
        [Display(Name = "4. Kategori")]
        public string Kategori4 { get; set; }

        [Display(Name = "4. Kategori link")]
        public string Kategori4Link { get; set; }

        [StringLength(100)]
        [Display(Name = "5. Kategori")]
        public string Kategori5 { get; set; }

        [Display(Name = "5. Kategori link")]
        public string Kategori5Link { get; set; }
        public string Tags { get; set; }

        
    }

    public class FooterGenelBilgilerBirlesimi
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "1. Kategori")]
        public string Kategori1 { get; set; }

        [Display(Name = "1. Kategori link")]
        public string Kategori1Link { get; set; }

        [StringLength(100)]
        [Display(Name = "2. Kategori")]
        public string Kategori2 { get; set; }

        [Display(Name = "2. Kategori link")]
        public string Kategori2Link { get; set; }

        [StringLength(100)]
        [Display(Name = "3. Kategori")]
        public string Kategori3 { get; set; }


        [Display(Name = "3. Kategori link")]
        public string Kategori3Link { get; set; }

        [StringLength(100)]
        [Display(Name = "4. Kategori")]
        public string Kategori4 { get; set; }

        [Display(Name = "4. Kategori link")]
        public string Kategori4Link { get; set; }

        [StringLength(100)]
        [Display(Name = "5. Kategori")]
        public string Kategori5 { get; set; }

        [Display(Name = "5. Kategori link")]
        public string Kategori5Link { get; set; }
        public string Tags { get; set; }

        public string firmaAdi { get; set; }

        public string adres { get; set; }

        public string facebookAdresi { get; set; }

        public string tweetAdresi { get; set; }

        public string mail { get; set; }

        public string canliDestek { get; set; }
    }
}