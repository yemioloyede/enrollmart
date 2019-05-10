using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnrolTraining.Startup))]
namespace EnrolTraining
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
