using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MainProject.Startup))]
namespace MainProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
