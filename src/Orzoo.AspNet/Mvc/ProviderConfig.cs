using System.Linq;
using System.Web.Mvc;
using Orzoo.AspNet.ModelBinders;

namespace Orzoo.AspNet.Mvc
{
    public class ProviderConfig
    {
        #region Methods

        #region Public Methods

        public static void ApplyFilterEnumModelBinder()
        {
            // 为了修复 Kendo Filter 传入 日期类型不能被识别的bug
            ValueProviderFactories.Factories.Remove(
                ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());

            System.Web.Mvc.ModelBinders.Binders.DefaultBinder = new EnumModelBinder();
        }

        #endregion

        #endregion
    }
}