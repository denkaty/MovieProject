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
    }
}
