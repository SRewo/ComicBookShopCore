using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicBookShopCore.Services.Series
{
    public interface ISeriesService
    {
        Task<IEnumerable<SeriesDto>> SeriesListAsync();
        Task<SeriesDetailsDto> DetailsAsync(int id);
        Task<SeriesInputDto> EditSeriesAsync(int id);
        Task AddSeriesAsync(SeriesInputDto seriesDto);
        Task UpdateSeriesAsync(int id, SeriesInputDto seriesDto);
        Task DeleteSeriesAsync(int id);
    }
}