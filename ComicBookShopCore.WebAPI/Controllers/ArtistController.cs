using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ComicBookShopCore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly IAsyncRepository<Artist> _artistRepository;

        public ArtistController(ILogger<ArtistController> logger, IAsyncRepository<Artist> artistRepository)
        {
            _logger = logger;
            _artistRepository = artistRepository;
        }

        [HttpGet]
        public Task<IEnumerable<Artist>> Get()
        {
            return _artistRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            var item = await _artistRepository.GetByIdAsync(id).ConfigureAwait(true);

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Artist artist)
        {
            artist.Validate();
            if (artist.HasErrors)
                return ValidationProblem();

            await _artistRepository.AddAsync(artist).ConfigureAwait(true);
            return Created(nameof(Get), null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _artistRepository.GetByIdAsync(id).ConfigureAwait(true);

            if (item == null)
            {
                return NotFound();
            }
            await _artistRepository.DeleteAsync(item).ConfigureAwait(true);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Artist artist)
        {
            var item = await _artistRepository.GetByIdAsync(id).ConfigureAwait(true);
            if (item == null) return NotFound();
            item.FirstName = artist.FirstName;
            item.LastName = artist.LastName;
            item.Description = artist.Description;
	    item.Validate();
            if (item.HasErrors)
            {
                return ValidationProblem();
            }
            await _artistRepository.UpdateAsync(item).ConfigureAwait(true);

            return Created(nameof(GetArtist), id);
        }
    }
} 
