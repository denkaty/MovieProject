using Newtonsoft.Json;

namespace MovieProject.Services.ApiClient.ViewModels.ImportViewModel
{
    public class MovieStaffImportDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonProperty("known_for_department")]
        public string Department { get; set; }
    }
}
