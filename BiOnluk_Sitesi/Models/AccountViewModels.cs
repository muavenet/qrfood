using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HemenTanitim.Models
{

    public class ExternalLoginViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı adı")]
        public string UserName { get; set; }

          [Required]
          [DataType(DataType.EmailAddress)]
          [EmailAddress]
        public string Email { get; set; }


    }

 public class BilgileriDegistir_Anasayfa
	{
		public string Fotograf { get; set; }

		[Display(Name = "Email Adresi")]
		public string EmailAdresi { get; set; }

		[Display(Name = "Konum")]
		public Int16? Konum { get; set; }

		[Display(Name = "Cep Telefonu")]
		public string Telefon { get; set; }

		[Display(Name = "Kullanıcı Adı")]
		public string UserName { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[Display(Name = "Son Görülme Tarihi")]
		public DateTime SonGorulmeTarihi{ get; set; }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[Display(Name = "Üyelik Tarihi")]
		public DateTime UyelikTarihi { get; set; }

		[Display(Name = "Ortalama Yanıt Süresi")]
		public string OrtalamaYanitSuresi { get; set; }

        [Display(Name = "Yorum sayısı")]
        public int YorumSayisi { get; set; } = 0;

		[Display(Name = "Önceki Siparişler")]
		public int OncekiSiparisler { get; set; }

		[Display(Name = "İptal Edilenler")]
		public int IptalEdilenler { get; set; }

		//[StringLength(250, ErrorMessage="Karakter sayısı 250'den fazla olamaz.")]
		[Display(Name = "Hakkımda")]
		public string Hakkimda { get; set; }

        [Display(Name = "Sözleşme İçeriği")]
        public string Sozlesme_icerigi { get; set; }

        [Display(Name = "Kimden")]
        public Int16? Kimden { get; set; }    

        [Display(Name = "Kimden")]
        public string KimdenAciklama { get; set; }

        public string adres { get; set; }

        public string ad { get; internal set; }

        public string soyad { get; internal set; }

        public string iban { get; internal set; }

        public string tckimlik { get; internal set; }
    }
    public class BilgeriDegistir
    {
		public string Fotograf { get; set; }

		[Display(Name = "Email Adresi :")]
		public string EmailAdresi { get; set; }

        public string Telefon { get; set; }

		[Display(Name = "Kullanıcı Adı :")]
		public string UserName { get; set; }

        public string kullaniciId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[Display(Name = "Son Görülme Tarihi")]
		public DateTime SonGorulmeTarihi{ get; set; }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		[Display(Name = "Üyelik Tarihi")]
		public DateTime UyelikTarihi { get; set; }

		[Display(Name = "Ortalama Yanıt Süresi :")]
		public string OrtalamaYanitSuresi { get; set; }

		[Display(Name = "Yorum Sayısı :")]
        [DefaultValue(0)]
		public int YorumSayisi { get; set; }

		[Display(Name = "Önceki Siparişler :  ")]
		public int OncekiSiparisler { get; set; }

		[Display(Name = "İptal Edilenler :")]
		public int IptalEdilenler { get; set; }

		[StringLength(150, ErrorMessage="Karakter sayısı 150'den fazla olamaz.")]
		[Display(Name = "Hakkımda")]
		public string Hakkimda { get; set; }

        [StringLength(150, ErrorMessage = "Karakter sayısı 150'den fazla olamaz.")]
        [Display(Name = "Sözleşme İçeriği")]
        public string Sozlesme_icerigi { get; set; }

        public Decimal puan { get; set; }

        public string kimdenAciklama{ get; set; }

        public string ad { get; set; }

        public string soyad { get; set; }

        [Display(Name = "Konum :")]
        public string konum { get; set; }

        public Int16? kimden { get; set; }

        public List<Yorumlar> Yorumlars{ get; set; }

        public List<Portfolyo> Portfolyos { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Eski şifre")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifre tekrar")]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Confirm code is required")] 
        public string Password1 { get; set; }

        [Required(ErrorMessage = "Confirm code is required")]       
        public string Password2 { get; set; }

        [Required(ErrorMessage = "Confirm code is required")]
        public string Password3 { get; set; }

        [Required(ErrorMessage = "Confirm code is required")]     
        public string Password4 { get; set; }    
        public string hotelAdi { get; set; }
        public int hotelId { get; set; }
        public string odaGuidId { get; set; }
        public string odaNo { get; set; }

        public string Lat { get; set; }
        public string log { get; set; }
    }

    public class RegisterViewModel
    {
		[Required]
        [Display(Name = "Kullanıcı adı")]
        public string UserName { get; set; }

        [Required]
      
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string ProfilFotografi { get; set; }

        [Required]
        //[DataType(DataType.PhoneNumber)]
        public string Telefon { get; set; }

        public short Konum { get; set; }

    }

    public class UserAndGroupAndPeople
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Telefon { get; set; }
        public string Role { get; set; }
    }

    public class UserYetkiViewModel
    {
        public List<UserViewModel> UserViewModels { get; set; }
        public List<UserViewModel> SelectedUserViewModel { get; set; }
        public List<int> PostedYetkiIDs { get; set; }
    }
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

      

    }
}
