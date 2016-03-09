using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.AspNet.Mvc;

namespace Orzoo.AspNet.Tests.Mvc
{
    [TestClass]
    public class ActionDescriptorExtensionsTest
    {
        [TestMethod]
        public void ActionDescriptorExtensions_GetFullName_call()
        {

            var controller = new TestController();
            var controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
            var actionDescriptor = controllerDescriptor.FindAction(controller.ControllerContext, "TestAction");
            var r = actionDescriptor.GetFullName();
            Assert.AreEqual("Orzoo.AspNet.Tests.TestController.TestAction", r);
        }
    }
}