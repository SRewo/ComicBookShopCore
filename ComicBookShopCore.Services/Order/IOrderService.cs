using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.Order
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderBasicDto>> OrderListAsync();
        Task<OrderDetailsDto> OrderDetailsAsync(int id);
        Task<Dictionary<string, string>> AddOrderAsync(OrderInputDto order);
        Task RemoveOrderAsync(int id);
    }
}