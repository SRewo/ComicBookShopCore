using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ComicBookShopCore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase 
    {
        private IAsyncRepository<Publisher> _publisherRepository;
        public PublisherController(IAsyncRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [HttpGet]
        public Task<IEnumerable<Publisher>> Get()
        {
	    return _publisherRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetById(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id).ConfigureAwait(true);
            if (publisher == null)
                return NotFound();

            return publisher;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Publisher publisher)
        {
	    publisher.Validate();
            
            if (publisher.HasErrors)
                return ValidationProblem();

            await _publisherRepository.AddAsync(publisher).ConfigureAwait(true);
            return Created(nameof(Get), null);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id).ConfigureAwait(true);

            if (publisher == null)
                return NotFound();

            await _publisherRepository.DeleteAsync(publisher).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Publisher publisher)
        {
	    publisher.Validate();
            if (publisher.HasErrors)
                return ValidationProblem();

            var dbPublisher = await _publisherRepository.GetByIdAsync(id).ConfigureAwait(true);
            if (dbPublisher == null)
                return NotFound();

            dbPublisher.Name = publisher.Name;
            dbPublisher.CreationDateTime = publisher.CreationDateTime;
            dbPublisher.Description = publisher.Description;
                 
            await _publisherRepository.UpdateAsync(dbPublisher).ConfigureAwait(true);

            return Created(nameof(GetById), id);
        }
    }
}