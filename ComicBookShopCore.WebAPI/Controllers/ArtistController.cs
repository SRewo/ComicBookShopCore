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
    public class ArtistInputModel{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly IRepository<Artist> _artistRepository;

        public ArtistController(ILogger<ArtistController> logger, IRepository<Artist> artistRepository)
        {
            _logger = logger;
            _artistRepository = artistRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> Get()
        {
            return await _artistRepository.GetAll().ToListAsync().ConfigureAwait(true);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            var item = await _artistRepository.GetAll().SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(true);

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPut]
        public ActionResult Put([FromBody]Artist artist)
        {
            artist.Validate();
            if (artist.HasErrors)
                return ValidationProblem();

            _artistRepository.Add(artist);
            return Created(nameof(Get), null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _artistRepository.GetAll().SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(true);

            if (item == null)
            {
                return NotFound();
            }
            _artistRepository.Delete(item);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] Artist artist)
        {
            var item = await _artistRepository.GetAll().SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(true);
            if (item == null) return NotFound();
            item.FirstName = artist.FirstName;
            item.LastName = artist.LastName;
            item.Description = artist.Description;
	    item.Validate();
            if (item.HasErrors)
            {
                return ValidationProblem();
            }
            _artistRepository.Update(item);

            return Created(nameof(GetArtist), id);
        }
    }
} 
