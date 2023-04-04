using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Services.Interfaces;
using MovieProject.Services.ViewModels;

namespace MovieProject.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext movieDbContext;

        public MovieService(MovieDbContext movieDbContext)
        {
            this.movieDbContext = movieDbContext;
        }

        public async Task CreateMovieAsync(MovieVM movieVM)
        {
            Movie movie = new Movie()
            {
                MovieId = Guid.NewGuid().ToString(),
                Title = movieVM.Title,
                Year = movieVM.Year,
                Released = movieVM.Released,
                Runtime = movieVM.Runtime,
                Genre = movieVM.Genre,
                Plot = movieVM.Plot,
                Language = movieVM.Language,
                Country = movieVM.Country,
                Poster = movieVM.Poster,
                BoxOffice = movieVM.BoxOffice,
                MoviesActors = movieVM.MoviesActors,
                MoviesWriters = movieVM.MoviesWriters
            };

            await movieDbContext.Movies.AddAsync(movie);
            await movieDbContext.SaveChangesAsync();
        }

        public async Task<List<MovieVM>> GetAllMoviesAsync()
        {
            List<MovieVM> moviesVM = await movieDbContext
                         .Movies
                         .Select(movie => new MovieVM()
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
                         }).ToListAsync();

            return moviesVM;
        }

        public async Task<MovieVM> GetMovieByIdAsync(string id)
        {
            MovieVM? movieVM = await movieDbContext
                .Movies
                .Where(movie => movie.MovieId == id)
                .Select(movie => new MovieVM()
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
                })
                .SingleOrDefaultAsync();

            return movieVM;
        }

        public async Task UpdateMovieAsync(MovieVM movieVM)
        {
            Movie? movie = await movieDbContext
                .Movies
                .FindAsync(movieVM.MovieId);

            if (movie == null)
            {
                return;
            }

            movie.Title = movieVM.Title;
            movie.Year = movieVM.Year;
            movie.Released = movieVM.Released;
            movie.Runtime = movieVM.Runtime;
            movie.Genre = movieVM.Genre;
            movie.Plot = movieVM.Plot;
            movie.Language = movieVM.Language;
            movie.Country = movieVM.Country;
            movie.Poster = movieVM.Poster;
            movie.BoxOffice = movieVM.BoxOffice;
            movie.MoviesActors = movieVM.MoviesActors;
            movie.MoviesWriters = movieVM.MoviesWriters;

            movieDbContext.Update(movie);
            await movieDbContext.SaveChangesAsync();
        }

        public async Task<MovieVM> UpdateMovieByIdAsync(string id)
        {
            MovieVM? movieVM = await movieDbContext
                .Movies
                .Where(movie => movie.MovieId == id)
                .Select(movie => new MovieVM()
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
            Movie? movie = await movieDbContext
                .Movies
                .SingleOrDefaultAsync(movie => movie.MovieId == id);

            movieDbContext
                .Movies
                .Remove(movie);

            await movieDbContext.SaveChangesAsync();
        }
    }
}
