using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Orzoo.Core.Utility
{
    public class XmlHelper
    {
        public static T Load<T>(XDocument doc)
        {
            var serializer = new XmlSerializer(typeof(T));
            var result = (T)serializer.Deserialize(doc.CreateReader());
            return result;
        }

        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static T Load<T>(string path)
        {
            var doc = XDocument.Load(path);
            return Load<T>(doc);
        }

        /// <summary>
        /// 保存XML
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="path">保存路径</param>
        public static void SaveTo(object data, string path)
        {
            var writer = File.CreateText(path);
            SaveTo(data, writer);
        }

        public static void SaveTo(object data, StreamWriter writer)
        {
            var serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(writer, data);
        }
    }
}
