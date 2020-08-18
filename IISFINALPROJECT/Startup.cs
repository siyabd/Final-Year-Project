using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IISFINALPROJECT.Startup))]
namespace IISFINALPROJECT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
