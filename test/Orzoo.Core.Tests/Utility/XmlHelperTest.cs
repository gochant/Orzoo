using System.Fakes;
using System.IO;
using System.IO.Fakes;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Utility;

namespace Orzoo.Core.Tests.Utility
{
    [TestClass]
    public class XmlHelperTest
    {
        [TestMethod]
        public void Load_XDocument_call()
        {
            var data = TestClass.CreateTestInstance();
            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(writer, data);
            }

            var r = XmlHelper.Load<TestClass>(doc);

            Assert.IsNotNull(r);
            Assert.AreEqual(TestClass.DefaultStringValue, r.StringValue);
        }

        [TestMethod]
        public void SaveTo_call()
        {
            var data = TestClass.CreateTestInstance();

            var stream = new MemoryStream();
            XmlHelper.SaveTo(data, new StreamWriter(stream));

            Assert.IsTrue(stream.Length > 0);
        }
    }
}