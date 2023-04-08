using MovieProject.Data.Entities;
using MovieProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieProject.ViewModels
{
    public class MovieActorViewModel
    {
        public string MovieId { get; set; }
        public MovieViewModel Movie { get; set; }
        public string ActorId { get; set; }
        public ActorViewModel Actor { get; set; }
    }
}
