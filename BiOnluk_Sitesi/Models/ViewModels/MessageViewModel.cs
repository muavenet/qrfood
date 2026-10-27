using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models.ViewModels
{
    public class MessageViewModel
    {
        public string Content { get; set; }
        public string tarih { get; set; }
        public string saat { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string receiverId { get; set; }
        public string senderId { get; set; }
        public string UsersId { get; set; }
        public string profil_fotosu { get; set; }
        public string isIlanId { get; set; }
    }


}