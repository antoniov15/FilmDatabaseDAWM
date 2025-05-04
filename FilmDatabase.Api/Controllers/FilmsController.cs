using FilmDatabase.Core.DTOs;
using FilmDatabase.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmDatabase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmsController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetAllFilmsWithActors()
        {
            var films = await _filmService.GetAllFilmsWithActorsAsync();
            return Ok(films);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDto>> GetFilmWithActors(int id)
        {
            var film = await _filmService.GetFilmWithActorsAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return Ok(film);
        }

        [HttpPost]
        public async Task<ActionResult<FilmDto>> CreateFilm(FilmDto filmDto)
        {
            if (filmDto == null)
            {
                return BadRequest();
            }

            var createdFilm = await _filmService.CreateFilmAsync(filmDto);

            return CreatedAtAction(
                nameof(GetFilmWithActors),
                new { id = createdFilm.Id },
                createdFilm);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FilmDto>> UpdateFilm(int id, FilmDto filmDto)
        {
            if (id != filmDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var updatedFilm = await _filmService.UpdateFilmAsync(filmDto);
            if (updatedFilm == null)
            {
                return NotFound();
            }
            return Ok(updatedFilm);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFilm(int id)
        {
            var result = await _filmService.DeleteFilmAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
