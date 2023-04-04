using MovieProject.Services.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IMovieService
    {
        public Task CreateMovieAsync(MovieVM movieVM);
        public Task<List<MovieVM>> GetAllMoviesAsync();
        public Task<MovieVM> GetMovieByIdAsync(string id);
        public Task UpdateMovieAsync(MovieVM movieVM);
        public Task<MovieVM> UpdateMovieByIdAsync(string id);
        public Task DeleteMovieByIdAsync(string id);
    }
}
