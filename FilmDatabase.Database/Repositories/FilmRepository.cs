using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
