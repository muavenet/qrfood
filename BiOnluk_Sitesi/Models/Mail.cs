using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HemenTanitim.Models
{
    public class MailModel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}