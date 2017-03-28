using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CoffeeBackendService.Startup))]

namespace CoffeeBackendService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}