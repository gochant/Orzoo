using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Tests.Extensions
{


    [TestClass]
    public class TypeExtensionsTest
    {
        [TestMethod]
        public void GetAttribute_call()
        {
            var r = typeof (TestClass).GetAttribute<DisplayNameAttribute>();
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void GetPropertyDisplayName_call()
        {
            var r = typeof (TestClass).GetPropertyDisplayName("StringValue");
            Assert.AreEqual("String Value", r);
        }

        [TestMethod]
        public void GetPropertyDisplayName_call_no_prop()
        {
            var r = typeof(TestClass).GetPropertyDisplayName("Invalid");
            Assert.AreEqual(null, r);
        }


        [TestMethod]
        public void GetRealType_call()
        {
           var type = typeof (int?).GetRealType();
            Assert.AreEqual(typeof(int), type);
        }

        [TestMethod]
        public void GetRealType_call_normal()
        {
            var type = typeof(TestClass).GetRealType();
            Assert.AreEqual(typeof(TestClass), type);
        }

        [TestMethod]
        public void GetJsType_call()
        {
            var r = typeof(string).GetJsType();
            Assert.AreEqual("string", r);
        }

        [TestMethod]
        public void GetDefault_call()
        {
            var r = typeof(string).GetDefault();
            Assert.AreEqual(null, r);
        }

        [TestMethod]
        public void GetDefault_call_int()
        {
            var r = typeof(int).GetDefault();
            Assert.AreEqual(0, r);
        }
    }
}
