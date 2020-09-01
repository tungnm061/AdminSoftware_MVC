using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdminSoftware.Startup))]
namespace AdminSoftware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
