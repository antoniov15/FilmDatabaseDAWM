﻿using FilmDatabase.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmDatabase.Core.Interfaces
{
    public interface IFilmService
    {
        Task<IEnumerable<FilmDto>> GetAllFilmsWithActorsAsync();
        Task<PagedResult<FilmDto>> GetFilmsWithFilteringSortingPagingAsync(FilmQueryParameters queryParams);
        Task<FilmDto?> GetFilmWithActorsAsync(int id);
        Task<FilmDto> CreateFilmAsync(FilmDto filmDto);
        Task<FilmDto?> UpdateFilmAsync(FilmDto filmDto);
        Task<bool> DeleteFilmAsync(int id);
    }
}