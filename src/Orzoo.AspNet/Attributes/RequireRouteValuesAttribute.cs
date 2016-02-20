using System.Reflection;
using System.Web.Mvc;

namespace Orzoo.AspNet.Attributes
{
    /// <summary>
    /// 检测路由值 属性
    /// </summary>
    /// <remarks>http://stackoverflow.com/questions/1045316/asp-net-mvc-ambiguous-action-methods</remarks>
    public class RequireRouteValuesAttribute : ActionMethodSelectorAttribute
    {
        #region Constructors

        public RequireRouteValuesAttribute(params string[] valueNames)
        {
            ValueNames = valueNames;
        }

        #endregion

        #region Properties, Indexers

        public string[] ValueNames { get; private set; }

        #endregion

        #region Methods

        #region Public Methods

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var contains = false;
            foreach (var value in ValueNames)
            {
                contains = controllerContext.Controller.ValueProvider.GetValue(value) != null;
                if (!contains) break;
            }
            return contains;
        }

        #endregion

        #endregion
    }
}