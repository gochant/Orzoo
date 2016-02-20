using System.Data.Entity.Validation;
using System.Text;
using System.Web.Mvc;
using Orzoo.AspNet.Mvc;
using Orzoo.Core;

namespace Orzoo.AspNet.Attributes
{
    /// <summary>
    /// 异常处理Filter属性
    /// </summary>
    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        #region Methods

        #region Public Methods

        public override void OnException(ExceptionContext filterContext)
        {
            Feedback feedback;
            var ex = filterContext.Exception;

            // 逻辑异常
            if (ex is LogicException)
            {
                var logicEx = (LogicException) ex;
                feedback = Feedback.Fail(ex.Message, type: logicEx.Level);
            }
            else
            {
                // EF 验证异常
                if (ex is DbEntityValidationException)
                {
                    var dbEx = (DbEntityValidationException) ex;
                    feedback = Feedback.From(dbEx);
                }
                // 其他异常
                else
                {
                    feedback = Feedback.From(ex);
                }
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.ExceptionHandled = true; // 标识为已处理的异常

                // var json = JsonConvert.SerializeObject(feedback); HttpUtility.UrlEncode(json)
                filterContext.Result = new JsonNetResult()
                {
                    Data = feedback,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                // TODO: 处理非Ajax请求的情况
                filterContext.Result = new ContentResult()
                {
                    Content = feedback.msg,
                    ContentEncoding = Encoding.UTF8,
                    ContentType = "text/plain"
                };
            }
        }

        #endregion

        #endregion
    }
}