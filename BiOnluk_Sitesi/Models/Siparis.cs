using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{

    public class SiparisBilgileri
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string SiparisId { get; set; }

        public int hotelId { get; set; }

        public string odaNo { get; set; }

        [StringLength(36)]
        public string SiparisVerenId { get; set; }

        public decimal Tutar { get; set; }

        [StringLength(36)]
        public string SiparisTarihi { get; set; }

        public int SiparisDurumu { get; set; }
        public bool siparisIptalTalebi { get; set; }

        public string IlanAciklama { get; set; }

        public string conversationId { get; set; }

        [StringLength(50)]
        public string token { get; set; }

        [DefaultValue(false)]
        public bool Okundumu { get; set; }

        [DefaultValue(false)]
        public bool yorum { get; set; }

        public byte Durumu { get; set; }

        public byte OdemeTipi { get; set; }
    }

    public class SiparisBilgileriIndex {
        public string SiparisId { get; set; }
        public int SiparisDurumu { get; set; }
        public string SiparisTarihi { get; set; }
    }

    public class SiparisOzet
    {
        public string  siparisId { get; set; }

        public decimal Tutar { get; set; }

        public string IlanAdi { get; set; }

        public bool Okundumu { get; set; }

        public byte OdemeTipi { get; set; }
      
    }
    public class EncokSiparisAlanUrunler
    {
        [Display(Name="Ürün adı")]
        public string Urunler { get; set; }

        [Display(Name="Sipariş Miktarı")]
        public int SiparisMiktari { get; set; }
    }

    public class SiparisIstatistikleri
    {
        [Display(Name = "Sipariş Durumu")]
        public string TeslimDurumu { get; set; }

        [Display(Name = "Miktar")]
        public int Sayi { get; set; }
    }

    public class ciroYillik
    {
        [Display(Name = "Yıl")]
        public string Yil { get; set; }

        [Display(Name = "Toplam")]
        public decimal Toplam { get; set; }

    }

    public class ciroAylik
    {
        [Display(Name = "Ay")]
        public string Ay { get; set; }

        [Display(Name = "Toplam")]
        public decimal Toplam { get; set; }


    }

    public class AylikSatisOranlari
    {
        public string AylikSatis { get; set; }
        public decimal ToplamSatis { get; set; }
    }

    public class GunlukSatisOranlari
    {
        public string SatisGunleri { get; set; }
        public decimal Tutar { get; set; }
    }

    public class UstMenuModel
    {
        public int YeniSiparis { get; set; }

        public int toplamSiparis { get; set; }
        public int KayitliKullanici { get; set; }
        public int OnaysizYorum { get; set; }
        public int OkunmayanMesajlar { get; set; }
    }

    public class FaturaKesimBilgileri
    {
        public string SiparisId { get; set; }
        public DateTime SiparisTarihi { get; set; }
        public string Urunler { get; set; }
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamFiyat { get; set; }
        public string TeslimDurumu { get; set; }
        public string OdemeTipi { get; set; }
        public string FaturaKisiKurulusUnvan { get; set; }
        public string FaturaVergiDairesi { get; set; }  
        public int FaturaVergiNo { get; set; }
        public string FaturaAdres { get; set; }
        public string FaturaIl { get; set; }
        public string KargoKisiKurulusUnvan { get; set; }
        public string KargoSirketi { get; set; }
        public string KargoSabitTel { get; set; }
        public string KargoCepTel { get; set; }
        public string KargoSehir { get; set; }
        public string KargoAdres { get; set; }
        public decimal GenelToplam { get; set; }
        public decimal KdvMiktarı{ get; set; }
        public string YaziSayi { get; set; }

    }

    public class Fatura
    {
        private DateTime _date = DateTime.Now;

        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SiparisId { get; set; }

        [DisplayName("Ad / Şirket Ad:")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string adsirketUnvan { get; set; }

        [DisplayName("Soyad / Ünvan:")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string SoyadUnvan { get; set; }

        [DisplayName("Şehir :")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Sehir { get; set; }

        [DisplayName("İlçe")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Ilce { get; set; }

        [DisplayName("Semt")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Semt { get; set; }

        [DisplayName("Mahalle")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Mahalle { get; set; }

        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Adres { get; set; }

        [DisplayName(" Posta Kodu")]
        public string PostaKodu { get; set; }

        [DisplayName("Sabit Telefon:")]
        public string SabitTelefon { get; set; }

        [DisplayName("Cep Telefonu:")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string CepTelefonu { get; set; }

        [DisplayName("Vergi Dairesi:")]
        public string VergiDairesi { get; set; }

        [DisplayName("T.C./Vergi No::")]
        public int TCVergiNo { get; set; }
        public DateTime GirisTarihi
        {
            get { return _date; }
            set { _date = value; }
        }


    }

    public class Kargo
    {

        private DateTime _date = DateTime.Now;

        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SiparisId { get; set; }

        [DisplayName("Ad / Şirket Ad:")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string adsirketUnvan { get; set; }

        [DisplayName("Soyad / Ünvan:")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string SoyadUnvan { get; set; }

        [DisplayName("Şehir :")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Sehir { get; set; }

        [DisplayName("İlçe")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Ilce { get; set; }

        [DisplayName("Semt")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Semt { get; set; }

        [DisplayName("Mahalle")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Mahalle { get; set; }

        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string Adres { get; set; }

        [DisplayName(" Posta Kodu")]
        public string PostaKodu { get; set; }

        [DisplayName("Sabit Telefon:")]

        public string SabitTelefon { get; set; }

        [DisplayName("Cep Telefonu:")]
        [Required(ErrorMessage = "Bu alan gereklidir.")]
        public string CepTelefonu { get; set; }

        [DisplayName("Vergi Dairesi:")]
        public string VergiDairesi { get; set; }

        [DisplayName("T.C./Vergi No::")]
        public int TCVergiNo { get; set; }

        public DateTime GirisTarihi
        {
            get { return _date; }
            set { _date = value; }
        }

    }

}