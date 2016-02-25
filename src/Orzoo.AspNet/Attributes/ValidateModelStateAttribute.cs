using System.Web.Mvc;
using Orzoo.AspNet.Extensions;
using Orzoo.AspNet.Mvc;
using Orzoo.Core;

namespace Orzoo.AspNet.Attributes
{
    /// <summary>
    /// 处理传入参数中实体的 ModelState 错误
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        #region Static Fields and Constants

        protected static readonly string Key = "ModelStateTransfer";

        #endregion

        #region Properties, Indexers

        /// <summary>
        /// 是否启用验证实体的有效性
        /// </summary>
        public bool Enabled { get; set; } = true;

        #endregion

        #region Methods

        #region Protected Methods

        protected virtual void ProcessNormal(ActionExecutingContext filterContext)
        {
            ExportModelStateToTempData(filterContext);

            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        protected virtual void ProcessAjax(ActionExecutingContext filterContext)
        {
            var result = Feedback.CreateFail(data: filterContext.Controller.ViewData.ModelState.GetAllErrors());
            // var json = JsonConvert.SerializeObject(result);

            filterContext.Result = new JsonNetResult()
            {
                Data = result,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        protected static void ExportModelStateToTempData(ControllerContext context)
        {
            context.Controller.TempData[Key] = context.Controller.ViewData.ModelState;
        }

        protected static void ImportModelStateFromTempData(ControllerContext context)
        {
            var prevModelState = context.Controller.TempData[Key] as ModelStateDictionary;
            context.Controller.ViewData.ModelState.Merge(prevModelState);
        }

        protected static void RemoveModelStateFromTempData(ControllerContext context)
        {
            context.Controller.TempData[Key] = null;
        }

        #endregion

        #region Public Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid && Enabled)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    ProcessAjax(filterContext);
                }
                else
                {
                    ProcessNormal(filterContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion

        #endregion
    }
}