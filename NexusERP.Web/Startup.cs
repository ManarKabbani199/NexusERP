using Microsoft.Owin;
using Owin;
using NexusERP.Web.Security;


[assembly: OwinStartup(typeof(NexusERP.Web.Startup))]

namespace NexusERP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            IdentitySeeder.SeedRoles();
        }
    }
}