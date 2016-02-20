using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orzoo.Core.Utility
{
    public class FileHandler
    {

        public static void Delete(string filename)
        {
            File.Delete(filename);
        }

        public static byte[] GetFileContent(string path, string filename)
        {
            EnsureDirectory(path);

            var filepathname = Path.Combine(path, filename);
            if (File.Exists(filepathname))
            {
                var bytes = File.ReadAllBytes(filepathname);

                return bytes;
            }
            return null;
        }

        public static IEnumerable<string> GetExistFilePaths(string basePath, string fileName)
        {
            var dir = new DirectoryInfo(basePath);
            return dir.GetFiles("main.js", SearchOption.AllDirectories).Select(f => f.FullName.Replace(basePath, string.Empty));
        }

        private static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
