using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.ComicBook
{
    public interface IComicBookService
    {
        Task<IEnumerable<ComicBookListDto>> ComicListAsync();
        Task<ComicBookDetailsDto> ComicDetailsAsync(int id);
        Task<ComicBookInputDto> ComicToEditAsync(int id);
        Task AddComicAsync(ComicBookInputDto comic);
        Task UpdateComicAsync(int id, ComicBookInputDto comic);
        Task DeleteComicAsync(int id);
    }
}