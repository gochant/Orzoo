namespace Orzoo.AspNet.Caching
{
    public class GlobalCachingProvider : CachingProviderBase, IGlobalCachingProvider
    {
        #region Singleton 

        protected GlobalCachingProvider()
        {
        }

        public static GlobalCachingProvider Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            #region Static Fields and Constants

            internal static readonly GlobalCachingProvider instance = new GlobalCachingProvider();

            #endregion

            #region Constructors

            static Nested()
            {
            }

            #endregion
        }

        #endregion

        #region ICachingProvider

        public new virtual void AddItem(string key, object value)
        {
            base.RemoveItem(key); // 移除原有的项
            base.AddItem(key, value);
        }

        public virtual object GetItem(string key)
        {
            return base.GetItem(key, false); // 默认不移除
        }

        public new virtual object GetItem(string key, bool remove)
        {
            return base.GetItem(key, remove);
        }

        #endregion
    }
}