using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Jonel.Domain.SignalRStartup))]
namespace Jonel.Domain
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            HubConfiguration config = new HubConfiguration { EnableDetailedErrors = true, Resolver = new DefaultDependencyResolver() };
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(config);
        }
    }
}
