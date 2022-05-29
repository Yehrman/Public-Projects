using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CompuskillsMvcProject.Startup))]
namespace CompuskillsMvcProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
