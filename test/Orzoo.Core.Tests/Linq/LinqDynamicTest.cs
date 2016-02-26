using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Dynamic;

namespace Orzoo.Core.Tests.Linq
{
    [TestClass]
    public class LinqDynamicTest
    {
        [TestMethod]
        public void Where_call_array_contains()
        {
            var context = BloggingContext.CreateMockContext();
            var search = new string[] { "AAA" };
            var r = context.Blogs.Where("@0.Contains(Name)", search);
            var expect = context.Blogs.Where(d => search.Contains(d.Name));

            Assert.AreEqual(expect.Count(), r.Count());
        }
    }
}