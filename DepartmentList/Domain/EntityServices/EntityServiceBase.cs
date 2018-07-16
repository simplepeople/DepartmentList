using System;
using System.Data.Entity;
using System.Linq;
using DepartmentList.Domain.Entities;

namespace DepartmentList.Domain.EntityServices
{
    public abstract class EntityServiceBase<TContext, TEntity> : IDisposable where TContext : DbContext where TEntity : class, IEntity
    {
        protected TContext Context { get; set; }

        public void Dispose()
        {
            Context?.Dispose();
        }

        protected void Save()
        {
            Context.SaveChanges();
        }

        public void Update(TEntity item)
        {
            var entity = Entities.Find(item.Id);
            if (entity == null)
            {
                return;
            }
            Context.Entry(entity).CurrentValues.SetValues(item);
        }

        private DbSet<TEntity> _entities;
        protected DbSet<TEntity> Entities
        {
            get
            {
                return _entities ?? (_entities = (DbSet<TEntity>)Context.GetType().GetProperties()
                           .First(x => x.PropertyType == typeof(DbSet<TEntity>)).GetValue(Context));
            }
        }
    }
}