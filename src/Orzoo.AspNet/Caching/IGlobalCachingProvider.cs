namespace Orzoo.AspNet.Caching
{
    internal interface IGlobalCachingProvider
    {
        #region Methods

        #region Public Methods

        void AddItem(string key, object value);
        object GetItem(string key);

        #endregion

        #endregion
    }
}