using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.Data.Entities
{
    public class MovieActor
    {
        [ForeignKey(nameof(Movie))]
        public string MovieId { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey(nameof(Actor))]
        public string ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
