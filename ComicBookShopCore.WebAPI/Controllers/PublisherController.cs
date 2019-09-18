using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Services.Publisher;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ComicBookShopCore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public Task<IEnumerable<PublisherBasicDto>> Get()
        {
            return _publisherService.PublisherListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDetailsDto>> GetById(int id)
        {
            var publisher = await _publisherService.PublisherDetailsAsync(id).ConfigureAwait(true);

            if (publisher == null)
                return NotFound();

            return publisher;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PublisherDto publisher)
        {
            try
            {
                await _publisherService.AddPublisherAsync(publisher).ConfigureAwait(true);
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails {Detail = e.Message});
            }

            return Created(nameof(Get), null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _publisherService.DeletePublisherAsync(id).ConfigureAwait(true);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PublisherDto publisher)
        {
            try
            {
                await _publisherService.UpdatePublisherAsync(id, publisher).ConfigureAwait(true);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails {Detail = e.Message});
            }

            return Created(nameof(GetById), id);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PublisherDto> patch)
        {
            var publisher = await _publisherService.PublisherToEditAsync(id);
            if (publisher == null)
                return NotFound();

            patch.ApplyTo(publisher);

            try
            {
                await _publisherService.UpdatePublisherAsync(id, publisher);
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails() {Detail = e.Message});
            }

            return Created(nameof(GetById), id);
        }
    }
}