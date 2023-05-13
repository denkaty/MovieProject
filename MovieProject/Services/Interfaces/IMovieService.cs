using MovieProject.Models;

namespace MovieProject.Services.Interfaces
{
    public interface IMovieService
    {
        public Task CreateMovieAsync(MovieViewModel movieVM);
        public Task<List<MovieViewModel>> GetAllMoviesAsync();
        public Task<MovieViewModel> GetMovieByIdAsync(string id);
        public Task UpdateMovieAsync(MovieViewModel movieVM);
        public Task DeleteMovieByIdAsync(string id);
    }
}
