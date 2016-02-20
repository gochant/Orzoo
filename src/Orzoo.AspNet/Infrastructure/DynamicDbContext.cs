using System;
using System.Data.Entity;
using System.Linq;
using Orzoo.Core.Data;

namespace Orzoo.AspNet.Infrastructure
{
    public class DynamicDbContext : DbContext
    {
        #region Constructors

        public DynamicDbContext() : base()
        {
        }

        public DynamicDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        #endregion

        #region Methods

        #region Protected Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var entityMethod = typeof (DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var entityTypes = assembly
                    .GetTypes()
                    .Where(t => typeof (IDbEntity).IsAssignableFrom(t) && t.IsClass);

                foreach (var type in entityTypes)
                {
                    entityMethod.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] {});
                }
            }
        }

        #endregion

        #endregion
    }
}