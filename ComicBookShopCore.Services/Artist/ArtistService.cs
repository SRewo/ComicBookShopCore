using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;

namespace ComicBookShopCore.Services.Artist
{
    public class ArtistService : IArtistService
    {
        private readonly IAsyncArtistRepository _repository;
        private readonly IMapper _mapper;

        public ArtistService(IAsyncArtistRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtistDto>> ListAsync()
        {
            var list = await _repository.GetAllAsync().ConfigureAwait(true);
            return _mapper.ProjectTo<ArtistDto>(list);
        }

        public async Task<ArtistDetailsDto> DetailsAsync(int id)
        {
            var artist = await _repository.GetByIdAsync(id).ConfigureAwait(true);
            return _mapper.Map<ArtistDetailsDto>(artist);
        }

        public Task AddArtistAsync(ArtistDetailsDto artist)
        {
            var addArtist = _mapper.Map<Data.Artist>(artist);
	    addArtist.Validate();

            if (addArtist.HasErrors)
                throw new ValidationException(addArtist.GetFirstError());

            return _repository.AddAsync(addArtist);
        }

        public async Task UpdateArtistAsync(int id,ArtistDetailsDto artist)
        {
            var dbArtist = await _repository.GetByIdAsync(id).ConfigureAwait(true);
            if (dbArtist == null)
                throw new NullReferenceException();

            _mapper.Map(artist, dbArtist);
	    dbArtist.Validate();

	    if(dbArtist.HasErrors)
	        throw new ValidationException(dbArtist.GetFirstError());

            await _repository.UpdateAsync(dbArtist).ConfigureAwait(true);
        }

        public async Task DeleteArtistAsync(int id)
        {
            var dbArtist = await _repository.GetByIdAsync(id).ConfigureAwait(true);

            if (dbArtist == null)
                throw new NullReferenceException();

            await _repository.DeleteAsync(dbArtist).ConfigureAwait(true);
        }
    }
}