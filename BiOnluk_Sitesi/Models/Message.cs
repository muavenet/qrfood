using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string receiverId { get; set; }

        public string senderId { get; set; }

        public string Content { get; set; }

        public string profil_fotosu { get; set; }

        //public string Timestamp { get; set; }

        public string tarih { get; set; }

        public string saat { get; set; }

        //public virtual ApplicationUser FromUser { get; set; }

        public string toUser { get; set; }

        public string fromUser { get; set; }
        public DateTime? ReadOn { get; set; }

    
        //[Required]
        //public virtual Room ToRoom { get; set; }
    }
}