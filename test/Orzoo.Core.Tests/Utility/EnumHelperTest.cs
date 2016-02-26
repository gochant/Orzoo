using System;
using System.Fakes;
using System.IO.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Utility;

namespace Orzoo.Core.Tests.Utility
{
    [TestClass]
    public class EnumHelperTest
    {
        [TestMethod]
        public void GetDisplays_call()
        {
            var r = EnumHelper.GetDisplays<TestEnum>();
            Assert.AreEqual(2, r.Count);
            Assert.AreEqual("display", r[0]);
            Assert.AreEqual("B", r[1]);
        }

        [TestMethod]
        public void GetDescriptions_call()
        {
            var r = EnumHelper.GetDescriptions<TestEnum>();
            Assert.AreEqual(2, r.Count);
            Assert.AreEqual("description", r[0]);
            Assert.AreEqual(null, r[1]);
        }

        [TestMethod]
        public void Parse_call()
        {
            var r = EnumHelper.Parse<TestEnum>("A");
            Assert.AreEqual(TestEnum.A, r);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_call_no_exist()
        {
            var r = EnumHelper.Parse<TestEnum>("C");
        }
    }
}