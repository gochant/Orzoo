using Orzoo.AspNet.Mvc;

namespace StandardWeb
{
    public class CacheConfig
    {
        #region Methods

        #region Public Methods

        public static void Configure()
        {
            DynamicCacheConfig.Configure();
        }

        #endregion

        #endregion
    }
}