using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace testmvc.Repository
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        T GetByID(object id);

        void Insert(T entry);

        void Delete(object id);

        void Delete(T entry);

        void Update(T entry);

        int Count();

        void Save();
    }

     
}
