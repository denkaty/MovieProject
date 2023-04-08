using MovieProject.Data.Entities;

namespace MovieProject.ViewModels
{
    public class DirectorViewModel
    {
        public string DirectorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
