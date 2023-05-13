using AutoMapper;
using MovieProject.Data.Entities;
using MovieProject.Services.ApiClient.Interfaces;

namespace MovieProject.Services.ApiClient
{
    public class MovieApiClient : IMovieApiClient
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly IMapper mapper;

        public MovieApiClient(HttpClient httpClient, string apiKey, IMapper mapper)
        {
            this.httpClient = httpClient;
            this.apiKey = apiKey;
            this.mapper = mapper;
        }

        public Task<List<Movie>> FetchActorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> FetchDirectorAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> FetchMovieAsync()
        {
            throw new NotImplementedException();
        }
    }
}
