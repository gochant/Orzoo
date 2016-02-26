using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Utility;

namespace Orzoo.Core.Tests
{
    [TestClass]
    public class FeedbackTest
    {
        [TestMethod]
        public void CreateData_call()
        {
            var r = Feedback.CreateData(1);
            Assert.AreEqual(1, r.Data);
        }

        [TestMethod]
        public void CreateFail_call()
        {
            var r = Feedback.CreateFail(type: AlertType.Warning);
            Assert.AreEqual(Orzoo.Core.Properties.Resources.OperationFailed, r.Msg);
            Assert.AreEqual(AlertType.Warning, r.Type);

        }

        [TestMethod]
        public void CreateSuccess_call()
        {
            var r = Feedback.CreateSuccess();
            Assert.AreEqual(Orzoo.Core.Properties.Resources.OperationSuccessful, r.Msg);
        }

        [TestMethod]
        public void CreateList_call()
        {
            var data = TestClass.CreateTestInstanceList();
            var r = Feedback.CreateList(data, data.Count);

            Assert.AreEqual(data, r.Data);
            Assert.AreEqual(data.Count, r.Total);
            Assert.AreEqual(AlertType.Silent, r.Type);
        }

        [TestMethod]
        public void Create_call()
        {
            var r = Feedback.Create(true, string.Empty, null, AlertType.Info);

            Assert.AreEqual(null, r.Data);
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual(AlertType.Info, r.Type);
        }
    }
}