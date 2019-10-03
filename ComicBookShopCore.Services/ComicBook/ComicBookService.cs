using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;
using ComicBookShopCore.Services.Artist;
using ComicBookShopCore.Services.Series;

namespace ComicBookShopCore.Services.ComicBook
{
    public class ComicBookService : IComicBookService
    {
        private readonly IComicBookRepositoryAsync _repository;
        private readonly IMapper _mapper;
        private readonly IAsyncSeriesRepository _seriesRepository;
        private readonly IAsyncArtistRepository _artistRepository;

        public ComicBookService(IComicBookRepositoryAsync repository, IMapper mapper, IAsyncSeriesRepository seriesRepository, IAsyncArtistRepository artistRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _seriesRepository = seriesRepository;
            _artistRepository = artistRepository;
        }

        public async Task<IEnumerable<ComicBookListDto>> ComicListAsync()
        {
            var list = await _repository.GetListAsync();
            return _mapper.ProjectTo<ComicBookListDto>(list).AsEnumerable();
        }

        public async Task<ComicBookDetailsDto> ComicDetailsAsync(int id)
        {
            var comic = await _repository.GetByIdAsync(id);
            return _mapper.Map<ComicBookDetailsDto>(comic);
        }

        public async Task<ComicBookInputDto> ComicToEditAsync(int id)
        {
            var comic = await _repository.GetByIdAsync(id);
            return _mapper.Map<ComicBookInputDto>(comic);
        }

        public async Task AddComicAsync(ComicBookInputDto comic)
        {

            var com = _mapper.Map<Data.ComicBook>(comic);

            com.Series = await _seriesRepository.GetByIdAsync(comic.SeriesId);
            if (com.Series == null)
                throw new ValidationException("Invalid Series Id");

            if (!com.ComicBookArtists.Any())
            {
                throw new ValidationException("Artist list cannot be empty");
            }
            
            com.Validate();

            if (com.HasErrors)
                throw new ValidationException(com.GetFirstError());

            await _repository.AddAsync(com);
        }

        public async Task UpdateComicAsync(int id, ComicBookInputDto comic)
        {
            var com = await _repository.GetByIdAsync(id);
            if (com == null)
                throw new NullReferenceException();

            _mapper.Map(comic, com);
            if (com.Series.Id != comic.SeriesId)
            {
                var series = await _seriesRepository.GetByIdAsync(comic.SeriesId);
                if (series ==null)
                    throw new ValidationException("Invalid series id.");
                com.Series = series;
            }

            com.Validate();
            if (com.HasErrors)
                throw new ValidationException(com.GetFirstError());

            await _repository.UpdateAsync(com);
        }

        public async Task DeleteComicAsync(int id)
        {
            var com = await _repository.GetByIdAsync(id);
            if (com == null)
                throw new NullReferenceException();

            await _repository.DeleteAsync(com);
        }
    }
}