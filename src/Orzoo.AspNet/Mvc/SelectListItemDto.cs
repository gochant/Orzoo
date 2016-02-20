using System.Web.Mvc;

namespace Orzoo.AspNet.Mvc
{
    public class SelectListItemDto : SelectListItem
    {
        #region Properties, Indexers

        public object Data { get; set; }
        public int Level { get; set; }

        #endregion
    }
}