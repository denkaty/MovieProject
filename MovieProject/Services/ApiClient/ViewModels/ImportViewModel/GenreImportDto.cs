using Newtonsoft.Json;

namespace MovieProject.Services.ApiClient.ViewModels.ImportViewModel
{
	public class GenreImportDto
	{
        [JsonProperty("id")]
        public string GenreId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
