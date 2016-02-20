using System;
using System.Runtime.Caching;

namespace Orzoo.AspNet.Caching
{
    public abstract class CachingProviderBase
    {
        #region Static Fields and Constants

        private static readonly object padlock = new object();

        #endregion

        #region Fields

        protected MemoryCache cache = new MemoryCache("CachingProvider");

        #endregion

        #region Constructors

        public CachingProviderBase()
        {
            DeleteLog();
        }

        #endregion

        #region Methods

        #region Protected Methods

        protected virtual void AddItem(string key, object value)
        {
            lock (padlock)
            {
                cache.Add(key, value, DateTimeOffset.MaxValue);
            }
        }

        protected virtual void RemoveItem(string key)
        {
            lock (padlock)
            {
                cache.Remove(key);
            }
        }

        protected virtual object GetItem(string key, bool remove)
        {
            lock (padlock)
            {
                var res = cache[key];

                if (res != null)
                {
                    if (remove == true)
                        cache.Remove(key);
                }
                else
                {
                    WriteToLog("CachingProvider-GetItem: Don't contains key: " + key);
                }

                return res;
            }
        }

        #endregion

        #endregion

        #region Error Logs

        private string LogPath = System.Environment.GetEnvironmentVariable("TEMP");

        protected void DeleteLog()
        {
            System.IO.File.Delete(string.Format("{0}\\CachingProvider_Errors.txt", LogPath));
        }

        protected void WriteToLog(string text)
        {
            using (
                System.IO.TextWriter tw =
                    System.IO.File.AppendText(string.Format("{0}\\CachingProvider_Errors.txt", LogPath)))
            {
                tw.WriteLine(text);
                tw.Close();
            }
        }

        #endregion
    }
}