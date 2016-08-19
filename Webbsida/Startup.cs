using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Webbsida.Startup))]
namespace Webbsida
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
