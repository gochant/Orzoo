using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Moq;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Orzoo.Core.Data;
using Orzoo.Core.Enums;

namespace Orzoo.Core.Tests
{
    // https://msdn.microsoft.com/en-us/library/dn314429.aspx

    public class BloggingContext : DbContext
    {
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        public static BloggingContext CreateMockContext()
        {
            var blogs = new List<Blog>
            {
                new Blog { Name = "BBB", Flag = DataFlag.Invalid },
                new Blog { Name = "ZZZ", Url = "BBB" },
                new Blog { Name = "ZZZ", Url = "AAA" },
                new Blog { Name = "AAA" },
            }.AsQueryable();
            var posts = new List<Post>
            {
                new Post { Title = "BBB", CreatedDate = DateTime.Now },
                new Post { Title = "ZZZ", CreatedDate = DateTime.Now.AddDays(1) },
                new Post { Title = "AAA", CreatedDate = DateTime.Now },
            }.AsQueryable();

            var mockContext = new Mock<BloggingContext>(); 

            mockContext.Setup(c => c.Blogs).Returns(CreateMockSet(blogs).Object);
            mockContext.Setup(c => c.Posts).Returns(CreateMockSet(posts).Object);

            return mockContext.Object;
        }

        protected static Mock<DbSet<T>> CreateMockSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
    }

    public class Blog : IFlagEntity
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
        public DataFlag? Flag { get; set; }
    }

    public class Post : IMetadataEntity
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
    }


    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}