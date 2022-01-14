using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.RepositoryAbstractions
{
    /// <summary>
    /// Repository pattern - define base for queryables to decouple EF
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        void Add(T entity);
        ValueTask<T> FindAsync(params object[] keyValues);
        void Update(T entity);
        void Remove(T entity);
    }
}
