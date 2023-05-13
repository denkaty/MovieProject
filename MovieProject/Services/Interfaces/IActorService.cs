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
    }
}
