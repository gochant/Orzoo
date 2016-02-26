using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Tests.Extensions
{


    [TestClass]
    public class ObjectExtensionsTest
    {
        [TestMethod]
        public void CallGenericMethod_call()
        {
            var data = new TestClass();
            var r = data.CallGenericMethod("CreateClass", data.GetType(), Type.Missing);
            Assert.AreEqual(data.GetType(), r.GetType());
        }

        [TestMethod]
        public void CallGenericMethod_call_with_params()
        {
            var data = new TestClass();
            var r = data.CallGenericMethod("CreateClass", data.GetType(), false);
            Assert.IsNull(r);
        }

        [TestMethod]
        public void GetTypeDisplayName_call()
        {
            var data = new TestClass();
            Assert.AreEqual("Test Class", data.GetTypeDisplayName());
        }


        [TestMethod]
        public void GetTypeFullName_call()
        {
            var data = new TestClass();
            Assert.AreEqual("Orzoo.Core.Tests.TestClass", data.GetTypeFullName());
        }

        [TestMethod]
        public void GetPropertyValue_T_call()
        {
            var data = TestClass.CreateTestInstance();
            var r = data.GetPropertyValue<string>("StringValue");
            Assert.AreEqual(TestClass.DefaultStringValue, r);
        }

        [TestMethod]
        public void GetPropertyValue_T_call_valueType_member()
        {
            var data = TestClass.CreateTestInstance();
            var r = data.GetPropertyValue<int>("IntValue");
            Assert.AreEqual(TestClass.DefaultIntValue, r);
        }


        [TestMethod]
        public void GetPropertyValue_T_call_no_property()
        {
            var data = TestClass.CreateTestInstance();
            var r = data.GetPropertyValue<int>("Invalid");
            Assert.AreEqual(default(int), r);
        }

        [TestMethod]
        public void GetPropertyValue_call()
        {
            var data = TestClass.CreateTestInstance();
            var r = data.GetPropertyValue("StringValue");
            Assert.AreEqual(TestClass.DefaultStringValue, r);
        }
    }
}
