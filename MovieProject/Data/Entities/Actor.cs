using System.ComponentModel.DataAnnotations;

namespace MovieProject.Data.Entities
{
    public class Actor
    {
        public Actor()
        {
            //?
            this.ActorId = Guid.NewGuid().ToString();
            this.MoviesActors = new HashSet<MovieActor>();
        }

        [Key]
        public string ActorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<MovieActor> MoviesActors { get; set; }
    }
}
