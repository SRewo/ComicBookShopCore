using System.Linq;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.Order
{
    public interface IOrderRepositoryAsync
    {
        Task<IQueryable<Data.Order>> GetAllAsync();
        Task<Data.Order> GetByIdAsync(int id);
        Task AddAsync(Data.Order order);
        Task RemoveAsync(Data.Order order);
    }
}