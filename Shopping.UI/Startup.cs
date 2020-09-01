using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shopping.UI.Startup))]
namespace Shopping.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
