using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;

namespace ComicBookShopCore.Services.Publisher
{
    public class PublisherService : IPublisherService 
    {
        private readonly IAsyncPublisherRepository _repository;
        private readonly IMapper _mapper;

        public PublisherService(IAsyncPublisherRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<PublisherBasicDto>> PublisherListAsync()
        {
            var list = await _repository.GetAsync().ConfigureAwait(true);
            return _mapper.ProjectTo<PublisherBasicDto>(list);
        }

        public async Task<PublisherDetailsDto> PublisherDetailsAsync(int id)
        {
            var publisher = await _repository.GetByIdAsync(id).ConfigureAwait(true);

            return _mapper.Map<PublisherDetailsDto>(publisher);
        }

        public Task AddPublisherAsync(PublisherDto publisher)
        {
            var pub = _mapper.Map<Data.Publisher>(publisher);
            pub.Validate();

            if (pub.HasErrors)
                throw new ValidationException(pub.GetFirstError());

            return _repository.AddAsync(pub);
        }

        public async Task UpdatePublisherAsync(int id, PublisherDto publisher)
        {
            var pub = await _repository.GetByIdAsync(id).ConfigureAwait(true);

	    if(pub == null)
	        throw new NullReferenceException();

            _mapper.Map(publisher, pub);
            pub.Validate();

	    if(pub.HasErrors)
                throw new ValidationException(pub.GetFirstError());

            await _repository.UpdateAsync(pub).ConfigureAwait(true);
        }

        public async Task DeletePublisherAsync(int id)
        {
            var pub = await _repository.GetByIdAsync(id).ConfigureAwait(true);

            if (pub == null)
                throw new NullReferenceException();

            await _repository.DeleteAsync(pub).ConfigureAwait(true);
        }
    }
}