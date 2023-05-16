using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
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
            movie.MovieId = Configuration.GenerateId();
            await SetDirector(movieVM, movie);
            await SetGenres(movieVM, movie);
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

            Movie? movie = await this.movieDbContext.Movies.Include(m => m.Director).Include(m => m.MoviesGenres).ThenInclude(mg => mg.Genre).FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                throw new ArgumentException("No Movie was found with the given id.", nameof(id));
            }
            MovieViewModel movieViewModel = this.mapper.Map<MovieViewModel>(movie);
            return movieViewModel;
        }

        public async Task UpdateMovieAsync(MovieViewModel movieVM)
        {

            Movie movie = this.mapper.Map<Movie>(movieVM);

            this.movieDbContext.Entry(movie).State = EntityState.Detached;

            await SetDirector(movieVM, movie);
            await SetGenres(movieVM, movie);

            Movie? existingMovie = await this.movieDbContext.Movies.FindAsync(movie.MovieId);
            if (existingMovie != null)
            {
                //Avoiding InvalidOperationException: The instance of entity type 'Movie' cannot be tracked because another instance with the key value '{MovieId: b3f2fa67-7fa9-493c-9085-f190131f1772}' is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached.
                this.mapper.Map(movie, existingMovie);
                this.movieDbContext.Movies.Update(existingMovie);
                await this.movieDbContext.SaveChangesAsync();
            }
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
        public async Task<IEnumerable<Director>> GetExistingDirectors()
        {
            IEnumerable<Director> directors =  await movieDbContext.Directors.ToListAsync();
            return directors;
        }
        private async Task SetDirector(MovieViewModel movieVM, Movie movie)
        {
            string[] directorNames = movieVM.DirectorFullName.Split(" ").ToArray();
            Director? director = await this.movieDbContext.Directors.Include(d => d.Movies).FirstOrDefaultAsync(d => d.FirstName == directorNames[0] && d.LastName == directorNames[1]);
            if (director == null)
            {
                throw new Exception($"There is no director with this name {movieVM.Director.FirstName} in the database.");
            }

            movie.Director = director;
            movie.DirectorId = director.DirectorId;

        }

        private async Task SetGenres(MovieViewModel movieVM, Movie movie)
        {
            this.movieDbContext.Entry(movie).State = EntityState.Detached;

            await ClearMovieGenres(movie);

            string[] genreNamesFromInput = movieVM.Genres.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            
            foreach (var currentGenreName in genreNamesFromInput)
            {
                Genre? genreFromDb = await this.movieDbContext.Genres.SingleOrDefaultAsync(g => g.Name == currentGenreName);
                if (genreFromDb == null)
                {
                    genreFromDb = new Genre
                    {
                        GenreId = Configuration.GenerateId(),
                        Name = currentGenreName
                    };
                    this.movieDbContext.Genres.Add(genreFromDb);
                    await this.movieDbContext.SaveChangesAsync();
                }

                MovieGenre movieGenre = new MovieGenre
                {
                    MovieId = movie.MovieId,
                    GenreId = genreFromDb.GenreId,
                };
                this.movieDbContext.MovieGenres.Add(movieGenre);

            }
        }

        private async Task ClearMovieGenres(Movie movie)
        {
            List<MovieGenre>? movieGenres = this.movieDbContext
                                .MovieGenres
                                .Include(mg => mg.Movie)
                                .Include(mg => mg.Genre)
                                .Where(mg => mg.Movie.MovieId == movie.MovieId)
                                .ToList();

            if (movieGenres != null && movieGenres.Count() > 0)
            {
                foreach (var currentMovieGenre in movieGenres)
                {
                    this.movieDbContext.MovieGenres.Remove(currentMovieGenre);
                }
                await this.movieDbContext.SaveChangesAsync();
            }
        }

    }
}
