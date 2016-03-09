using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.AspNet.Mvc;

namespace Orzoo.AspNet.Tests.Mvc
{
    [TestClass]
    public class BaseControllerTest
    {
        [TestMethod]
        public void Json_call()
        {
            var controller = new TestController();
            var r = controller.TestAction();

            Assert.IsNotNull(r);
        }


    }
}