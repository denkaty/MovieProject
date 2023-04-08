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
        public virtual DbSet<MovieWriter> MovieWriters { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Writer> Writers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.CONNECTION_STRING);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            builder.Entity<MovieWriter>()
                .HasKey(mw => new { mw.MovieId, mw.WriterId });

            builder.Entity<Movie>()
                .Property(movie => movie.MovieId)
                .ValueGeneratedOnAdd();

            builder.Entity<Actor>()
                .Property(actor => actor.ActorId)
                .ValueGeneratedOnAdd();
        }
    }
}