﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Services.Artist;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.WebAPI.Controllers
{
    [Route("api/artist")]
    public class ArtistController : ODataController
    {
        private readonly IArtistService _artistService;
        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [EnableQuery]
        [AllowAnonymous]
        [HttpGet()] 
        public Task<IEnumerable<ArtistDto>> Get()
        {
            return _artistService.ListAsync();
        }

        [EnableQuery]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDetailsDto>> GetArtist(int id)
        {
            var item = await _artistService.DetailsAsync(id).ConfigureAwait(true);

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult> Post([FromBody]ArtistDetailsDto artist)
        {
            try
            {
                await _artistService.AddArtistAsync(artist).ConfigureAwait(true);
            }
            catch(ValidationException)
            {
                return ValidationProblem();
            }
            
            return Created(nameof(Get), null);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _artistService.DeleteArtistAsync(id).ConfigureAwait(true);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult> Put(int id, [FromBody] ArtistDetailsDto artist)
        {
            try
            {
                await _artistService.UpdateArtistAsync(id, artist).ConfigureAwait(true);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return ValidationProblem(new ValidationProblemDetails(){Detail = ex.Message});
            }

            return Created(nameof(GetArtist), id);
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Employee")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ArtistDetailsDto> patch)
        {
            var artist = await _artistService.DetailsAsync(id);
            if (artist == null)
                return NotFound();
            
            patch.ApplyTo(artist);
            try
            {
                await _artistService.UpdateArtistAsync(id, artist);
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails() {Detail = e.Message});
            }

            return Created(nameof(GetArtist), id);
        }
    }
} 
