using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.Artist
{
    public interface IAsyncArtistRepository
    {
        Task<IQueryable<Data.Artist>> GetAllAsync();
        Task<IEnumerable<Data.Artist>> GetWhereAsync(Expression<Func<Data.Artist, bool>> predicate);
        Task<Data.Artist> GetByIdAsync(int id);
        Task AddAsync(Data.Artist artist);
	Task UpdateAsync(Data.Artist artist);
        Task DeleteAsync(Data.Artist artist);
    }
}