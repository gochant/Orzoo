#region

using System.Collections.Generic;
using System.Web.Routing;
using Orzoo.AspNet.Mvc;

#endregion

namespace Orzoo.AspNet.Html
{
    /// <summary>
    /// 模板参数
    /// </summary>
    public class TemplateParams
    {
        #region Fields

        private object additionalViewData;

        #endregion

        #region Properties, Indexers

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string HtmlFieldName { get; set; }

        /// <summary>
        /// 特性
        /// </summary>
        public List<TemplateFeature> Features { get; set; } = new List<TemplateFeature>();

        public RouteValueDictionary AdditionalViewDataDictionary { get; private set; } = new RouteValueDictionary();

        public object AdditionalViewData
        {
            get { return additionalViewData; }
            set
            {
                additionalViewData = value;
                AdditionalViewDataDictionary = TypeHelper.ObjectToDictionary(additionalViewData);
            }
        }

        public Dictionary<string, object> DefaultOptions { get; set; } = new Dictionary<string, object>();

        #endregion

        #region Methods

        #region Public Methods

        public void SetOptions(string key, string value, bool isForce = false)
        {
            if (isForce)
            {
                DefaultOptions[key] = value;
            }
            else
            {
                DefaultOptions[key] = (DefaultOptions.ContainsKey(key) ? DefaultOptions[key] : null) ?? value;
            }
        }

        public object GetAdditionalViewData(string key)
        {
            object result;
            AdditionalViewDataDictionary.TryGetValue(key, out result);
            if (result == null)
            {
                DefaultOptions.TryGetValue(key, out result);
            }
            return result;
        }

        public static object GetViewData(RouteValueDictionary dic, string key)
        {
            object result;
            dic.TryGetValue(key, out result);
            return result;
        }

        #endregion

        #endregion
    }
}