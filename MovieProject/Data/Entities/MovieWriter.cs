using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.Data.Entities
{
    public class MovieWriter
    {
        [ForeignKey(nameof(Movie))]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey(nameof(Writer))]
        public string WriterId { get; set; }
        public Writer Writer { get; set; }
    }
}
