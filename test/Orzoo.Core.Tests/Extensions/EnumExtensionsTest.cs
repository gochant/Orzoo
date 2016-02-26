using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Tests.Extensions
{


    [TestClass]
    public class EnumExtensionsTest
    {
        [TestMethod]
        public void GetDisplay_call()
        {
            var em = TestEnum.A;
            var result = em.GetDisplay();
            Assert.AreEqual("display", result);
        }

        [TestMethod]
        public void GetDescription_call()
        {
            var em = TestEnum.A;
            var result = em.GetDescription();
            Assert.AreEqual("description", result);
        }
    }
}
