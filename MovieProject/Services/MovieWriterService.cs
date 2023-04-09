using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.Services
{
    public class MovieWriterService : IMovieWriterService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public MovieWriterService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }
        public async Task CreateMovieWriterAsync(MovieWriterViewModel movieWriterVM)
        {
            if (movieWriterVM == null)
            {
                throw new ArgumentNullException("The MovieWriterViewModel parameter cannot be null.", nameof(movieWriterVM));
            }
            MovieWriter movieWriter = this.mapper.Map<MovieWriter>(movieWriterVM);
            await this.movieDbContext.AddAsync(movieWriter);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<MovieWriterViewModel>> GetAllMovieWritersAsync()
        {
            List<MovieWriter> movieWriters = await this.movieDbContext.MovieWriters.ToListAsync();
            List<MovieWriterViewModel> movieWritersViewModels = this.mapper.Map<List<MovieWriterViewModel>>(movieWriters);
            return movieWritersViewModels;
        }

        public async Task<MovieWriterViewModel> GetMovieWriterByIdAsync(string movieId, string writerId)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(writerId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            MovieWriter? movieWriter = await this.movieDbContext.MovieWriters.FindAsync(movieId, writerId);
            if (movieWriter == null)
            {
                throw new ArgumentException("No MovieWriter was found with the given id.");
            }

            MovieWriterViewModel movieWriterViewModel = this.mapper.Map<MovieWriterViewModel>(movieWriter);
            return movieWriterViewModel;
        }

        public async Task UpdateMovieWriterAsync(MovieWriterViewModel movieWriterVM)
        {
            if (movieWriterVM == null)
            {
                throw new ArgumentNullException("The MovieWriterViewModel parameter cannot be null.", nameof(movieWriterVM));
            }
            MovieWriter movieWriter = this.mapper.Map<MovieWriter>(movieWriterVM);
            this.movieDbContext.MovieWriters.Update(movieWriter);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<MovieWriterViewModel> UpdateMovieWriterByIdAsync(string movieId, string writerId, MovieWriterViewModel movieWriterVM)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(writerId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            if (movieWriterVM == null)
            {
                throw new ArgumentException("The MovieWriterViewModel parameter cannot be null.");
            }

            MovieWriter? movieWriterEntity = await this.movieDbContext.MovieWriters.FindAsync((movieId, writerId));
            if (movieWriterEntity == null)
            {
                throw new ArgumentException("No MovieWriterEntity was found with the given id.");
            }
            MovieWriter movieWriterUpdated = this.mapper.Map(movieWriterVM, movieWriterEntity);

            this.movieDbContext.MovieWriters.Update(movieWriterUpdated);
            await this.movieDbContext.SaveChangesAsync();

            MovieWriterViewModel movieWriterViewModel = this.mapper.Map<MovieWriterViewModel>(movieWriterUpdated);
            return movieWriterViewModel;
        }
        public async Task DeleteMovieWriterByIdAsync(string movieId, string writerId)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(writerId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            MovieWriter? movieWriter = await this.movieDbContext.MovieWriters.FindAsync((movieId, writerId));
            if (movieWriter == null)
            {
                throw new ArgumentException($"There is no MovieWriter with the id in the database.");
            }

            this.movieDbContext.MovieWriters.Remove(movieWriter);
            await this.movieDbContext.SaveChangesAsync();
        }
    }
}
