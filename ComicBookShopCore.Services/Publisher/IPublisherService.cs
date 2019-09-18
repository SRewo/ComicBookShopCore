using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.Publisher
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherBasicDto>> PublisherListAsync();
        Task<PublisherDetailsDto> PublisherDetailsAsync(int id);
        Task AddPublisherAsync(PublisherDto publisher);
        Task UpdatePublisherAsync(int id, PublisherDto publisher);
        Task DeletePublisherAsync(int id);
        Task<PublisherDto> PublisherToEditAsync(int id);
    }
}