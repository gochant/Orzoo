using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Orzoo.Core;
using Orzoo.Core.Extensions;

namespace Orzoo.AspNet.Mvc
{
    public static class ActionExtensions
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// 获取 Action 全名称
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string GetFullName(this ActionDescriptor x)
        {
            return x.ControllerDescriptor.ControllerType.FullName + '.' + x.ActionName;
        }

        /// <summary>
        /// 获取 Controller 名称
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string GetControllerDisplay(this ActionDescriptor x)
        {
            return x.ControllerDescriptor.ControllerType.GetDisplayName().Replace("Controller", string.Empty);
        }

        /// <summary>
        /// 获取 Action 显示名称
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string GetDisplayName(this ActionDescriptor x)
        {
            var attr = x.GetCustomAttributes(typeof (DisplayAttribute), true).FirstOrDefault();
            return attr != null ? ((DisplayAttribute) attr).Name : null;
        }

        /// <summary>
        /// 获取 Action 用户友好名称
        /// </summary>
        /// <param name="x"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public static string GetFriendlyName(this ActionDescriptor x, Dictionary<string, string> mapping = null)
        {
            var key = x.ActionName;
            string value;
            // 获取映射
            mapping.TryGetValue(key, out value);
            if (string.IsNullOrEmpty(value))
            {
                value = x.GetDisplayName() ?? x.ActionName;
            }
            return value;
        }

        /// <summary>
        /// 获取当前Action的权限角色名称
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string GetRoleName(this ActionDescriptor x)
        {
            return x.GetFullName().Replace("Controllers", string.Empty)
                .Replace("Controller", string.Empty).Replace(".", string.Empty);
        }

        public static Feedback GetFeedback(this ActionExecutedContext context)
        {
            return (context.Result as JsonResult)?.Data as Feedback;
        }

        public static string GetActionName(this ActionExecutedContext context)
        {
            return context.ActionDescriptor.GetDisplayName() ?? context.ActionDescriptor.ActionName;
        }

        #endregion

        #endregion
    }
}