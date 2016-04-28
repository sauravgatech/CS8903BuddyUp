using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GT.BuddyUp.WebApp.Startup))]
namespace GT.BuddyUp.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
