using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RentaMaq.Startup))]
namespace RentaMaq
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
