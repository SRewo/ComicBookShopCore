using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.Publisher
{
    public interface IAsyncPublisherRepository
    {
        Task<IQueryable<Data.Publisher>> GetAsync();
        Task<Data.Publisher> GetByIdAsync(int id);
        Task AddAsync(Data.Publisher publisher);
        Task UpdateAsync(Data.Publisher publisher);
        Task DeleteAsync(Data.Publisher publisher);
    }
}