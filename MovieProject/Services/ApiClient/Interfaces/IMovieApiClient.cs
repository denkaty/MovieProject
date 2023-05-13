using MovieProject.Data.Entities;

namespace MovieProject.Services.ApiClient.Interfaces
{
    public interface IMovieApiClient
    {
        public Task<List<Movie>> FetchMovieAsync();
        public Task<List<Movie>> FetchDirectorAsync();
        public Task<List<Movie>> FetchActorsAsync();

    }
}
