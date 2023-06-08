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
        public Task RemoveDirectorFromMovieAsync(string movieId);
        public Task<List<DirectorViewModel>> SearchByNameAsync(string name);
        public Task<List<DirectorViewModel>> GetDirectorsToShowAsync(int? page);
        public int GetDirectorsCount();
    }
}
