using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrivateLibrary.Startup))]
namespace PrivateLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
