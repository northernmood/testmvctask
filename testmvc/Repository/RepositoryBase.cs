using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace testmvc.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class 
    {
        protected DbContext context;
        private bool disposed = false;

        protected RepositoryBase(DbContext context)
        {
            this.context = context;
        }

        public virtual int Count()
        {
            return context.Set<T>().Count();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public virtual T GetByID(object id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual void Insert(T entry)
        {
            context.Set<T>().Add(entry);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = context.Set<T>().Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entry)
        {
            if (context.Entry(entry).State == EntityState.Detached)
            {
                context.Set<T>().Attach(entry);
            }

            context.Set<T>().Remove(entry);
        }

        public virtual void Update(T entry)
        {
            context.Set<T>().Attach(entry);
            context.Entry(entry).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public IQueryable<T> All()
        {
            return context.Set<T>().AsQueryable<T>();
        }
    }
}