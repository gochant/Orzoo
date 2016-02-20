using System;
using System.Web.Routing;
using Orzoo.Core;

namespace Orzoo.AspNet.Mvc
{
    public static class TypeHelper
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// 对象转 RouteValue Dictionary
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RouteValueDictionary ObjectToDictionary(object value)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();

            if (value != null)
            {
                foreach (PropertyHelper helper in PropertyHelper.GetProperties(value))
                {
                    dictionary.Add(helper.Name, helper.GetValue(value));
                }
            }

            return dictionary;
        }

        /// <summary>
        /// 测试日期（年、月、日）的有效性
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns>异常</returns>
        public static LogicException TestDateValid(int year, int month, int day)
        {
            var exception = new LogicException("数据无效");
            if (year < 0 || year > 9999)
            {
                exception.Key = "Year";
            }
            if (month > 0 && month <= 12)
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);
                if (day > daysInMonth)
                {
                    exception.Key = "Day";
                }
                else
                {
                    return null;
                }
            }
            else
            {
                exception.Key = "Month";
            }
            return exception;
        }

        #endregion

        #endregion
    }
}