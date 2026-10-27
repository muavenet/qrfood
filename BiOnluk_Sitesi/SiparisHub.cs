using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HemenTanitim
{
    public class SiparisHub : Hub
    {
        private static string conString = ConfigurationManager.ConnectionStrings["DB150408011842Entities"].ToString();

        [HubMethodName("show")]
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SiparisHub>();
            context.Clients.All.displaySiparisler();
        }

    }
}