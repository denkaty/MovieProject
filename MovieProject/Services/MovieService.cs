using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.Services.ApiClient;
using MovieProject.Services.ApiClient.ViewModels.ImportViewModel;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using System.IO;
using static System.Net.WebRequestMethods;

namespace MovieProject.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;
        private readonly MovieApiClient movieApiClient;

        public MovieService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
            this.movieApiClient = new MovieApiClient();
        }
        public async Task CreateMovieAsync(CreateMovieViewModel movieVM)
        {
            Movie movie = new Movie();
            this.mapper.Map(movieVM, movie);

            movie.MovieId = Configuration.GenerateId();
            await SetDirector(movieVM, movie);
            await SetGenres(movieVM, movie);

            await this.movieDbContext.Movies.AddAsync(movie);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task UpdateMovieAsync(UpdateMovieViewModel movieVM)
        {
            Movie? existingMovie = await this.movieDbContext
                .Movies
                .FindAsync(movieVM.MovieId);

            this.mapper.Map(movieVM, existingMovie);
            await UpdateDirector(movieVM, existingMovie);
            await UpdateGenres(movieVM, existingMovie);

            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task<List<MovieViewModel>> GetAllMoviesAsync()
        {
            List<Movie> movies = await this.movieDbContext
                .Movies
                .OrderByDescending(m => m.Released)
                .ToListAsync();

            List<MovieViewModel> movieViewModels = this.mapper.Map<List<MovieViewModel>>(movies);
            return movieViewModels;
        }
        public async Task<List<MovieViewModel>> SearchByTitleAsync(string title)
        {
            List<Movie> movies = await this.movieDbContext
                .Movies
                .OrderByDescending(m => m.Released)
                .Where(x => x.Title
                    .ToLower()
                    .Trim()
                    .Contains(title.ToLower().Trim()))
                .ToListAsync();

            List<MovieViewModel> movieViewModels = this.mapper.Map<List<MovieViewModel>>(movies);
            return movieViewModels;
        }
        public async Task<MovieViewModel> GetMovieByIdAsync(string id)
        {
            Movie? movie = await this.movieDbContext
                .Movies
                .Include(m => m.Director)
                .Include(m => m.MoviesGenres)
                .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.MovieId == id);

            MovieViewModel movieViewModel = this.mapper.Map<MovieViewModel>(movie);
            return movieViewModel;
        }
        public async Task DeleteMovieByIdAsync(string id)
        {
            Movie? movie = await this.movieDbContext.Movies.FindAsync(id);

            this.movieDbContext.Movies.Remove(movie);
            await movieDbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Director>> GetExistingDirectorsAsync()
        {
            IEnumerable<Director> directors = await movieDbContext.Directors.ToListAsync();
            return directors;
        }
        public async Task FetchMoviesAsync()
        {
            List<GenreImportDto> fetchedGenres = await this.movieApiClient.FetchGenresAsync();
            await AddGenresToDb(fetchedGenres);

            List<MovieImportDto> fetchedMovies = await this.movieApiClient.FetchMoviesAsync(5);
            await AddMoviesToDb(fetchedMovies);

            Dictionary<string, List<MovieStaffImportDto>> fetchedMovieStaffs = await this.movieApiClient.FetchMoviesStaffs(fetchedMovies);
            await AddMovieStaffsToDb(fetchedMovieStaffs);

            APIStatus? status = await this.movieDbContext.APIStatus.FindAsync("1");
            status.Fetched = true;
            this.movieDbContext.APIStatus.Update(status);
            await this.movieDbContext.SaveChangesAsync();

        }
        public async Task<bool> GetAPIFetchedStatusAsync()
        {
            APIStatus? apiStatus = await this.movieDbContext.APIStatus.FirstOrDefaultAsync(a => a.Id == "1");
            bool fetchedStatus = apiStatus.Fetched;

            return fetchedStatus;
        }
        public async Task ClearDataAsync()
        {
            this.movieDbContext.MovieActors.RemoveRange(await this.movieDbContext.MovieActors.ToListAsync());
            this.movieDbContext.MovieGenres.RemoveRange(await this.movieDbContext.MovieGenres.ToListAsync());
            this.movieDbContext.Movies.RemoveRange(await this.movieDbContext.Movies.ToListAsync());
            this.movieDbContext.Actors.RemoveRange(await this.movieDbContext.Actors.ToListAsync());
            this.movieDbContext.Genres.RemoveRange(await this.movieDbContext.Genres.ToListAsync());
            this.movieDbContext.Directors.RemoveRange(await this.movieDbContext.Directors.ToListAsync());

            APIStatus? status = await this.movieDbContext.APIStatus.FindAsync("1");
            status.Fetched = false;
            this.movieDbContext.APIStatus.Update(status);

            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task<List<MovieViewModel>> GetMoviesToShowAsync(int? page)
        {

            int moviesPerPage = 21;
            int startIndex = (int)((page - 1) * moviesPerPage);

            List<Movie> movies = await this.movieDbContext
                .Movies
                .OrderByDescending(m => m.Released)
                .Skip(startIndex)
                .Take(moviesPerPage)
                .ToListAsync();

            List<MovieViewModel> movieViewModels = this.mapper.Map<List<MovieViewModel>>(movies);
            return movieViewModels;
        }
        public int GetMoviesCount()
        {
            int count = this.movieDbContext.Movies.Count();
            return count;
        }
        private async Task SetDirector(CreateMovieViewModel movieVM, Movie movie)
        {
            string[] directorNames = movieVM.DirectorFullName
                            .Split(" ")
                            .ToArray();

            Director? director = await this.movieDbContext.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.FirstName == directorNames[0] && d.LastName == directorNames[1]);

            if (director == null)
            {
                director = new Director
                {
                    DirectorId = Configuration.GenerateId(),
                    FirstName = directorNames[0],
                    LastName = directorNames[1]
                };
                await this.movieDbContext.Directors.AddAsync(director);
            }

            movie.Director = director;
            movie.DirectorId = director.DirectorId;
        }
        private async Task SetGenres(CreateMovieViewModel movieVM, Movie movie)
        {
            string[] genreNamesFromInput = movieVM.Genres.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var currentGenreName in genreNamesFromInput)
            {
                Genre? genreFromDb = await this.movieDbContext
                    .Genres
                    .SingleOrDefaultAsync(g => g.Name == currentGenreName);

                if (genreFromDb == null)
                {
                    genreFromDb = new Genre
                    {
                        GenreId = Configuration.GenerateId(),
                        Name = currentGenreName
                    };

                    this.movieDbContext.Genres.Add(genreFromDb);
                }

                MovieGenre movieGenre = new MovieGenre
                {
                    MovieId = movie.MovieId,
                    GenreId = genreFromDb.GenreId,
                };
                this.movieDbContext.MovieGenres.Add(movieGenre);
            }
        }
        private async Task UpdateDirector(UpdateMovieViewModel movieVM, Movie? existingMovie)
        {
            string[] directorNames = movieVM.DirectorFullName
                            .Split(" ")
                            .ToArray();

            Director? director = await this.movieDbContext
                .Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.FirstName == directorNames[0] && d.LastName == directorNames[1]);

            if (director == null)
            {
                director = new Director
                {
                    DirectorId = Configuration.GenerateId(),
                    FirstName = directorNames[0],
                    LastName = directorNames[1]
                };
                await this.movieDbContext.Directors.AddAsync(director);
            }

            existingMovie.Director = director;
            existingMovie.DirectorId = director.DirectorId;
        }
        private async Task UpdateGenres(UpdateMovieViewModel movieVM, Movie? existingMovie)
        {
            await ClearMovieGenres(existingMovie);

            string[] genreNamesFromInput = movieVM.Genres.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            foreach (var currentGenreName in genreNamesFromInput)
            {
                Genre? genreFromDb = await this.movieDbContext
                    .Genres
                    .SingleOrDefaultAsync(g => g.Name == currentGenreName);
                if (genreFromDb == null)
                {
                    genreFromDb = new Genre
                    {
                        GenreId = Configuration.GenerateId(),
                        Name = currentGenreName
                    };
                    this.movieDbContext.Genres.Add(genreFromDb);
                }

                MovieGenre movieGenre = new MovieGenre
                {
                    MovieId = existingMovie.MovieId,
                    GenreId = genreFromDb.GenreId,
                };
                this.movieDbContext.MovieGenres.Add(movieGenre);
            }
        }
        private async Task AddGenresToDb(List<GenreImportDto> fetchedGenres)
        {
            List<Genre> genres = this.mapper.Map<List<Genre>>(fetchedGenres);
            await this.movieDbContext.Genres.AddRangeAsync(genres);
            await this.movieDbContext.SaveChangesAsync();
        }
        private async Task AddMoviesToDb(List<MovieImportDto> fetchedMovies)
        {
            List<Movie> movies = this.mapper.Map<List<Movie>>(fetchedMovies);
            foreach (var movie in movies)
            {
                bool isMovieExisting = await this.movieDbContext.Movies.AnyAsync(m => m.MovieId == movie.MovieId);
                if (!isMovieExisting)
                {
                    movie.Poster = "https://image.tmdb.org/t/p/original" + movie.Poster;
                    this.movieDbContext.Movies.Add(movie);
                }
            }
            await this.movieDbContext.SaveChangesAsync();


            foreach (Movie movie in movies)
            {
                foreach (var genreId in movie.Genres.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray())
                {
                    Genre? genre = await this.movieDbContext.Genres.FindAsync(genreId);

                    MovieGenre movieGenre = new MovieGenre
                    {
                        MovieId = movie.MovieId,
                        Movie = movie,
                        GenreId = genreId,
                        Genre = genre
                    };

                    await this.movieDbContext.MovieGenres.AddAsync(movieGenre);
                    await this.movieDbContext.SaveChangesAsync();
                }
            }

        }
        private async Task AddMovieStaffsToDb(Dictionary<string, List<MovieStaffImportDto>> fetchedMovieStaffs)
        {
            foreach (var entry in fetchedMovieStaffs)
            {
                bool isDirectorSet = false;
                string movieId = entry.Key;
                List<MovieStaffImportDto> staffList = entry.Value;
                Movie? movie = await this.movieDbContext.Movies.FindAsync(movieId);
                foreach (var staff in staffList)
                {
                    if (string.IsNullOrEmpty(staff.FirstName) || string.IsNullOrEmpty(staff.LastName))
                    {
                        continue;
                    }
                    if (staff.Department == "Directing")
                    {
                        if (!isDirectorSet)
                        {
                            Director director = mapper.Map<Director>(staff);
                            bool isDirectorExisting = await this.movieDbContext.Directors.AnyAsync(d => d.DirectorId == director.DirectorId);

                            if (!isDirectorExisting)
                            {
                                this.movieDbContext.Directors.Add(director);
                                await this.movieDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                this.movieDbContext.Entry(director).State = EntityState.Detached;
                                director = await this.movieDbContext.Directors.FindAsync(director.DirectorId);
                            }

                            movie.Director = director;
                            this.movieDbContext.Movies.Update(movie);
                            await this.movieDbContext.SaveChangesAsync();
                            isDirectorSet = true;
                        }
                    }
                    else
                    {
                        Actor actor = mapper.Map<Actor>(staff);
                        bool isActorExisting = await this.movieDbContext.Actors.AnyAsync(a => a.ActorId == actor.ActorId);
                        if (!isActorExisting)
                        {
                            this.movieDbContext.Actors.Add(actor);
                            await this.movieDbContext.SaveChangesAsync();
                        }
                        else
                        {
                            this.movieDbContext.Entry(actor).State = EntityState.Detached;
                            actor = await this.movieDbContext.Actors.FindAsync(actor.ActorId);
                        }

                        MovieActor movieActor = new MovieActor
                        {
                            MovieId = movieId,
                            Movie = movie,
                            ActorId = actor.ActorId,
                            Actor = actor
                        };

                        await this.movieDbContext.MovieActors.AddAsync(movieActor);
                        await this.movieDbContext.SaveChangesAsync();

                    }
                }
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
