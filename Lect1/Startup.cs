using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lect1.Startup))]
namespace Lect1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
