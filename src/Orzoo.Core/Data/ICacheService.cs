using System.Collections.Generic;

namespace Orzoo.Core.Data
{

    public interface ICacheService
    {
        bool UseCache { get; set; }
        void UpdateCache();

    }

    public interface ICacheService<T>: ICacheService
    {
        List<T> GetCache();
    }
}