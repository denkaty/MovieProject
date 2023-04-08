using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.Services.Interfaces;
using System.IO;

namespace MovieProject.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public MovieService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }

        public async Task CreateMovieAsync(MovieViewModel movieVM)
        {
            if (movieVM == null)
            {
                throw new ArgumentNullException("The MovieViewModel parameter cannot be null.", nameof(movieVM));
            }

            Movie movie = this.mapper.Map<Movie>(movieVM);
            await this.movieDbContext.Movies.AddAsync(movie);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<MovieViewModel>> GetAllMoviesAsync()
        {
            List<Movie> movies = await this.movieDbContext.Movies.ToListAsync();
            List<MovieViewModel> movieViewModels = this.mapper.Map<List<MovieViewModel>>(movies);
            return movieViewModels;
        }

        public async Task<MovieViewModel> GetMovieByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Movie? movie = await this.movieDbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new ArgumentException("No Movie was found with the given id.", nameof(id));
            }
            MovieViewModel movieViewModel = this.mapper.Map<MovieViewModel>(movie);
            return movieViewModel;
        }

        public async Task UpdateMovieAsync(MovieViewModel movieVM)
        {
            if (movieVM == null)
            {
                throw new ArgumentNullException("The MovieViewModel parameter cannot be null.", nameof(movieVM));
            }

            Movie movie = this.mapper.Map<Movie>(movieVM);
            this.movieDbContext.Movies.Update(movie);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<MovieViewModel> UpdateMovieByIdAsync(string id)
        {
            MovieViewModel? movieVM = await movieDbContext
                .Movies
                .Where(movie => movie.MovieId == id)
                .Select(movie => new MovieViewModel()
                {
                    Title = movie.Title,
                    Year = movie.Year,
                    Released = movie.Released,
                    Runtime = movie.Runtime,
                    Genre = movie.Genre,
                    Plot = movie.Plot,
                    Language = movie.Language,
                    Country = movie.Country,
                    Poster = movie.Poster,
                    BoxOffice = movie.BoxOffice,
                    MoviesActors = movie.MoviesActors,
                    MoviesWriters = movie.MoviesWriters
                }).SingleOrDefaultAsync();

            return movieVM;
        }
        public async Task DeleteMovieByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Movie? movie = await this.movieDbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new ArgumentException($"There is no movie with the id {id} in the database.", nameof(id));
            }

            this.movieDbContext.Movies.Remove(movie);
            await movieDbContext.SaveChangesAsync();
        }
    }
}
