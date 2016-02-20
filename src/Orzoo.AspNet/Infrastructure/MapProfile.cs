using System;
using System.Collections.Generic;
using AutoMapper;

namespace Orzoo.AspNet.Infrastructure
{
    public class MapProfile : Profile
    {
        protected Dictionary<string, object> Mappings { get; set; } = new Dictionary<string, object>();

        public IMappingExpression<TSrc, TDest> AddMap<TSrc, TDest>(Func<IMappingExpression<TSrc, TDest>, IMappingExpression<TSrc, TDest>> func = null)
        {
            var map = Mapper.CreateMap<TSrc, TDest>();
            if(func != null)
            {
                map = func(map);
            }

            Mappings.Add(GetMapKey<TSrc, TDest>(), map);

            return map;
        }

        protected string GetMapKey<TSrc, TDest>()
        {
           return typeof (TSrc).FullName + typeof (TDest).FullName;
        }

        public IMappingExpression<TSrc, TDest> GetMap<TSrc, TDest>()
        {
            var key = GetMapKey<TSrc, TDest>();
            return Mappings.ContainsKey(key) ? (IMappingExpression<TSrc, TDest>)Mappings[key] : null;
        }

        public void ClearMapCache()
        {
            Mappings.Clear();
        }

        protected override void Configure()
        {
            CustomConfigure();
            ClearMapCache();
        }

        public virtual void CustomConfigure()
        {
        }
    }
}