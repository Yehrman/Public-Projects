using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EducationAlarm.Startup))]
namespace EducationAlarm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
