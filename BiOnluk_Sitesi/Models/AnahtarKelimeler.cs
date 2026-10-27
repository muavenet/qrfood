using System.ComponentModel.DataAnnotations;

namespace HemenTanitim.Models
{
    public class AnahtarKelimeler
    {
        [Key]
        public int Id { get; set; }
        public string Kelime_adi { get; set; }
        public string icerik { get; set; }
    }
}