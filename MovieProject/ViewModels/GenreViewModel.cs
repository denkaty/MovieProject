using MovieProject.Data.Entities;

namespace MovieProject.ViewModels
{
    public class GenreViewModel
    {
        public string GenreId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MovieGenre> MoviesGenres { get; set; }
    }
}
