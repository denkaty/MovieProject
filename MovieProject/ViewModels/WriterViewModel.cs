using MovieProject.Data.Entities;

namespace MovieProject.ViewModels
{
    public class WriterViewModel
    {
        public string WriterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<MovieWriter> MoviesWriters { get; set; }
    }
}
