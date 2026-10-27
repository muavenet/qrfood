using GridMvc.DataAnnotations;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace HemenTanitim.Models
{
    public class Urunler
    {
        [Key]
        public int UrunId { get; set; }

        [StringLength(36)]
        public string UyeId { get; set; }
        public int AnaKategoriId { get; set; }
        public int AltKategoriId { get; set; }
        public string Guid { get; set; }

        [StringLength(200)]
        [Display(Name = "Ürün adı")]
        public string UrunAdi { get; set; }  

        [Display(Name = "Ürün detay")]
        public string UrunIcerik { get; set; }
        public int Fiyat { get; set; }
        public int satisSayisi { get; set; }
        public string siteUrl { get; set; }
        public bool Trending { get; set; }
        public bool Hot { get; set; }
        public bool Promoted { get; set; }
  
        public DateTime Tarih { get; set; }
        public Kategoriler Kategoriler { get; set; }        
        public int? ZiyaretciSayisi { get; set; }
        public IEnumerable<Fotolar> Fotolar { get; set; }

        [StringLength(50)]
        public string yayinSuresi { get; set; }
        public Guid siteId { get; set; }
        public bool Onay { get; set; }
        public bool kaldir { get; set; }

    }

    public class EnCokZiyaretEdenler
    {
        [Display(Name = "Ürün adı")]
        public string UrunAdi { get; set; }

        public int? ZiyaretciSayisi { get; set; }
    }

    public class KategoriListesiVeFotolar {

        public List<UrunListele> UrunListesi { get; set; }
        public List<Fotolar> Fotolar { get; set; }
    }
    public class VitrinContactModel
    {
        public VitrinUrunleri VitrinUrunleri { get; set; }
        public MailModel MailModel { get; set; }
    }
    public class KategoriSayi
    {
        public int Id { get; set; }
        public string AnaKategoriAdi { get; set; }
        public string KategoriAdi { get; set; }
        public int UrunAdet { get; set; }
        public int AnaKategoriId { get; set; }
    }

    public class FotoVeUrunler
    {
        public string UrunAdi {get; set; }
        public string ThumbnailYolu { get; set; }
        public string UrunId { get; set; }
        public int Id { get; set; }
        public decimal UstuCiziliFiyat { get; set; }
        public decimal Fiyat { get; set; }
        public int? IndirimOrani { get; set; }
     
    }

    public class VitrinUrunleri
    {
        public int Id { get; set; }
        public string ResimYolu { get; set; }
        public string UrunAdi { get; set; }
        public decimal? Fiyat { get; set; }
        public int? IndirimOrani { get; set; }
        public decimal UstuCiziliFiyat { get; set; }
        public string ThumbYolu { get; set; }
        public bool OzelUrun { get; set; }
        public bool SlaytResimMi { get; set; }
        public string SlaytResimYolu { get; set; }
        public bool Vitrin { get; set; }
        public SepetAdetUrunAdi SepetUrunAdi { get; set; }
       

        public string tag { get; set; }
    }
    public class UrunListele
    {
        public int Id { get; set; }
        public string ResimYolu { get; set; }
        public string UrunAdi { get; set; }
        public int Count { get; set; }
        public decimal Fiyat { get; set; }
        public decimal UstuCiziliFiyat { get; set; }
        public int? IndirimOrani { get; set; }
        public string tag { get; set; }
       
    }

    public class UrunListele2
    {
        public string isId { get; set; }
        public string baslik { get; set; }
        public string Guid { get; set; }
        public string ResimYolu { get; set; }

        public int tavsiye { get; set; }
        public Decimal puan_ortalamasi { get; set; }

        public string KullaniciAdi { get; set; }
        public string KullaniciFoto { get; set; }
        public string KullaniciId { get; set; }

        public string MinFiyat { get; set; }

        public int AySayisi { get; set; }

        public DateTime Tarih { get; set; }

        public string metaDescripton { get; set; }
        public string title { get; set; }   
        public string siteUrl { get; set; }     


        public bool oneCikarma { get; set; }

        public int artanSayi{ get; set; }

        public string AltKategoriAdi { get; set; }
        public int AltKategoriId { get; set; }
        public string AnaKategoriAdi { get; set; }
        public string AnaKategoriLink { get; set; }  
        public int AnaKategoriId { get; set; } 

        public string KimdenAciklama { get; set; }
        
        public string UserName { get; set; }

        public int? ZiyaretciSayisi { get; set; }

        public int YorumSayisi { get; set; }

        public bool Trending { get; set; }
        public bool Hot { get; set; }
        public bool Promoted { get; set; }

        public string TeslimSuresi { get; set; }
        public int kacKisilik { get; set; }
        public int indirimOrani { get; set; }
        public bool mostPopular { get; set; }
        public bool mostSales { get; set; }  
        public bool Vitrin { get; set; }
    }

    public class IsIlanlariListele_IlanDetay
    {
        public string isId { get; set; }
        public string baslik { get; set; }
        public string Guid { get; set; }
        public string ResimYolu { get; set; }


        public string aciklama { get; set; }
        public int tavsiye { get; set; }
        public int is_puan_ortalamasi { get; set; }
        public int yorum_sayisi { get; set; }

        public string KullaniciAdi { get; set; }
        public string KullaniciFoto { get; set; }

        public string MinFiyat { get; set; }

        public DateTime Tarih { get; set; }

        public string metaDescripton { get; set; }
        public string title { get; set; }
        public string siteUrl { get; set; }

        public int teslimatSuresi { get; set; }

        public bool Promoted { get; set; }
        public bool Trending { get; set; }
        public bool Hot { get; set; }

        public string hotelAdi { get; set; }

        public string odaNo { get; set; }

        public int artanSayi { get; set; }
        public int AySayisi { get; set; }

        public string AltKategoriAdi { get; set; }
        public int AltKategoriId { get; set; }
        public string AnaKategoriAdi { get; set; }
        public string AnaKategoriLink { get; set; }
        public int AnaKategoriId { get; set; }

        public string UserName { get; set; }
        public string kullaniciId { get; set; }

        public string ilanGereksinimleri { get; set; }
        public int? ZiyaretciSayisi { get; set; }

    }

    public class Kategoriler
    {
        public int Id { get; set; }

        public string KategoriAdi { get; set; }

        public virtual ICollection<Urunler> Urunlerim { get; set; }
    }
    public class Fotolar
    {
        public int Id { get; set; }
        public string UrunId { get; set; }
        public string ResimYolu { get; set; }
        public string ContentType { get; set; }
        public string ThumbnailYolu { get; set; }
        public bool Vitrin { get; set; }
        public int AltKategoriId { get; set; }
        public bool Slayt { get; set; }
        public bool SlaytKucukAnasayfa1 { get; set; }
        public bool SlaytKucukAnasayfa2 { get; set; }

        [Display(Name = "Anasayfa en uzun banner")]
        public bool AnasayfaEnUzunBanner{ get; set; }
        public bool KategoriSlayt_h248 { get; set; }
        public bool KategoriSlayt_h430 { get; set; }

        [Display(Name = "Bizi tercih eden firmalar")]
        public bool BiziTercihEdenFirmalar { get; set; }

        [Display(Name="Başlık 1")]
        public string Baslik1 {get; set;}
        [Display(Name = "Başlık 2")]
        public string Baslik2 { get; set; }
        [Display(Name = "İçerik")]
        public string Icerik { get; set; }
        [Display(Name = "Link")]
        public string link { get; set; }
        public virtual List<Urunler> Urunlerim { get; set; }
    }

    public class FavoriUrunler
    {
        [Key]
        public int Id { get; set; }
        public string isId { get; set; }
        public string KullaniciId { get; set; }
    }

    public class Karsilastirmalar
    {
        [Key]
        public int Id { get; set; }
        public int UrunId { get; set; }
        public string KullaniciId { get; set; }
    }

    public class KarsilastirmaUrunlerFotolarla
    {
        public int UrunId { get; set; }
        public string ResimYolu { get; set; }
        public string UrunAdi { get; set; }
        public int Fiyat { get; set; }

        public int KarsilastirmaUrunId { get; set; }
    }

    public class FavoriUrunlerFotolarla
    {
        public string isId { get; set; }
        public string ResimYolu { get; set; }
        public string UrunAdi { get; set; }
        public int Fiyat{ get; set; }

        public int FavoriUrunId { get;set;}
    }


    //#region HttpPostedFile kullanımı

    //public class UploadFileModel
    //{
    //    [FileSize(10240)]
    //    [FileTypes("jpg,jpeg,png")]
    //    public HttpPostedFileBase File { get; set; }
    //}
    //public class FileSizeAttribute : ValidationAttribute
    //{
    //    private readonly int _maxSize;

    //    public FileSizeAttribute(int maxSize)
    //    {
    //        _maxSize = maxSize;
    //    }

    //    public override bool IsValid(object value)
    //    {
    //        if (value == null) return true;

    //        return (value as HttpPostedFileBase).ContentLength <= _maxSize;
    //    }

    //    public override string FormatErrorMessage(string name)
    //    {
    //        return string.Format("The file size should not exceed {0}", _maxSize);
    //    }
    //}

    //public class FileTypesAttribute : ValidationAttribute
    //{
    //    private readonly List<string> _types;

    //    public FileTypesAttribute(string types)
    //    {
    //        _types = types.Split(',').ToList();
    //    }

    //    public override bool IsValid(object value)
    //    {
    //        if (value == null) return true;

    //        var fileExt = System.IO.Path.GetExtension((value as HttpPostedFileBase).FileName).Substring(1);
    //        return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
    //    }

    //    public override string FormatErrorMessage(string name)
    //    {
    //        return string.Format("Invalid file type. Only the following types {0} are supported.", String.Join(", ", _types));
    //    }
    //}
    //#endregion

    public class SepetCount { 
        public int Count { get; set; }
    } 
  
   
}