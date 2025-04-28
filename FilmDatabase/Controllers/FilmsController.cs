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
    }
}
