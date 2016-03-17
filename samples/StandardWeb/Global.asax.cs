using System.Web.Mvc;
using System.Web.Routing;

namespace StandardWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        #region Methods

        #region Protected Methods

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DependencyConfig.SetResolver();
            MapperConfig.Configure();
        }

        #endregion

        #endregion
    }
}