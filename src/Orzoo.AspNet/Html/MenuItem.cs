#region

using System.Collections.Generic;

#endregion

namespace Orzoo.AspNet.Html
{
    /// <summary>
    /// 导航菜单项
    /// </summary>
    public class MenuItem
    {
        #region Properties, Indexers

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string Role { get; set; }

        public string ClassName { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public List<MenuItem> Items { get; set; }
            = new List<MenuItem>();

        #endregion
    }
}