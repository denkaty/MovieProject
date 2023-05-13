using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data.Entities;

namespace MovieProject.Data
{
    public class MovieDbContext : IdentityDbContext
    {
        public MovieDbContext()
        {
        }

        public MovieDbContext(DbContextOptions<MovieDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Director> Directors { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieActor> MovieActors { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.CONNECTION_STRING);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            builder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            builder.Entity<Movie>()
                .Property(m => m.BoxOffice)
                .HasColumnType("decimal(18,2)");

        }
    }
}