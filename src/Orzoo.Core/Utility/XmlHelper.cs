using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Orzoo.Core.Utility
{
    public class XmlHelper
    {
        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static T Load<T>(string path)
        {
            var doc = XDocument.Load(path);
            var serializer = new XmlSerializer(typeof(T));
            var result = (T)serializer.Deserialize(doc.CreateReader());
            return result;
        }

        /// <summary>
        /// 保存XML
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="path">保存路径</param>
        public static void SaveTo(object data, string path)
        {
            var serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(File.CreateText(path), data);
        }
    }
}
