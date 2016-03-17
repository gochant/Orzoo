using System.Data.Entity;
using Orzoo.AspNet.Infrastructure;
using Orzoo.AspNet.Logging;

namespace StandardWeb.Data
{
    public class ApplicationDbContext : DynamicDbContext, ILogDbContext
    {
        #region Constructors

        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new DatabaseReleaseInitializer());
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}