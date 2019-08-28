using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;

namespace ComicBookShopCore.Services.Artist
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistDto>> ListAsync();
        Task<ArtistDetailsDto> DetailsAsync(int id);
        Task AddArtistAsync(ArtistDetailsDto artist);
        Task UpdateArtistAsync(int id,ArtistDetailsDto artist);
        Task DeleteArtistAsync(int id);
    }
}