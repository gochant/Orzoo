using System;
using System.Web.Mvc;
using System.Web.Routing;
using Orzoo.AspNet.Mvc;
using Orzoo.Core;

namespace Orzoo.AspNet.Attributes
{
    /// <summary>
    /// 验证会话过期（未使用）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionTimeoutFilterAttribute : ActionFilterAttribute
    {
        #region Methods

        #region Public Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            bool skip = filterContext.ActionDescriptor.IsDefined(typeof (AllowAnonymousAttribute), inherit: true)
                        ||
                        filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute),
                            inherit: true);
            var isNew = session.IsNewSession;
            if (!skip && (filterContext.HttpContext.User.Identity.Name == null))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    JsonResult result = new JsonNetResult()
                    {
                        Data = Feedback.CreateFail("会话过期", "SessionTimeout"),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    filterContext.Result = result;
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"Controller", "Account"},
                        {"Action", "Login"}
                    });
                }
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion

        #endregion
    }
}