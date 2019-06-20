using System.Linq;

namespace ComicBookShopCore.Data.Interfaces
{
    public interface IOpenable<T>
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        bool CanOpen();
    }
}