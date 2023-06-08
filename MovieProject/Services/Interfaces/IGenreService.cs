using MovieProject.Data.Entities;
using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IGenreService
    {
        public Task CreateGenreAsync(GenreViewModel genreVM);
        public Task<List<GenreViewModel>> GetAllGenresAsync();
        public Task<GenreViewModel> GetGenreByIdAsync(string id);
        public Task UpdateGenreAsync(GenreViewModel genreVM);
        public Task DeleteGenreByIdAsync(string id);
        public Task<List<GenreViewModel>> GetGenresToShowAsync(int? page);
        public Task RemoveGenreFromMovieAsync(string movieId, string genreId);
        public Task<IEnumerable<Movie>> GetAllMoviesAsync();
        public Task<bool> ManageNewMovieGenreAsync(MovieGenreViewModel movieGenreViewModel);
        public Task<List<GenreViewModel>> SearchByNameAsync(string genre);
        public int GetGenresCount();
    }
}
