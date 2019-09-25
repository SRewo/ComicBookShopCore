using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Services.ComicBook;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ComicBookShopCore.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ComicBookController : ControllerBase
    {
        private readonly IComicBookService _service;

        public ComicBookController(IComicBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicBookListDto>>> Get()
        {
            var list = await _service.ComicListAsync();
            if (!list.Any())
                return NotFound();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComicBookDetailsDto>> Get(int id)
        {
            var comic = await _service.ComicDetailsAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            return Ok(comic);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ComicBookInputDto comicDto)
        {
            try
            {
                await _service.AddComicAsync(comicDto);
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails() {Detail = e.Message});
            }

            return Created(nameof(Get), null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromBody] ComicBookInputDto comicDto)
        {
            try
            {
                await _service.UpdateComicAsync(id, comicDto);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails() {Detail = e.Message});
            }

            return Created(nameof(Get), id);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ComicBookInputDto> patch)
        {
            var comic = await _service.ComicToEditAsync(id);

            if (comic == null)
                return NotFound();

            patch.ApplyTo(comic);

            try
            {
                await _service.UpdateComicAsync(id, comic);
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails() {Detail = e.Message});
            }

            return Created(nameof(Get), id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteComicAsync(id);
            }
            catch (NullReferenceException )
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}