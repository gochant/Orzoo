using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orzoo.Core.Linq;
using System.Linq;

namespace Orzoo.Core.Tests.Linq
{
    [TestClass]
    public class QueryableExtensionsTest
    {
        [TestMethod]
        public void ValidFilter_T_call()
        {
            var context = BloggingContext.CreateMockContext();

            var r = context.Blogs.ValidFilter();

            Assert.AreEqual(context.Blogs.Count() - 1, r.Count());
        }

        [TestMethod]
        public void ValidFilter_T_call_error_type()
        {
            var context = BloggingContext.CreateMockContext();
            var r = context.Posts.ValidFilter();
            Assert.AreEqual(context.Posts.Count(), r.Count());
        }

        [TestMethod]
        public void DateOrder_T_call()
        {
            var context = BloggingContext.CreateMockContext();
            var r = context.Posts.DateOrder();
            Assert.AreEqual("ZZZ", r.First().Title);
        }



        [TestMethod]
        public void DateOrder_T_call_error_type()
        {
            var context = BloggingContext.CreateMockContext();
            var r = context.Blogs.DateOrder();
            Assert.AreEqual(context.Blogs.Count(), r.Count());
        }

        [TestMethod]
        public void ToDataSourceResult_T_call()
        {
            var context = BloggingContext.CreateMockContext();
            var r = context.Blogs.ToDataSourceResult(new AdvanceDataSourceRequest {
                 Take = 1,
                 Skip = 1,
                  Sort = new System.Collections.Generic.List<Sort>
                  {
                      new Sort { Field = "Name", Dir = "asc" }
                  }
            });
            var data = ((IEnumerable<Blog>) r.Data);

            Assert.IsNotNull(r.Data);
            Assert.AreEqual(context.Blogs.Count(), r.Total);
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual("BBB", data.First().Name);
        }

        [TestMethod]
        public void ToDataSourceResult_T_call_two_sort()
        {
            var context = BloggingContext.CreateMockContext();
            var r = context.Blogs.ToDataSourceResult(new AdvanceDataSourceRequest
            {
                Sort = new System.Collections.Generic.List<Sort>
                  {
                      new Sort { Field = "Name", Dir = "asc" },
                      new Sort { Field = "Url", Dir = "desc" }
                  }
            });
            var data = ((IEnumerable<Blog>)r.Data);

            Assert.IsNotNull(r.Data);
            Assert.AreEqual("BBB", data.ElementAt(2).Url);
        }
    }
}