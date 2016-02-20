using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Orzoo.AspNet.Attributes
{
    /// <summary>
    /// 比某个值大于的验证属性
    /// </summary>
    public class GreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        #region Constructors

        public GreaterThanAttribute(string otherProperty)
            : base("{0} 必须大于 {1}")
        {
            OtherProperty = otherProperty;
        }

        #endregion

        #region Properties, Indexers

        public string OtherProperty { get; set; }

        #endregion

        #region Methods

        #region Protected Methods

        protected override ValidationResult IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);
            var otherName = GetOtherDisplayName(validationContext.ObjectType);
            if (firstComparable != null && secondComparable != null)
            {
                if (firstComparable.CompareTo(secondComparable) < 1)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName, otherName));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(ValidationContext validationContext)
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var secondValue = propertyInfo?.GetValue(validationContext.ObjectInstance, null);
            return secondValue as IComparable;
        }

        protected string GetOtherDisplayName(Type type)
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(
                null, type, OtherProperty);
            if (metadata != null)
            {
                return metadata.GetDisplayName();
            }
            return OtherProperty;
        }

        #endregion

        #region Public Methods

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        #endregion

        #endregion

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage =
                    FormatErrorMessage(metadata.GetDisplayName(), GetOtherDisplayName(metadata.ContainerType))
            };
            rule.ValidationParameters.Add("other", OtherProperty);
            rule.ValidationType = "greaterthan";
            yield return rule;
        }

        #endregion
    }
}