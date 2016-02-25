using System.Fakes;
using System.IO.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Utility;

namespace Orzoo.Core.Tests.Utility
{
    [TestClass]
    public class FileHandlerTest
    {

         public void GetFileContent_call()
         {
            using (ShimsContext.Create())
            {
                ShimFile.ExistsString = (path) => true;
                ShimDirectory.ExistsString = (path) => true;
                ShimFile.ReadAllBytesString = (path) => { return new byte[] { }; };

                var result = FileHandler.GetFileContent("123", "456");
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Length);
            }

        }
    }
}