using AutoMapper;
using MovieProject.Data.Entities;
using MovieProject.Data;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using MovieProject.Models;

namespace MovieProject.Services
{
    public class GenreService : IGenreService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public GenreService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }
        public async Task CreateGenreAsync(GenreViewModel genreVM)
        {
            if (genreVM == null)
            {
                throw new ArgumentNullException("The GenreViewModel parameter cannot be null.", nameof(genreVM));
            }

            Genre genre = this.mapper.Map<Genre>(genreVM);
            genre.GenreId = Configuration.GenerateId();
            await this.movieDbContext.Genres.AddAsync(genre);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<GenreViewModel>> GetAllGenresAsync()
        {
            List<Genre> genres = await this.movieDbContext.Genres.Include(g => g.MoviesGenres).ThenInclude(mg => mg.Movie).ToListAsync();
            List<GenreViewModel> genresViewModels = this.mapper.Map<List<GenreViewModel>>(genres);
            return genresViewModels;
        }
        public async Task<GenreViewModel> GetGenreByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Genre? genre = await this.movieDbContext.Genres.Include(a => a.MoviesGenres).ThenInclude(a => a.Movie).FirstOrDefaultAsync(a => a.GenreId == id);
            if (genre == null)
            {
                throw new ArgumentException("No Genre was found with the given id.", nameof(id));
            }

            GenreViewModel genreViewModel = this.mapper.Map<GenreViewModel>(genre);
            return genreViewModel;
        }
        public async Task UpdateGenreAsync(GenreViewModel genreVM)
        {
            if (genreVM == null)
            {
                throw new ArgumentNullException("The GenreViewModel parameter cannot be null.", nameof(genreVM));
            }
            Genre genre = this.mapper.Map<Genre>(genreVM);
            this.movieDbContext.Genres.Update(genre);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task DeleteGenreByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Genre? genre = await this.movieDbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                throw new ArgumentException($"There is no genre with the id {id} in the database.", nameof(id));
            }

            this.movieDbContext.Genres.Remove(genre);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task RemoveGenreFromMovie(string movieId, string genreId)
        {
            Movie? movie = await this.movieDbContext.Movies.FindAsync(movieId);
            Genre? genre = await this.movieDbContext.Genres.FindAsync(genreId);

            MovieGenre movieGenre = new MovieGenre
            {
                MovieId = movie.MovieId,
                Movie = movie,
                GenreId = genre.GenreId,
                Genre = genre
            };
            this.movieDbContext.MovieGenres.Remove(movieGenre);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            IEnumerable<Movie> movies = await this.movieDbContext.Movies.ToListAsync();
            return movies;
        }

        public async Task<bool> ManageNewMovieGenre(MovieGenreViewModel movieGenreViewModel)
        {
            string movieTitle = movieGenreViewModel.Movie.Title;
            string genreId = movieGenreViewModel.GenreId;

            Movie? movie = await this.movieDbContext.Movies.SingleOrDefaultAsync(m => m.Title == movieGenreViewModel.Movie.Title);
            Genre? genre = await this.movieDbContext.Genres.SingleOrDefaultAsync(a => a.GenreId == genreId);

            if (movie == null || genre == null)
            {
                return false;
            }
            MovieViewModel movieViewModel = this.mapper.Map<MovieViewModel>(movie);
            GenreViewModel genreViewModel = this.mapper.Map<GenreViewModel>(genre);

            MovieGenre movieGenre = new MovieGenre
            {
                MovieId = movie.MovieId,
                Movie = movie,
                GenreId = genre.GenreId,
                Genre = genre
            };

            if (this.movieDbContext.MovieGenres.Contains(movieGenre))
            {
                return false;
            }
            this.movieDbContext.MovieGenres.Add(movieGenre);
            await this.movieDbContext.SaveChangesAsync();
            return true;
        }
    }
}
