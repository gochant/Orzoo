using System.Web;
using System.Web.Mvc;
using Orzoo.AspNet.Attributes;
using Orzoo.Core;

namespace Orzoo.AspNet.Mvc
{
    //[Authorize]
    [ValidateModelState]
    public class BaseController : Controller
    {
        #region Methods

        #region Protected Methods

        protected string EncodeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            if (!Request.Browser.Browser.ToUpper().Contains("FIREFOX"))
                fileName = HttpUtility.UrlEncode(fileName);

            return fileName;
        }

        #endregion

        #endregion

        #region 重写JsonResult

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data
            };
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data,
                JsonRequestBehavior = behavior
            };
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var result = filterContext.Result as JsonResult;
            if (result != null)
            {
                var feedback = result.Data as Feedback;
                if (feedback != null)
                {
                    feedback.temp = null;
                    result.Data = feedback;
                    filterContext.Result = result;
                }
            }

            base.OnResultExecuting(filterContext);
        }

        #endregion

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    base.OnException(filterContext);
        //}
    }
}