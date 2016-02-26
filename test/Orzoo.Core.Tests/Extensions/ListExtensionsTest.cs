using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Tests.Extensions
{


    [TestClass]
    public class ListExtensionsTest
    {
        [TestMethod]
        public void ToDataTable_T_call()
        {
            var list = TestClass.CreateTestInstanceList();
            var len = list.Count;

            var r = list.ToDataTable();

            Assert.AreEqual(len, r.Rows.Count);
        }
    }
}
