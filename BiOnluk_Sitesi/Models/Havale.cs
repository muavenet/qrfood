using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
		public class Havale
		{
			[Key]
			public int id { get; set; }

			[Display(Name="Banka Adı")]
			public string bankaAdi { get; set; }

			[Display(Name = "Banka Şubesi")]
			public string bankaSubesi { get; set; }

			[Display(Name = "Şube Kodu")]
			public int subeKodu { get; set; }

			[Display(Name = "Hesap No")]
			public int hesapNo { get; set; }

			[Display(Name = "IBAN")]
			public string iban { get; set; }
		}
}