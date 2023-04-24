using AutoMapper;
using MovieProject.Data.Entities;
using MovieProject.Data;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MovieProject.Services
{
    public class MovieGenreService : IMovieGenreService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public MovieGenreService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }

        public async Task CreateMovieGenreAsync(MovieGenreViewModel movieGenreVM)
        {
            if (movieGenreVM == null)
            {
                throw new ArgumentNullException("The MovieGenreViewModel parameter cannot be null.", nameof(movieGenreVM));
            }
            MovieGenre movieGenre = this.mapper.Map<MovieGenre>(movieGenreVM);
            await this.movieDbContext.AddAsync(movieGenre);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<MovieGenreViewModel>> GetAllMovieGenresAsync()
        {
            List<MovieGenre> movieGenres = await this.movieDbContext.MovieGenres.ToListAsync();
            List<MovieGenreViewModel> movieGenresViewModels = this.mapper.Map<List<MovieGenreViewModel>>(movieGenres);
            return movieGenresViewModels;
        }

        public async Task<MovieGenreViewModel> GetMovieGenreByIdAsync(string movieId, string genreId)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(genreId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            MovieGenre? movieGenre = await this.movieDbContext.MovieGenres.FindAsync(movieId, genreId);
            if (movieGenre == null)
            {
                throw new ArgumentException("No MovieGenre was found with the given id.");
            }

            MovieGenreViewModel movieGenreViewModel = this.mapper.Map<MovieGenreViewModel>(movieGenre);
            return movieGenreViewModel;
        }

        public async Task UpdateMovieGenreAsync(MovieGenreViewModel movieGenreVM)
        {
            if (movieGenreVM == null)
            {
                throw new ArgumentNullException("The MovieGenreViewModel parameter cannot be null.", nameof(movieGenreVM));
            }
            MovieGenre movieGenre = this.mapper.Map<MovieGenre>(movieGenreVM);
            this.movieDbContext.MovieGenres.Update(movieGenre);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<MovieGenreViewModel> UpdateMovieGenreByIdAsync(string movieId, string genreId, MovieGenreViewModel movieGenreVM)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(genreId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            if (movieGenreVM == null)
            {
                throw new ArgumentException("The MovieGenreViewModel parameter cannot be null.");
            }

            MovieGenre? movieGenreEntity = await this.movieDbContext.MovieGenres.FindAsync((movieId, genreId));
            if (movieGenreEntity == null)
            {
                throw new ArgumentException("No MovieGenreEntity was found with the given id.");
            }
            MovieGenre movieGenreUpdated = this.mapper.Map(movieGenreVM, movieGenreEntity);

            this.movieDbContext.MovieGenres.Update(movieGenreUpdated);
            await this.movieDbContext.SaveChangesAsync();

            MovieGenreViewModel movieGenreViewModel = this.mapper.Map<MovieGenreViewModel>(movieGenreUpdated);
            return movieGenreViewModel;
        }
        public async Task DeleteMovieGenreByIdAsync(string movieId, string genreId)
        {
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(genreId))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.");
            }

            MovieGenre? movieGenre = await this.movieDbContext.MovieGenres.FindAsync((movieId, genreId));
            if (movieGenre == null)
            {
                throw new ArgumentException($"There is no MovieGenre with the id in the database.");
            }

            this.movieDbContext.MovieGenres.Remove(movieGenre);
            await this.movieDbContext.SaveChangesAsync();
        }
    }
}
