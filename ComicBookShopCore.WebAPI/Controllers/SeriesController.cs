﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Services.Series;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicBookShopCore.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SeriesController : ControllerBase
    {
        private readonly ShopDbEntities _context;
        private readonly ISeriesService _service;

        public SeriesController( ShopDbEntities context, ISeriesService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeriesDto>>> Get()
        {
            var seriesList = await _service.SeriesListAsync().ConfigureAwait(true);

            return Ok(seriesList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeriesDetailsDto>> GetById(int id)
        {
            var series = await _service.DetailsAsync(id).ConfigureAwait(true);

            if (series == null)
                return NotFound();

            return Ok(series);
        }

        [HttpPost]
        public async Task<ActionResult> Post(SeriesInputDto seriesDto)
        {
            try
            {
                await _service.AddSeriesAsync(seriesDto).ConfigureAwait(true);
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails() {Detail = e.Message});
            }

            return Created(nameof(Get), null);
        }

	[HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, SeriesInputDto seriesDto)
        {
            try
            {
                await _service.UpdateSeriesAsync(id, seriesDto).ConfigureAwait(true);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (ValidationException e)
            {
                return ValidationProblem(new ValidationProblemDetails() {Detail = e.Message});
            }

            return Created(nameof(GetById), id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteSeriesAsync(id).ConfigureAwait(true);
            }
            catch (NullReferenceException e)
            {
                return NotFound();
            }

            return NoContent();
        }
 }
}