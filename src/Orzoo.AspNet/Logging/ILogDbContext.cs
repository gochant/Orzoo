using System;
using Orzoo.AspNet.Infrastructure;

namespace Orzoo.AspNet.Logging
{
    public interface ILogDbContext : IDbContext, IDisposable
    {
    }
}