using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Attention_Seeker.Startup))]
namespace Attention_Seeker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
