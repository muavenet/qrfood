using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HemenTanitim
{
    /// <summary>
    /// Summary description for Resim_Yakala
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Resim_Yakala : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public void Yakala(string[] ss)
        {
            System.Web.HttpContext.Current.Session.Add("ListeEkle", ss);
        }

        [WebMethod(EnableSession = true)]
        public bool YakalaDuzelt(string[] ss, string[] silinecek)
        {
            HttpContext.Current.Session.Add("ListeDuzenle", ss);
            HttpContext.Current.Session.Add("ListeSil", silinecek);
            string[] Ses1 = HttpContext.Current.Session["ListeDuzenle"] as string[];
            string[] Ses2 = HttpContext.Current.Session["ListeSil"] as string[];
            if (Ses1.Length != 0 || Ses2.Length != 0)
            {
                return true;
            }
            return false;
        }
    }
}
