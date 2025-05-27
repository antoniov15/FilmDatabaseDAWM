using System.ComponentModel.DataAnnotations;

namespace FilmDatabase.Core.DTOs
{
    public class FilmQueryParameters
    {
        // Paginare
        private int _pageSize = 10;
        private const int MaxPageSize = 100;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        // Filtrare
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public int? Year { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public string? Title { get; set; }

        // Sortare
        public string? SortBy { get; set; } = "Title"; // Title, Year, Director, Genre
        public string? SortOrder { get; set; } = "asc"; // asc, desc

        // Proprietăți helper pentru validarea sortării
        public bool IsValidSortBy()
        {
            var validSortFields = new string[] { "Title", "Year", "Director", "Genre" };
            return string.IsNullOrEmpty(SortBy) || Array.Exists(validSortFields, field =>
                string.Equals(field, SortBy, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsValidSortOrder()
        {
            var validSortOrders = new string[] { "asc", "desc" };
            return string.IsNullOrEmpty(SortOrder) || Array.Exists(validSortOrders, order =>
                string.Equals(order, SortOrder, StringComparison.OrdinalIgnoreCase));
        }
    }
}