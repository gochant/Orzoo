using System.Data.Entity.Infrastructure;

namespace Orzoo.AspNet.Infrastructure
{
    public interface IDbContext
    {
        #region Methods

        #region Public Methods

        int SaveChanges();
        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        #endregion

        #endregion
    }
}