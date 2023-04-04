using System.ComponentModel.DataAnnotations;

namespace MovieProject.Data.Entities
{
    public class Director
    {
        public Director()
        {
            this.Movies = new HashSet<Movie>();
        }
        [Key]
        public string DirectorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
