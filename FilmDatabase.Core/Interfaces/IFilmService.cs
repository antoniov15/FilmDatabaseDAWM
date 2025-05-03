using FilmDatabase.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmDatabase.Core.Interfaces
{
    public interface IFilmService
    {
        Task<IEnumerable<FilmDto>> GetAllFilmsWithActorsAsync();
        Task<FilmDto?> GetFilmWithActorsAsync(int id);
        Task<FilmDto> CreateFilmAsync(FilmDto filmDto);
    }
}