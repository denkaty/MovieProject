using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IMovieActorService
    {
        public Task CreateMovieActorAsync(MovieActorViewModel movieActorVM);
        public Task<List<MovieActorViewModel>> GetAllMovieActorsAsync();
        public Task<MovieActorViewModel> GetMovieActorByIdAsync(string movieId, string actorId);
        public Task UpdateMovieActorAsync(MovieActorViewModel movieActorVM);
        public Task<MovieActorViewModel> UpdateMovieActorByIdAsync(string movieId, string actorId, MovieActorViewModel movieActorVM);
        public Task DeleteMovieActorByIdAsync(string movieId, string actorId);
    }
}
