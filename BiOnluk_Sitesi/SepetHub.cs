using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Configuration;

namespace HemenTanitim
{
    public class SepetHub : Hub
    {
        private static string conString = ConfigurationManager.ConnectionStrings["DB150408011842Entities"].ToString();

        [HubMethodName("show")]
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SepetHub>();
            context.Clients.All.displayStatus();
        }
    }
}