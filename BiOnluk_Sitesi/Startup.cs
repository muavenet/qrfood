
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HemenTanitim.Startup))]
namespace HemenTanitim
{
 
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();

        }
      

    }
}
