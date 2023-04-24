using MovieProject.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.ViewModels
{
    public class MovieGenreViewModel
    {
        public string MovieId { get; set; }
        public Movie Movie { get; set; }
        public string GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
