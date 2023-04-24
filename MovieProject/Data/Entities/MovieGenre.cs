using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.Data.Entities
{
    public class MovieGenre
    {
        [ForeignKey(nameof(Movie))]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey(nameof(Genre))]
        public string GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
