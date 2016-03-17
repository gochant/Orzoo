using System.Linq;
using System.Web.Mvc;
using Orzoo.Core.Utility;

namespace StandardWeb.Controllers
{
    public class HomeController : Controller
    {
        #region Methods

        #region Public Methods

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     获取菜单
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Menu()
        {
            var sitemap = MvcSiteMapProvider.SiteMaps.Current;
            return PartialView(sitemap);
        }

        public ActionResult GetModules()
        {
            var basePath = Server.MapPath("~/app/modules");

            var result = FileHandler.GetExistFilePaths(basePath, "main.js").Select(f => f
            .Replace("\\", string.Empty)
            .Replace("-", string.Empty));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion
    }
}