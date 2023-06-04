using System.ComponentModel.DataAnnotations;

namespace MovieProject.Data.Entities
{
    public class APIStatus
    {
        [Key]
        public string Id { get; set; }
        public bool Fetched { get; set; }

    }
}
