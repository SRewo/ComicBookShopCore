using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.ComicBook
{
    public interface IComicBookRepositoryAsync
    {
        Task<IQueryable<Data.ComicBook>> GetListAsync();
        Task<Data.ComicBook> GetByIdAsync(int id);
        Task AddAsync(Data.ComicBook comic);
        Task UpdateAsync(Data.ComicBook comic);
        Task DeleteAsync(Data.ComicBook comic);
    }
}