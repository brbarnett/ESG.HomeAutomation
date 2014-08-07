using ESG.HomeAutomation.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup("SignalR", typeof(Startup))]
namespace ESG.HomeAutomation.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}