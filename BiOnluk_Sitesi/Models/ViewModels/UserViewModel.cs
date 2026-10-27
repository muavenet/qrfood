using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models.ViewModels
{
    public class UserViewModel
    {
        public string Username { get; set; }
   
        public string DisplayName { get; set; }
        public string Fotograf { get; set; }
        public string CurrentRoom { get; set; }
        public string Device { get; set; }
        public string fromUser { get; set; }
        public bool IsOnline { get; set; }
        public string UserId { get; set; }
        //public string ilgiliProjeAdi { get; set; }
        public string mesaj_icerik { get; set; }
        public int? UnreadMessageCount { get; set; }
    }
}