using MovieProject.Data.Entities;
using MovieProject.Models;

namespace MovieProject.ViewModels
{
    public class DirectorViewModel
    {
        public string DirectorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<MovieViewModel> Movies { get; set; }
    }
}
