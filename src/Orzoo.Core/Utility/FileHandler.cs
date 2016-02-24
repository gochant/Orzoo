using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orzoo.Core.Utility
{
    public class FileHandler
    {
        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="filename">文件</param>
        public static void Delete(string filename)
        {
            File.Delete(filename);
        }

        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="path">路径名</param>
        /// <param name="filename">文件名称</param>
        /// <returns>内容</returns>
        public static byte[] GetFileContent(string path, string filename)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }

            var filepathname = Path.Combine(path, filename);
            if (File.Exists(filepathname))
            {
                var bytes = File.ReadAllBytes(filepathname);

                return bytes;
            }
            return null;
        }

        /// <summary>
        /// 获取存在的所有文件
        /// </summary>
        /// <param name="basePath">基础路径</param>
        /// <param name="fileName">文件</param>
        /// <returns></returns>
        public static IEnumerable<string> GetExistFilePaths(string basePath, string fileName)
        {
            var dir = new DirectoryInfo(basePath);
            return dir.GetFiles(fileName, SearchOption.AllDirectories).Select(f => f.FullName.Replace(basePath, string.Empty));
        }

        /// <summary>
        /// 确保路径存在
        /// </summary>
        /// <param name="path">路径</param>
        public static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
