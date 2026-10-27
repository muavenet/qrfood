using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using AutoMapper;
using HemenTanitim.Mappings;

namespace HemenTanitim
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string connString = ConfigurationManager.ConnectionStrings["DB150408011842Entities"].ConnectionString;

        protected void Application_BeginRequest()
        {
            //if (!Context.Request.IsSecureConnection)
            //    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
        }
        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer<Models.HemenTanitimContext>(null);

            //Mapper.Initialize(cfg =>
            //{
            //    cfg.AddProfile<MessageProfile>();
            //    cfg.AddProfile<RoomProfile>();
            //    cfg.AddProfile<UserProfile>();
            //});
            //RouteTable.Routes.MapHubs();

            //Application["Totaluser"] = 0;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SqlDependency.Start(connString);

            
        }
        protected void Application_End()
        {
            SqlDependency.Stop(connString);
        }

        protected void Application_EndRequest()
        {
        }

        protected void Session_Start()
        {
            //Application.Lock();
            //Application["Totaluser"] = (int)Application["Totaluser"] + 1;
            //Application.UnLock();
        }

        public class RefreshDetectFilter : ActionFilterAttribute, IActionFilter
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var cookie = filterContext.HttpContext.Request.Cookies["RefreshFilter"];
                filterContext.RouteData.Values["IsRefreshed"] = cookie != null &&
                                                                cookie.Value == filterContext.HttpContext.Request.Url.ToString();

                filterContext.HttpContext.Response.SetCookie(new HttpCookie("RefreshFilter", filterContext.HttpContext.Request.Url.ToString()));
            }
        }
    }
}
