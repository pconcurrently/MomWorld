using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MomWorld.Startup))]
namespace MomWorld
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
