using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace Orzoo.AspNet.ModelBinders
{
    /// <summary>
    /// 枚举类型模型绑定
    /// </summary>
    public class EnumModelBinder : DefaultModelBinder
    {
        #region Methods

        #region Protected Methods

        protected override object GetPropertyValue(ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyDescriptor.PropertyType);

            var propertyType = underlyingType ?? propertyDescriptor.PropertyType;
            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (null != providerValue)
            {
                var value = providerValue.RawValue;
                if (propertyType.IsEnum)
                {
                    if (null != value)
                    {
                        var valueType = value.GetType();
                        if (!valueType.IsEnum)
                        {
                            return Enum.ToObject(propertyType, int.Parse(value.ToString()));
                        }
                    }
                }
            }

            var result = base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);

            return result;
        }

        #endregion

        #endregion
    }
}