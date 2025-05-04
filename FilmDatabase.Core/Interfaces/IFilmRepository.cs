using FilmDatabase.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmDatabase.Core.Interfaces
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAllFilmsWithActorsAsync();
        Task<Film?> GetFilmWithActorsAsync(int id);
        Task<Film> AddFilmAsync(Film film);
        Task<Actor> AddActorAsync(Actor actor);
        Task<Actor?> GetActorByNameAsync(string firstName, string lastName);
        Task UpdateFilmAsync(Film film);
        Task DeleteFilmAsync(Film film);
    }
}