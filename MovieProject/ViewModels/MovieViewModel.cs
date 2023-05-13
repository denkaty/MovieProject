using MovieProject.Data.Entities;
using MovieProject.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace MovieProject.Models
{
    public class MovieViewModel
    {
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Released { get; set; }
        public int Runtime { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Poster { get; set; }
        public decimal BoxOffice { get; set; }
        public string DirectorId { get; set; }
        public Director Director { get; set; }

        [Display(Name = "Director")]
        public string DirectorFullName { get; set; }
        public string Genres { get; set; }
        public ICollection<MovieActorViewModel> MoviesActors { get; set; }
        public ICollection<MovieGenreViewModel> MoviesGenres { get; set; }
    }
}
