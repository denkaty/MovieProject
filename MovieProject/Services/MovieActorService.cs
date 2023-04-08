using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using System.IO;

namespace MovieProject.Services
{
    public class MovieActorService : IMovieActorService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public MovieActorService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }

        public async Task CreateMovieActorAsync(MovieActorViewModel movieActorVM)
        {
            if (movieActorVM == null)
            {
                throw new ArgumentNullException("The MovieActorViewModel parameter cannot be null.", nameof(movieActorVM));
            }
            MovieActor movieActor = this.mapper.Map<MovieActor>(movieActorVM);
            await this.movieDbContext.AddAsync(movieActor);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<MovieActorViewModel>> GetAllMovieActorsAsync()
        {
            List<MovieActor> movieActors = await this.movieDbContext.MovieActors.ToListAsync();
            List<MovieActorViewModel> movieActorsViewModels = this.mapper.Map<List<MovieActorViewModel>>(movieActors);
            return movieActorsViewModels;
        }

        public async Task<MovieActorViewModel> GetMovieActorByIdAsync(string movieId, string actorId)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(actorId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            MovieActor? movieActor = await this.movieDbContext.MovieActors.FindAsync(movieId, actorId);
            if (movieActor == null)
            {
                throw new ArgumentException("No MovieActor was found with the given id.");
            }

            MovieActorViewModel movieActorViewModel = this.mapper.Map<MovieActorViewModel>(movieActor);
            return movieActorViewModel;
        }

        public async Task UpdateMovieActorAsync(MovieActorViewModel movieActorVM)
        {
            if (movieActorVM == null)
            {
                throw new ArgumentNullException("The MovieActorViewModel parameter cannot be null.", nameof(movieActorVM));
            }
            MovieActor movieActor = this.mapper.Map<MovieActor>(movieActorVM);
            this.movieDbContext.MovieActors.Update(movieActor);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<MovieActorViewModel> UpdateMovieActorByIdAsync(string movieId, string actorId, MovieActorViewModel movieActorVM)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(actorId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            if (movieActorVM == null)
            {
                throw new ArgumentException("The MovieActorViewModel parameter cannot be null.");
            }

            MovieActor? movieActorEntity = await this.movieDbContext.MovieActors.FindAsync((movieId, actorId));
            if(movieActorEntity == null)
            {
                throw new ArgumentException("No MovieActorEntity was found with the given id.");
            }
            MovieActor movieActorUpdated = this.mapper.Map(movieActorVM, movieActorEntity);

            this.movieDbContext.MovieActors.Update(movieActorUpdated);
            await this.movieDbContext.SaveChangesAsync();

            MovieActorViewModel movieActorViewModel = this.mapper.Map<MovieActorViewModel>(movieActorUpdated);
            return movieActorViewModel;
        }
        public async Task DeleteMovieActorByIdAsync(string movieId, string actorId)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(actorId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            MovieActor? movieActor = await this.movieDbContext.MovieActors.FindAsync((movieId, actorId));
            if (movieActor == null)
            {
                throw new ArgumentException($"There is no MovieActor with the id in the database.");
            }

            this.movieDbContext.MovieActors.Remove(movieActor);
            await this.movieDbContext.SaveChangesAsync();
        }
    }
}
