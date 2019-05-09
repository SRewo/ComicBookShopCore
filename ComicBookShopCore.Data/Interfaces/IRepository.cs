

using System.Linq;

namespace ComicBookShopCore.Data.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetAll();
        T GetById(int id);
        void Reload(T entity);
        bool CanOpen();
    }
}