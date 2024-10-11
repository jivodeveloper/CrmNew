using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Call_Centre_Management.Startup))]

namespace Call_Centre_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}