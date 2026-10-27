using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Portfolyo_stpr
    {
        [StringLength(36)]
        public string IsIlanId { get; set; }

        [StringLength(36)]
        public string userId { get; set; }

        public Portfolyo_stpr()
        {
            Fotograflar = new List<string>();
            SilenenFotoğraflar = new List<string>();
        }

        public List<string> Fotograflar { get; set; }

        public List<string> SilenenFotoğraflar { get; set; }

        public string StrFotograflar
        {
            get
            {
                string strFotograflar = "";
                for (int i = 0; i < Fotograflar.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Fotograflar[i]))
                    {
                        strFotograflar += Fotograflar[i];
                        if (i != Fotograflar.Count - 1)
                            strFotograflar += ",";
                    }
                }
                return strFotograflar;
            }
        }

        public string StrSilinenFotograflar
        {
            get
            {
                string strSilinenFotograflar = "";
                for (int i = 0; i < SilenenFotoğraflar.Count; i++)
                {
                    if (!string.IsNullOrEmpty(SilenenFotoğraflar[i]))
                    {
                        strSilinenFotograflar += SilenenFotoğraflar[i];
                        if (i != SilenenFotoğraflar.Count - 1)
                            strSilinenFotograflar += ",";
                    }
                }
                return strSilinenFotograflar;
            }


        }

    }

    public class Portfolyo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(36)]
        public string port_Id { get; set; }

        [StringLength(36)]
        public string userId { get; set; }

        public string Fotograflar { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Lütfen daha kısa bir site adı giriniz")]
        [Display(Name = "Başlık")]
        public string baslik { get; set; }

        
        [Url]
        [StringLength(250)]
        [Display(Name = "URL")]
        public string demoUrl { get; set; }

        public DateTime eklenmeTarihi { get; set; }

        public int AnaKategoriId { get; set; }

        public int AltKategoriId { get; set; }

        public AnaKategoriler AnaKategoriler { get; set; }

     

    }

    public class Portfolyo_m
    {

        public int Id { get; set; }

        [StringLength(36)]
        public string port_Id { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        [StringLength(36)]
        public string userId { get; set; }

        public string Fotograflar { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Lütfen daha kısa bir site adı giriniz")]
        [Display(Name = "Başlık")]
        public string baslik { get; set; }

        [Url]
        [StringLength(250)]
        [Display(Name = "URL")]
        public string demoUrl { get; set; }

        public DateTime eklenmeTarihi { get; set; }

        public int AnaKategoriId { get; set; }

        public int AltKategoriId { get; set; }

        public AnaKategoriler AnaKategoriler { get; set; }



    }




}