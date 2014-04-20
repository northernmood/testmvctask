using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace testmvc.Repository
{
    public abstract class RepositoryBase<T> : IDisposable, IRepository<T> where T : class 
    {
        protected DbContext context;
        private bool disposed = false;

        public RepositoryBase(DbContext context)
        {
            this.context = context;
        }

        public int Count()
        {
            return context.Set<T>().Count();
        }

        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
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
            else
            {
                return query.ToList();
            }
        }

        public T GetByID(object id)
        {
            return context.Set<T>().Find(id);
        }

        public void Insert(T entry)
        {
            context.Set<T>().Add(entry);
        }

        public void Delete(object id)
        {
            T entityToDelete = context.Set<T>().Find(id);
            Delete(entityToDelete);
        }

        public void Delete(T entry)
        {
            if (context.Entry(entry).State == EntityState.Detached)
            {
                context.Set<T>().Attach(entry);
            }

            context.Set<T>().Remove(entry);
        }

        public void Update(T entry)
        {
            context.Set<T>().Attach(entry);
            context.Entry(entry).State = EntityState.Modified;
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

        public void Save()
        {
            context.SaveChanges();
        }
    }
}