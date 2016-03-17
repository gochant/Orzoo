using System.Web.Mvc;
using Orzoo.AspNet.Attributes;

namespace StandardWeb
{
    public class FilterConfig
    {
        #region Methods

        #region Public Methods

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionAttribute());
            filters.Add(new ValidateModelStateAttribute());
        }

        #endregion

        #endregion
    }
}