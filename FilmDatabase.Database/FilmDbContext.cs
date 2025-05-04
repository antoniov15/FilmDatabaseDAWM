using FilmDatabase.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmDatabase.Database.Context
{
    public class FilmDbContext : DbContext
    {
        public FilmDbContext(DbContextOptions<FilmDbContext> options) : base(options)
        { }

        public DbSet<Film> Films { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<FilmActor> FilmActors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // relatia many-to-many dintre Film si Actor
            modelBuilder.Entity<FilmActor>()
                .HasKey(fa => new { fa.FilmId, fa.ActorId });

            modelBuilder.Entity<FilmActor>()
                .HasOne(fa => fa.Film)
                .WithMany(f => f.FilmActors)
                .HasForeignKey(fa => fa.FilmId);
            
            modelBuilder.Entity<FilmActor>()
                .HasOne(fa => fa.Actor)
                .WithMany(a => a.FilmActors)
                .HasForeignKey(fa => fa.ActorId);
        }

        public static void SeedData(FilmDbContext context)
        {
            if (!context.Films.Any())
            {
                // filme
                var film1 = new Film { Title = "Inception", Year = 2010, Genre = "Sci-Fi", Director = "Christopher Nolan", Description = "A thief who steals corporate secrets..." };
                var film2 = new Film { Title = "The Shawshank Redemption", Year = 1994, Genre = "Drama", Director = "Frank Darabont", Description = "Two imprisoned men bond over a number of years..." };

                context.Films.AddRange(film1, film2);
                context.SaveChanges();

                // actori
                var actor1 = new Actor { FirstName = "Leonardo", LastName = "DiCaprio", DateOfBirth = new DateTime(1974, 11, 11), Nationality = "American" };
                var actor2 = new Actor { FirstName = "Tom", LastName = "Hardy", DateOfBirth = new DateTime(1977, 9, 15), Nationality = "British" };
                var actor3 = new Actor { FirstName = "Morgan", LastName = "Freeman", DateOfBirth = new DateTime(1937, 6, 1), Nationality = "American" };
                var actor4 = new Actor { FirstName = "Tim", LastName = "Robbins", DateOfBirth = new DateTime(1958, 10, 16), Nationality = "American" };

                context.Actors.AddRange(actor1, actor2, actor3, actor4);
                context.SaveChanges();

                // relatii
                var filmActor1 = new FilmActor { FilmId = film1.Id, ActorId = actor1.Id, Role = "Dom Cobb" };
                var filmActor2 = new FilmActor { FilmId = film1.Id, ActorId = actor2.Id, Role = "Eames" };
                var filmActor3 = new FilmActor { FilmId = film2.Id, ActorId = actor3.Id, Role = "Ellis Boyd 'Red' Redding" };
                var filmActor4 = new FilmActor { FilmId = film2.Id, ActorId = actor4.Id, Role = "Andy Dufresne" };

                context.Set<FilmActor>().AddRange(filmActor1, filmActor2, filmActor3, filmActor4);
                context.SaveChanges();
            }
        }
    }
}
