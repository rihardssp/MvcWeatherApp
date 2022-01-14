using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.RepositoryAbstractions
{
    /// <summary>
    /// Repository pattern - define concrete logic for accessing EF with any entity
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public DbContext Context { get; set; }
        public RepositoryBase(DbContext context)
        {
            Context = context;
        }

        public IQueryable<T> FindAll()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public ValueTask<T> FindAsync(params object[] keyValues)
        {
            return Context.Set<T>().FindAsync(keyValues);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
    }
}
