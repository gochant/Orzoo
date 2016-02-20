using System;
using System.Data.Entity;
using System.Web.Mvc;
using Orzoo.Core.Data;
using Orzoo.Core.Utility;

namespace Orzoo.AspNet.Mvc
{
    public class DynamicCacheConfig
    {
        #region Methods

        #region Public Methods

        public static void Configure()
        {
            var services = RuntimeHelper
                .GetCurrentDomainImplementTypes(typeof (ICacheService<>));
            using (var context = (DbContext) DependencyResolver.Current.GetService(typeof (DbContext)))
            {
                foreach (var serviceType in services)
                {
                    var obj = Activator.CreateInstance(serviceType, new object[] {context});
                    var service = (ICacheService) obj;
                    service.UpdateCache();
                }
            }
        }

        #endregion

        #endregion
    }
}