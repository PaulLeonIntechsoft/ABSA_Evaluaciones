using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ABSA_Evaluaciones.Startup))]
namespace ABSA_Evaluaciones
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
