

using System.Linq;

namespace ComicBookShopCore.Data.Interfaces
{
    public interface IRepository<T> : IEditable<T>, IOpenable<T>
    {
        void Reload(T entity);
    }
}