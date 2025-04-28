using FilmDatabase.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmDatabase.Core.Interfaces
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAllFilmsWithActorsAsync();
        Task<Film?> GetFilmWithActorsAsync(int id);
    }
}