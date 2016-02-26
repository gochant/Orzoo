using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Extensions;
using Orzoo.Core.Linq;

namespace Orzoo.Core.Tests.Extensions
{


    [TestClass]
    public class DataSourceResultExtensionsTest
    {
        [TestMethod]
        public void ToFeedback_call()
        {
            var fd = new DataSourceResult() {
                Data = TestClass.CreateTestInstanceList(),
                Total = 2
            }.ToFeedback();

            Assert.AreEqual(2, fd.Total);
            Assert.AreEqual(typeof(List<TestClass>), fd.Data.GetType());
        }

        [TestMethod]
        public void ToFeedback_call_aggregates()
        {
            // TODO:
        }

    }
}
