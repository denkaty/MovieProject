using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IDirectorService
    {
        public Task CreateDirectorAsync(DirectorViewModel directorVM);
        public Task<List<DirectorViewModel>> GetAllDirectorsAsync();
        public Task<DirectorViewModel> GetDirectoryIdAsync(string id);
        public Task UpdateDirectorAsync(DirectorViewModel directorVM);
        public Task DeleteDirectorByIdAsync(string id);
    }
}
