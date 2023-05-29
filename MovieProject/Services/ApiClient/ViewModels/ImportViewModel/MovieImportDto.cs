using Newtonsoft.Json;

namespace MovieProject.Services.ApiClient.ViewModels.ImportViewModel
{
    public class MovieImportDto
    {
        [JsonProperty("id")]
        public string MovieId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("release_date")]
        public string Released { get; set; }

        [JsonProperty("overview")]
        public string Plot { get; set; }

        [JsonProperty("original_language")]
        public string Language { get; set; }

        [JsonProperty("poster_path")]
        public string Poster { get; set; }

        [JsonProperty("genre_ids")]
        public string[] Genres { get; set; }
    }
}
