using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using ComicBookShopCore.Services.Publisher;

namespace ComicBookShopCore.Services.Series
{
    public class SeriesService : ISeriesService
    {
        private readonly IAsyncSeriesRepository _repository;
        private readonly IAsyncPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;

        public SeriesService(IAsyncSeriesRepository repository, IMapper mapper, IAsyncPublisherRepository publisherRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _publisherRepository = publisherRepository;
        }

        public async Task<IEnumerable<SeriesDto>> SeriesListAsync()
        {
            var list = await _repository.GetAsync().ConfigureAwait(true);
            return _mapper.ProjectTo<SeriesDto>(list);
        }

        public async Task<SeriesDetailsDto> DetailsAsync(int id)
        {
            var series = await _repository.GetByIdAsync(id).ConfigureAwait(true);

            return _mapper.Map<SeriesDetailsDto>(series);
        }

        public async Task AddSeriesAsync(SeriesInputDto seriesDto)
        {
            var series = _mapper.Map<Data.Series>(seriesDto);
            var publisher = await _publisherRepository.GetByIdAsync(seriesDto.PublisherId).ConfigureAwait(true);

	    if(publisher == null)
	        throw new ValidationException("Invalid publisher id.");

            series.Publisher = publisher;
            
	    series.Validate();

            if (series.HasErrors)
                throw new ValidationException(series.GetFirstError());

            await _repository.AddAsync(series).ConfigureAwait(true);
        }

        public async Task UpdateSeriesAsync(int id, SeriesInputDto seriesDto)
        {
            var dbSeries = await _repository.GetByIdAsync(id).ConfigureAwait(true);

            if (dbSeries == null)
                throw new NullReferenceException();

            if (dbSeries.Publisher.Id != seriesDto.PublisherId && seriesDto.PublisherId > 0)
            {
                var publisher = await _publisherRepository.GetByIdAsync(seriesDto.PublisherId).ConfigureAwait(true);

		if(publisher == null)
                    throw new ValidationException("Invalid publisher id.");

                dbSeries.Publisher = publisher;
            }

            _mapper.Map(seriesDto, dbSeries);
            dbSeries.Validate();

            if (dbSeries.HasErrors)
                throw new ValidationException(dbSeries.GetFirstError());
            await _repository.UpdateAsync(dbSeries).ConfigureAwait(true);
        }

        public async Task DeleteSeriesAsync(int id)
        {
            var dbSeries = await _repository.GetByIdAsync(id).ConfigureAwait(true);

            if (dbSeries== null)
                throw new NullReferenceException();

            await _repository.DeleteAsync(dbSeries).ConfigureAwait(true);
        }
    }
}