using System;
using System.Web.Mvc;
using Orzoo.AspNet.Mvc;
using Orzoo.Core;

namespace Orzoo.AspNet.Attributes
{
    /// <summary>
    /// 根据方法名配置进行权限验证
    /// </summary>
    public class BaseAuthorizeAttribute : AuthorizeAttribute
    {
        #region Methods

        #region Protected Methods

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // 权限状态
            var state = filterContext.HttpContext.User?.Identity?.IsAuthenticated == false
                ? AuthorizeState.UserNotFound
                : AuthorizeState.Unauthorized;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonNetResult
                {
                    Data = Feedback.Fail(data: Enum.GetName(typeof (AuthorizeState), state), type: AlertType.Silent),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                if (state == AuthorizeState.UserNotFound)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
                else
                {
                    if (state == AuthorizeState.Unauthorized)
                    {
                        filterContext.Result = new ContentResult() {Content = "Unauthorized!!"};
                    }
                }
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 授权状态
    /// </summary>
    public enum AuthorizeState
    {
        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized,

        /// <summary>
        /// 用户信息未找到
        /// </summary>
        UserNotFound
    }
}