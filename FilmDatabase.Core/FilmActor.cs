using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDatabase.Core.Models
{
    /*
     Tabel de legatura dintre Film si Actor pentru relatia many-to-many
     */
    public class FilmActor
    {
        public int FilmId { get; set; }
        public Film Film { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }

        public string Role { get; set; }
    }
}
