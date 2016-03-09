using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.AspNet.Mvc;

namespace Orzoo.AspNet.Tests
{
    public class TestController : BaseController
    {
        public ActionResult TestAction()
        {
            return Json(new TestClass());
        }
    }

    public class TestClass
    {
        public string StringValue { get; set; } = "StringValue";
        public int IntValue { get; set; } = 1;
        public DateTime DateTimeValue { get; set; } = new DateTime(2011, 1, 1);
    }
}
