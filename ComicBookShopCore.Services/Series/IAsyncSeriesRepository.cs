using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.Series
{
    public interface IAsyncSeriesRepository
    {
        Task<IQueryable<Data.Series>> GetAsync();
        Task<Data.Series> GetByIdAsync(int id);
        Task AddAsync(Data.Series series);
        Task UpdateAsync(Data.Series series);
        Task DeleteAsync(Data.Series series);
    }
}