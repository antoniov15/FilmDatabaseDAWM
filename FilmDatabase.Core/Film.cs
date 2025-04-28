using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDatabase.Core.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }

        // many-to-many relationship with Actor
        public ICollection<FilmActor> FilmActors { get; set; }
    }
}
