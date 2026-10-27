using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;



namespace HemenTanitim
{
    public class SiparisOnayla : Hub
    {
        private static string conString = ConfigurationManager.ConnectionStrings["DB150408011842Entities"].ToString();

        [HubMethodName("show")]
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SiparisOnayla>();
            context.Clients.All.displaySiparisler();
        }
    }
}
