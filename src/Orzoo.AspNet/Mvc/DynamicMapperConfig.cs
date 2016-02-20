using AutoMapper;
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
            Mapper.Initialize(cfg =>
            {
                var types = RuntimeHelper.GetCurrentDomainImplementTypes(typeof (Profile));

                foreach (var type in types)
                {
                    cfg.CallGenericMethod(nameof(cfg.AddProfile), type);
                }
            });
        }

        #endregion

        #endregion
    }
}