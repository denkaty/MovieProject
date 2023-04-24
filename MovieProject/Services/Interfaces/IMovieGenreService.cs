using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IMovieGenreService
    {
        public Task CreateMovieGenreAsync(MovieGenreViewModel movieGenreVM);
        public Task<List<MovieGenreViewModel>> GetAllMovieGenresAsync();
        public Task<MovieGenreViewModel> GetMovieGenreByIdAsync(string movieId, string genreId);
        public Task UpdateMovieGenreAsync(MovieGenreViewModel movieGenreVM);
        public Task<MovieGenreViewModel> UpdateMovieGenreByIdAsync(string movieId, string genreId, MovieGenreViewModel movieGenreVM);
        public Task DeleteMovieGenreByIdAsync(string movieId, string genreId);
    }
}
