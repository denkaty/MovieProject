using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IMovieWriterService
    {
        public Task CreateMovieWriterAsync(MovieWriterViewModel movieWriterVM);
        public Task<List<MovieWriterViewModel>> GetAllMovieWritersAsync();
        public Task<MovieWriterViewModel> GetMovieWriterByIdAsync(string movieId, string writerId);
        public Task UpdateMovieWriterAsync(MovieWriterViewModel movieWriterVM);
        public Task<MovieWriterViewModel> UpdateMovieWriterByIdAsync(string movieId, string writerId, MovieWriterViewModel movieWriterVM);
        public Task DeleteMovieWriterByIdAsync(string movieId, string writerId);
    }
}
