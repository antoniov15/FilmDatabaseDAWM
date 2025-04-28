using FilmDatabase.Core.Interfaces;
using FilmDatabase.Core.DTOs;
using FilmDatabase.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDatabase.Core.Services
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;

        public FilmService(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public async Task<IEnumerable<FilmDto>> GetAllFilmsWithActorsAsync()
        {
            var films = await _filmRepository.GetAllFilmsWithActorsAsync();
            return films.Select(MapToFilmDto);
        }

        public async Task<FilmDto> GetFilmWithActorsAsync(int id)
        {
            var film = await _filmRepository.GetFilmWithActorsAsync(id);
            if (film == null) return null;

            return MapToFilmDto(film);
        }
        // mapare
        private FilmDto MapToFilmDto(Film film)
        {
            return new FilmDto
            {
                Id = film.Id,
                Title = film.Title,
                Year = film.Year,
                Genre = film.Genre,
                Director = film.Director,
                Description = film.Description,
                Actors = film.FilmActors.Select(fa => new ActorDto
                {
                    Id = fa.Actor.Id,
                    FullName = $"{fa.Actor.FirstName} {fa.Actor.LastName}",
                    Role = fa.Role
                }).ToList()
            };
        }
    }
}
