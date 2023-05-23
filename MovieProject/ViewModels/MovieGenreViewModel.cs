using MovieProject.Data.Entities;
using MovieProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.ViewModels
{
    public class MovieGenreViewModel
    {
        public string MovieId { get; set; }
        public MovieViewModel Movie { get; set; }
        public string GenreId { get; set; }
        public GenreViewModel Genre { get; set; }
    }
}
