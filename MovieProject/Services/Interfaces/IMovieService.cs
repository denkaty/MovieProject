using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IMovieService
    {
        public Task CreateMovieAsync(CreateMovieViewModel movieVM);
        public Task UpdateMovieAsync(UpdateMovieViewModel movieVM);
        public Task<List<MovieViewModel>> GetAllMoviesAsync();
        public Task<List<MovieViewModel>> SearchByTitleAsync(string title);
        public Task<MovieViewModel> GetMovieByIdAsync(string id);
        public Task DeleteMovieByIdAsync(string id);
        public Task<IEnumerable<Director>> GetExistingDirectorsAsync();
        public Task FetchMoviesAsync();
        public Task<bool> GetAPIFetchedStatusAsync();
        public Task ClearDataAsync();
        public Task<List<MovieViewModel>> GetMoviesToShowAsync(int? page);
        public int GetMoviesCount();
    }
}
