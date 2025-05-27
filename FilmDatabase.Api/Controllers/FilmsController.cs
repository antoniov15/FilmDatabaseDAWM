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

        /// <summary>
        /// Obține toate filmele cu filtrare, sortare și paginare
        /// </summary>
        /// <param name="queryParams">Parametrii pentru filtrare, sortare și paginare</param>
        /// <returns>Listă paginată de filme</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResult<FilmDto>>> GetFilms([FromQuery] FilmQueryParameters queryParams)
        {
            // Validare parametrii de sortare
            if (!queryParams.IsValidSortBy())
            {
                return BadRequest("Invalid sort field. Valid options: Title, Year, Director, Genre");
            }

            if (!queryParams.IsValidSortOrder())
            {
                return BadRequest("Invalid sort order. Valid options: asc, desc");
            }

            var result = await _filmService.GetFilmsWithFilteringSortingPagingAsync(queryParams);
            return Ok(result);
        }

        /// <summary>
        /// Obține toate filmele (fără paginare) - pentru compatibilitate
        /// </summary>
        /// <returns>Lista tuturor filmelor</returns>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetAllFilmsWithActors()
        {
            var films = await _filmService.GetAllFilmsWithActorsAsync();
            return Ok(films);
        }

        /// <summary>
        /// Obține un film după ID
        /// </summary>
        /// <param name="id">ID-ul filmului</param>
        /// <returns>Filmul cu ID-ul specificat</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDto>> GetFilmWithActors(int id)
        {
            var film = await _filmService.GetFilmWithActorsAsync(id);
            if (film == null)
            {
                return NotFound($"Film with ID {id} not found.");
            }
            return Ok(film);
        }

        /// <summary>
        /// Creează un film nou
        /// </summary>
        /// <param name="filmDto">Datele filmului</param>
        /// <returns>Filmul creat</returns>
        [HttpPost]
        public async Task<ActionResult<FilmDto>> CreateFilm(FilmDto filmDto)
        {
            if (filmDto == null)
            {
                return BadRequest("Film data is required.");
            }

            if (string.IsNullOrWhiteSpace(filmDto.Title))
            {
                return BadRequest("Film title is required.");
            }

            var createdFilm = await _filmService.CreateFilmAsync(filmDto);

            return CreatedAtAction(
                nameof(GetFilmWithActors),
                new { id = createdFilm.Id },
                createdFilm);
        }

        /// <summary>
        /// Actualizează un film existent (endpoint PUT pentru Tema 2)
        /// </summary>
        /// <param name="id">ID-ul filmului de actualizat</param>
        /// <param name="filmDto">Noile date ale filmului</param>
        /// <returns>Filmul actualizat</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<FilmDto>> UpdateFilm(int id, FilmDto filmDto)
        {
            if (id != filmDto.Id)
            {
                return BadRequest("ID mismatch between route and body.");
            }

            if (string.IsNullOrWhiteSpace(filmDto.Title))
            {
                return BadRequest("Film title is required.");
            }

            // Excepția KeyNotFoundException va fi prinsă de middleware
            var updatedFilm = await _filmService.UpdateFilmAsync(filmDto);
            return Ok(updatedFilm);
        }

        /// <summary>
        /// Șterge un film
        /// </summary>
        /// <param name="id">ID-ul filmului de șters</param>
        /// <returns>Status de confirmare</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFilm(int id)
        {
            var result = await _filmService.DeleteFilmAsync(id);
            if (!result)
            {
                return NotFound($"Film with ID {id} not found.");
            }
            return NoContent();
        }
    }
}