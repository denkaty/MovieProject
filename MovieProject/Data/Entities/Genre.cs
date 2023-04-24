using System.ComponentModel.DataAnnotations;

namespace MovieProject.Data.Entities
{
    public class Genre
    {
        public Genre()
        {
            this.GenreId = Guid.NewGuid().ToString();
            this.MoviesGenres = new HashSet<MovieGenre>();
        }

        [Key]
        public string GenreId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MovieGenre> MoviesGenres { get; set; }
    }
}
