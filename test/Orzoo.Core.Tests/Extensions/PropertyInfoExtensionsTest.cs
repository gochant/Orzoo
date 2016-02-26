using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Tests.Extensions
{


    [TestClass]
    public class PropertyInfoExtensionsTest
    {
        [TestMethod]
        public void GetAttribute_T_call()
        {
            var r = typeof (TestClass).GetProperty("StringValue").GetAttribute<DisplayAttribute>();
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void GetAttribute_T_call_not_exists()
        {
            var r = typeof(TestClass).GetProperty("StringValue").GetAttribute<DisplayNameAttribute>();
            Assert.IsNull(r);
        }
    }
}
