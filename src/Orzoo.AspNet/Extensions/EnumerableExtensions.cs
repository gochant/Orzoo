using System.Collections.Generic;
using AutoMapper;

namespace Orzoo.AspNet.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TDest> MapTo<TSrc, TDest>(this IEnumerable<TSrc> list)
        {
            return Mapper.Map<IEnumerable<TSrc>, IEnumerable<TDest>>(list);
        }
    }
}