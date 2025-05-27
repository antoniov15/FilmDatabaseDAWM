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

        public async Task<PagedResult<FilmDto>> GetFilmsWithFilteringSortingPagingAsync(FilmQueryParameters queryParams)
        {
            var pagedResult = await _filmRepository.GetFilmsWithFilteringSortingPagingAsync(queryParams);

            var filmDtos = pagedResult.Data.Select(MapToFilmDto);

            return new PagedResult<FilmDto>(filmDtos, pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalRecords);
        }

        public async Task<FilmDto?> GetFilmWithActorsAsync(int id)
        {
            var film = await _filmRepository.GetFilmWithActorsAsync(id);
            if (film == null) return null;

            return MapToFilmDto(film);
        }

        public async Task<FilmDto> CreateFilmAsync(FilmDto filmDto)
        {
            var film = new Film
            {
                Title = filmDto.Title,
                Year = filmDto.Year,
                Genre = filmDto.Genre,
                Director = filmDto.Director,
                Description = filmDto.Description,
                FilmActors = new List<FilmActor>()
            };

            if (filmDto.Actors != null && filmDto.Actors.Any())
            {
                foreach (var actorDtoItem in filmDto.Actors)
                {
                    var nameParts = actorDtoItem.FullName.Split(' ');
                    string firstName = nameParts[0];
                    string lastName = string.Join(" ", nameParts.Skip(1));

                    var existingActor = await _filmRepository.GetActorByNameAsync(firstName, lastName);

                    if (existingActor == null)
                    {
                        var newActor = new Actor
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            DateOfBirth = DateTime.Now,
                            Nationality = "Unknown"
                        };
                        newActor = await _filmRepository.AddActorAsync(newActor);

                        film.FilmActors.Add(new FilmActor
                        {
                            ActorId = newActor.Id,
                            Role = actorDtoItem.Role
                        });
                    }
                    else
                    {
                        film.FilmActors.Add(new FilmActor
                        {
                            ActorId = existingActor.Id,
                            Role = actorDtoItem.Role
                        });
                    }
                }
            }

            await _filmRepository.AddFilmAsync(film);

            var savedFilm = await _filmRepository.GetFilmWithActorsAsync(film.Id);
            return MapToFilmDto(savedFilm);
        }

        public async Task<FilmDto?> UpdateFilmAsync(FilmDto filmDto)
        {
            var existingFilm = await _filmRepository.GetFilmWithActorsAsync(filmDto.Id);
            if (existingFilm == null)
            {
                throw new KeyNotFoundException($"Film with ID {filmDto.Id} not found.");
            }

            existingFilm.Title = filmDto.Title;
            existingFilm.Year = filmDto.Year;
            existingFilm.Genre = filmDto.Genre;
            existingFilm.Director = filmDto.Director;
            existingFilm.Description = filmDto.Description;

            await _filmRepository.UpdateFilmAsync(existingFilm);

            var updatedFilm = await _filmRepository.GetFilmWithActorsAsync(filmDto.Id);
            return MapToFilmDto(updatedFilm);
        }

        public async Task<bool> DeleteFilmAsync(int id)
        {
            var existingFilm = await _filmRepository.GetFilmWithActorsAsync(id);
            if (existingFilm == null) return false;

            await _filmRepository.DeleteFilmAsync(existingFilm);
            return true;
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
                Actors = film.FilmActors?.Select(fa => new ActorDto
                {
                    Id = fa.Actor.Id,
                    FullName = $"{fa.Actor.FirstName} {fa.Actor.LastName}",
                    Role = fa.Role
                }).ToList() ?? new List<ActorDto>()
            };
        }
    }
}