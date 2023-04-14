using MovieProject.Data.Entities;

namespace MovieProject.ViewModels
{
    public class ActorViewModel
    {
        public string ActorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<MovieActorViewModel> MoviesActors { get; set; }
    }
}