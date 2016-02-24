using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Extensions;

namespace Orzoo.Core.Tests.Extensions
{
    public enum TestEnum
    {
        [Display(Name = "测试")]
        [System.ComponentModel.Description("描述 ")]
        A,

        B
    }

    [TestClass]
    public class EnumExtensionsTest
    {
        [TestMethod]
        public void GetDisplay_call()
        {
            var em = TestEnum.A;
            var result = em.GetDisplay();
            Assert.AreEqual("测试", result);
        }

        [TestMethod]
        public void GetDescription_call()
        {
            var em = TestEnum.A;
            var result = em.GetDescription();
            Assert.AreEqual("测试", result);
        }
    }
}
