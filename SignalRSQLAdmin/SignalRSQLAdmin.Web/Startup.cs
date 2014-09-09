using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalRSQLAdmin.Web.Startup))]
namespace SignalRSQLAdmin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
