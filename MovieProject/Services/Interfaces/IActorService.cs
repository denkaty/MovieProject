using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IActorService
    {
        public Task CreateActorAsync(ActorViewModel actorVM);
        public Task<List<ActorViewModel>> GetAllActorsAsync();
        public Task<ActorViewModel> GetActorByIdAsync(string id);
        public Task UpdateActorAsync(ActorViewModel actorVM);
        public Task DeleteActorByIdAsync(string id);
        public Task RemoveActorFromMovieAsync(string movieId, string actorId);
        public Task<IEnumerable<Movie>> GetMoviesAsync(string actorId);
        public Task<bool> CreateNewRoleAsync(MovieActorViewModel movieActorViewModel);
        public Task<List<ActorViewModel>> SearchByNameAsync(string name);
        public Task<List<ActorViewModel>> GetActorsToShowAsync(int? page);
        public int GetActorsCount();
    }
}
