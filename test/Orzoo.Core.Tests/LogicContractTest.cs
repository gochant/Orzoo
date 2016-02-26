using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Utility;

namespace Orzoo.Core.Tests
{
    [TestClass]
    public class LogicContractTest
    {
        [TestMethod]
        public void Assert_call()
        {
            LogicContract.Assert(true);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void Assert_call_fail()
        {
            LogicContract.Assert(false);
        }
    }
}