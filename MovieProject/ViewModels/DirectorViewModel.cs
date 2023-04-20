using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieProject.Data.Entities;
using MovieProject.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieProject.ViewModels
{
    public class DirectorViewModel
    {
        public string DirectorId { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public virtual ICollection<MovieViewModel> Movies { get; set; }
    }
}
