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
