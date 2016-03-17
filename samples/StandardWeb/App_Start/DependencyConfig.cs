using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using Orzoo.AspNet.Logging;
using StandardWeb.Data;

namespace StandardWeb
{
    public class DependencyConfig
    {
        #region Methods

        #region Public Methods

        public static void SetResolver()
        {
            var container = new Container();
            //container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Register<ApplicationDbContext, ApplicationDbContext>();
            container.Register<ILogDbContext, ApplicationDbContext>();
            container.Register<DbContext, ApplicationDbContext>();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();

            //  container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

        }

        #endregion

        #endregion
    }
}