using AutoMapper;
using MovieProject.Data.Entities;
using MovieProject.MappingConfiguration;
using MovieProject.Services.ApiClient.Interfaces;
using MovieProject.Services.ApiClient.ViewModels.ImportViewModel;
using Newtonsoft.Json;
using System.Net.Http;

namespace MovieProject.Services.ApiClient
{
    public class MovieApiClient
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly IMapper mapper;

        public MovieApiClient()
        {
            this.httpClient = new HttpClient();
            this.apiKey = "8a274eb881367b096e37ea215599ff7b";
        }
        public async Task<List<MovieImportDto>> FetchMoviesAsync(int pages)
        {
            List<MovieImportDto> movieImportDtos = new List<MovieImportDto>();
            for (int i = 1; i <= pages; i++)
            {
                string apiUrl = $"https://api.themoviedb.org/3/discover/movie?api_key={apiKey}&page={i}";
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiUrl);
                httpResponseMessage.EnsureSuccessStatusCode();

                string json = await httpResponseMessage.Content.ReadAsStringAsync();

                List<MovieImportDto> movieImportPageDtos = JsonConvert.DeserializeObject<dynamic>(json).results.ToObject<List<MovieImportDto>>();
                movieImportPageDtos = movieImportPageDtos.Where(m => m.Released != null).ToList();
                movieImportDtos.AddRange(movieImportPageDtos);
            }

            return movieImportDtos;
        }
        public async Task<List<GenreImportDto>> FetchGenresAsync()
        {
            string apiUrl = $"https://api.themoviedb.org/3/genre/movie/list?api_key={apiKey}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiUrl);
            httpResponseMessage.EnsureSuccessStatusCode();

            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            List<GenreImportDto> genreImportDtos = JsonConvert.DeserializeObject<dynamic>(json).genres.ToObject<List<GenreImportDto>>();

            return genreImportDtos;
        }
        public async Task<Dictionary<string, List<MovieStaffImportDto>>> FetchMoviesStaffs(List<MovieImportDto> fetchedMovies)
        {
            Dictionary<string, List<MovieStaffImportDto>> moviesStaffs = new Dictionary<string, List<MovieStaffImportDto>>();

            foreach (var movie in fetchedMovies)
            {
                List<MovieStaffImportDto> movieStaffImportDtos = new List<MovieStaffImportDto>();
                string apiUrl = $"http://api.themoviedb.org/3/movie/{movie.MovieId}/casts?api_key={apiKey}";
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(apiUrl);
                httpResponseMessage.EnsureSuccessStatusCode();

                string json = await httpResponseMessage.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                dynamic castData = data.cast;
                dynamic crewData = data.crew;

                foreach (var staff in castData)
                {
                    string firstName, lastName, department, staffId;
                    string fullName = staff.name;
                    dynamic nameParts = fullName.Split(" ");
                    if (nameParts.Length == 1)
                    {
                        firstName = nameParts[0];
                        lastName = string.Empty;
                    }
                    else
                    {
                        firstName = nameParts[0];
                        lastName = nameParts[nameParts.Length - 1];
                    }
                    department = staff.known_for_department;
                    staffId = staff.cast_id;

                    MovieStaffImportDto movieStaff = new MovieStaffImportDto
                    {
                        Id = staffId,
                        FullName = fullName,
                        FirstName = firstName,
                        LastName = lastName,
                        Department = department
                    };
                    movieStaffImportDtos.Add(movieStaff);
                }

                foreach (var staff in crewData)
                {
                    string firstName, lastName, department, staffId, job;
                    department = "Directing";
                    job = staff.job;
                    if (job != "Director")
                    {
                        continue;
                    }
                    string fullName = staff.name;
                    dynamic nameParts = fullName.Split(" ");
                    if (nameParts.Length == 1)
                    {
                        firstName = nameParts[0];
                        lastName = string.Empty;
                    }
                    else
                    {
                        firstName = nameParts[0];
                        lastName = nameParts[1];
                    }
                    staffId = staff.id;

                    MovieStaffImportDto movieStaff = new MovieStaffImportDto
                    {
                        Id = staffId,
                        FullName = fullName,
                        FirstName = firstName,
                        LastName = lastName,
                        Department = department
                    };
                    movieStaffImportDtos.Add(movieStaff);
                    break;
                }
                if (!moviesStaffs.ContainsKey(movie.MovieId))
                {
                    moviesStaffs.Add(movie.MovieId, new List<MovieStaffImportDto>());
                }
                moviesStaffs[movie.MovieId] = movieStaffImportDtos;
            }

            return moviesStaffs;
        }

    }
}
