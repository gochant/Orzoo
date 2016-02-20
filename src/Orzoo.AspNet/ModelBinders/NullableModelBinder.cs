using System;
using System.Web.Mvc;

namespace Orzoo.AspNet.ModelBinders
{
    /// <summary>
    /// 可空类型的模型绑定（修复 ASP.NET MVC 3）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullableModelBinder<T> : IModelBinder
    {
        #region Methods

        #region Private Methods

        private static object ConvertValue(Type t, string value)
        {
            if (typeof (double) == t)
            {
                return Convert.ToDouble(value);
            }
            if (typeof (decimal) == t)
            {
                return Convert.ToDecimal(value);
            }
            if (typeof (DateTime) == t)
            {
                return Convert.ToDateTime(value);
            }
            return Convert.ToInt32(value);
        }

        #endregion

        #endregion

        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var modelState = new ModelState {Value = valueProviderResult};

            object value = null;

            if (!string.IsNullOrWhiteSpace(valueProviderResult.AttemptedValue))
            {
                try
                {
                    value = ConvertValue(typeof (T), valueProviderResult.AttemptedValue);
                }
                catch (FormatException ex)
                {
                    modelState.Errors.Add(ex);
                }
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

            return value;
        }

        #endregion
    }
}