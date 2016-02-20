using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orzoo.Core.Extensions;

namespace Orzoo.AspNet.Mvc
{
    public class EnumHelper
    {
        #region Methods

        #region Public Methods

        public static IEnumerable<SelectListItem> GetSelectList<T>(Func<Enum, string> textSelector = null,
            Func<Enum, string> valueSelector = null)
        {
            var t = typeof (T);
            if (textSelector == null) textSelector = (x) => x.GetDisplay();
            if (valueSelector == null) valueSelector = (x) => Convert.ToInt32(x).ToString();

            return !t.IsEnum
                ? new List<SelectListItem>()
                : Enum.GetValues(t).Cast<Enum>()
                    .Select(x => new SelectListItem
                    {
                        Text = textSelector(x),
                        Value = valueSelector(x)
                    }).ToList();
        }

        #endregion

        #endregion
    }
}