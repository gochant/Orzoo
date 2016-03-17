using System.Data.Entity;

namespace StandardWeb.Data
{
    /// <summary>
    /// 数据库初始化（Release模式）
    /// </summary>
    public class DatabaseReleaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        #region Methods

        #region Protected Methods

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
        }

        #endregion

        #endregion
    }
}