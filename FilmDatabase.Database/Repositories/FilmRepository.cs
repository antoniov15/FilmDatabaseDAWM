using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmDatabase.Core.DTOs;
using FilmDatabase.Core.Interfaces;
using FilmDatabase.Core.Models;
using FilmDatabase.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace FilmDatabase.Database.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly FilmDbContext _context;

        public FilmRepository(FilmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Film>> GetAllFilmsWithActorsAsync()
        {
            return await _context.Films
                .Include(f => f.FilmActors)
                    .ThenInclude(fa => fa.Actor)
                .ToListAsync();
        }

        public async Task<PagedResult<Film>> GetFilmsWithFilteringSortingPagingAsync(FilmQueryParameters queryParams)
        {
            var query = _context.Films
                .Include(f => f.FilmActors)
                    .ThenInclude(fa => fa.Actor)
                .AsQueryable();

            // Aplicarea filtrelor
            if (!string.IsNullOrEmpty(queryParams.Genre))
            {
                query = query.Where(f => f.Genre.ToLower().Contains(queryParams.Genre.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParams.Director))
            {
                query = query.Where(f => f.Director.ToLower().Contains(queryParams.Director.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParams.Title))
            {
                query = query.Where(f => f.Title.ToLower().Contains(queryParams.Title.ToLower()));
            }

            if (queryParams.Year.HasValue)
            {
                query = query.Where(f => f.Year == queryParams.Year.Value);
            }

            if (queryParams.MinYear.HasValue)
            {
                query = query.Where(f => f.Year >= queryParams.MinYear.Value);
            }

            if (queryParams.MaxYear.HasValue)
            {
                query = query.Where(f => f.Year <= queryParams.MaxYear.Value);
            }

            // Aplicarea sortării
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                switch (queryParams.SortBy.ToLower())
                {
                    case "title":
                        query = queryParams.SortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(f => f.Title)
                            : query.OrderBy(f => f.Title);
                        break;
                    case "year":
                        query = queryParams.SortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(f => f.Year)
                            : query.OrderBy(f => f.Year);
                        break;
                    case "director":
                        query = queryParams.SortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(f => f.Director)
                            : query.OrderBy(f => f.Director);
                        break;
                    case "genre":
                        query = queryParams.SortOrder?.ToLower() == "desc"
                            ? query.OrderByDescending(f => f.Genre)
                            : query.OrderBy(f => f.Genre);
                        break;
                    default:
                        query = query.OrderBy(f => f.Title);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(f => f.Title);
            }

            // Calcularea totalului de înregistrări
            var totalRecords = await query.CountAsync();

            // Aplicarea paginării
            var films = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PagedResult<Film>(films, queryParams.PageNumber, queryParams.PageSize, totalRecords);
        }

        public async Task<Film?> GetFilmWithActorsAsync(int id)
        {
            return await _context.Films
                .Include(f => f.FilmActors)
                    .ThenInclude(fa => fa.Actor)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Film> AddFilmAsync(Film film)
        {
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            return film;
        }

        public async Task<Actor> AddActorAsync(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;
        }

        public async Task<Actor?> GetActorByNameAsync(string firstName, string lastName)
        {
            return await _context.Actors
                .FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName);
        }

        public async Task UpdateFilmAsync(Film film)
        {
            _context.Films.Update(film);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFilmAsync(Film film)
        {
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
        }
    }
}