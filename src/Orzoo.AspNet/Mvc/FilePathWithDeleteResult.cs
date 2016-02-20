using System.Web;
using System.Web.Mvc;

namespace Orzoo.AspNet.Mvc
{
    public class FilePathWithDeleteResult : FilePathResult
    {
        #region Constructors

        public FilePathWithDeleteResult(string fileName, string contentType) : base(fileName, contentType)
        {
        }

        #endregion

        #region Methods

        #region Protected Methods

        protected override void WriteFile(HttpResponseBase response)
        {
            base.WriteFile(response);
            //File.Delete(FileName);
        }

        #endregion

        #endregion
    }
}