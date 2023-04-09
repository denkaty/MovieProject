using MovieProject.Data.Entities;

namespace MovieProject.ViewModels
{
    public class MovieWriterViewModel
    {
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
        public string WriterId { get; set; }
        public Writer Writer { get; set; }
    }
}
