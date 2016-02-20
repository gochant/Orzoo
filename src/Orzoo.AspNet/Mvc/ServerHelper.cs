using System;
using System.IO;
using System.Web;

namespace Orzoo.AspNet.Mvc
{
    public class ServerHelper
    {
        #region Static Fields and Constants

        public static string TempPath = "~/Temp/";

        public static string TemplatePath = "~/SystemData/Template/";

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// 获取文件的服务器路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="filename">文件名</param>
        /// <returns>绝对路径</returns>
        protected static string GetServerPath(string relativePath, string filename)
        {
            var server = HttpContext.Current.Server;
            var path = server.MapPath(relativePath);
            return path + filename;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取临时文件夹文件路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="createRandom">是否创建一个随机文件名</param>
        /// <returns></returns>
        public static string GetTempPathFileName(string filename, bool createRandom = false)
        {
            if (createRandom)
            {
                filename = GetRandomFileName(filename);
            }
            return GetServerPath(TempPath, filename);
        }

        /// <summary>
        /// 获取模板文件夹文件路径
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static string GetTemplatePath(string filename)
        {
            return GetServerPath(TemplatePath, filename);
        }

        /// <summary>
        /// 获取随机文件名
        /// </summary>
        /// <param name="filename">原文件名</param>
        /// <returns></returns>
        public static string GetRandomFileName(string filename)
        {
            var fileExtension = Path.GetExtension(filename);
            var name = Path.GetFileNameWithoutExtension(filename);
            name += Guid.NewGuid().ToString() + fileExtension;
            return name;
        }

        #endregion

        #endregion
    }
}