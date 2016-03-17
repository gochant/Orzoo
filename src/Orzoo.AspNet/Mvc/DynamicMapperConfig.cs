using System;
using AutoMapper;
using Orzoo.AspNet.Infrastructure;
using Orzoo.Core.Extensions;
using Orzoo.Core.Utility;

namespace Orzoo.AspNet.Mvc
{
    public class DynamicMapperConfig
    {
        #region Methods

        #region Public Methods

        public static void Configure()
        {
            new MapperConfiguration(cfg =>
            {
                var types = RuntimeHelper.GetCurrentDomainImplementTypes(typeof(MapProfile));

                foreach (var type in types)
                {
                    cfg.AddProfile((Profile)Activator.CreateInstance(type));
                }
            });
        }

        #endregion

        #endregion
    }
}