using System.Web.Routing;
using Glimpse.Elmah.SampleMvc.App_Start;

namespace Glimpse.Elmah.SampleMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}