using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.Data.Entities
{
    public class Movie
    {
        public Movie()
        {
            this.MovieId = Guid.NewGuid().ToString();
            this.MoviesActors = new HashSet<MovieActor>();
        }
        [Key]
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public int Runtime { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Poster { get; set; }
        public decimal BoxOffice { get; set; }

        [ForeignKey(nameof(Director))]
        public string DirectorId { get; set; }
        public Director Director { get; set; }
        public virtual ICollection<MovieActor> MoviesActors{ get; set; }
        public virtual ICollection<MovieGenre> MoviesGenres { get; set; }
    }
}
