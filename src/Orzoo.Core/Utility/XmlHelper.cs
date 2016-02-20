using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Orzoo.Core.Utility
{
    public class XmlHelper
    {
        public static T Load<T>(string path)
        {
            var doc = XDocument.Load(path);
            var serializer = new XmlSerializer(typeof(T));
            var result = (T)serializer.Deserialize(doc.CreateReader());
            return result;
        }

        public static void SaveTo(object data, string path)
        {
            var serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(File.CreateText(path), data);
        }
    }
}
