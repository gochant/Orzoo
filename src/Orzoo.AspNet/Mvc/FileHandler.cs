using System.Web;

namespace Orzoo.AspNet.Mvc
{
    public class FileHandler
    {
        #region Methods

        #region Public Methods

        public static string GetAttachmentPath()
        {
            var server = HttpContext.Current.Server;
            return server.MapPath("~/App_Data/Attachments/");
        }

        #endregion

        #endregion
    }
}