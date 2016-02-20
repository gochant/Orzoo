using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orzoo.Core;

namespace Orzoo.AspNet.Extensions
{
    public static class ModelStateExtensions
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// 获取 ModelState 中的所有错误
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<ViewError> GetAllErrors(this ModelStateDictionary modelState)
        {
            var result = new List<ViewError>();
            var erroneousFields = modelState.Where(ms => ms.Value.Errors.Any())
                .Select(x => new {x.Key, x.Value.Errors});


            foreach (var erroneousField in erroneousFields)
            {
                var fieldKey = erroneousField.Key;
                var fieldErrors = erroneousField.Errors
                    .Select(error => new ViewError(fieldKey, error.ErrorMessage));
                result.AddRange(fieldErrors);
            }

            return result;
        }

        #endregion

        #endregion
    }
}