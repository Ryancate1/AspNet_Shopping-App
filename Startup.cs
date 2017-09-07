using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(rcateShoppingApp1.Startup))]
namespace rcateShoppingApp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
