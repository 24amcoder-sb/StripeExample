using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StripeExample.Web.Startup))]
namespace StripeExample.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
