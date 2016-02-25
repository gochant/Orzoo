using System.Configuration;
using System.Web.Mvc;
using Orzoo.AspNet.Mvc;
using Orzoo.Core.Data;
using Orzoo.Core.Extensions;

namespace Orzoo.AspNet.Logging
{
    /// <summary>
    /// 自动记录用户活动
    /// </summary>
    public class UserActivityAttribute : ActionFilterAttribute
    {
        #region Methods

        #region Public Methods

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is EmptyResult)
            {
                return;
            }
            var enableLog = ConfigurationManager.AppSettings["LogUserActivity"];
            if(enableLog != null && enableLog == "false")
            {
                return;
            }

            var actionName = filterContext.GetActionName();
            var fb = filterContext.GetFeedback();

            if (fb == null || !fb.Success)
            {
                return;
            }

            if (fb.Temp.ContainsKey("activity"))
            {
                var activity = fb.Temp["activity"] as UserActivity;
                UserActivityTracker.Record(activity);
            }
            else
            {
                if (!fb.Temp.ContainsKey("entity")) return;
                var entity = fb.Temp["entity"];
                UserActivityTracker.Record(filterContext.Controller.ControllerContext, new BasicEntity
                {
                    Id = entity.GetPropertyValue("Id")?.ToString(),
                    Name = entity.GetPropertyValue("Name")?.ToString()
                }, entity.GetType(), actionName);
            }
        }

        #endregion

        #endregion
    }
}