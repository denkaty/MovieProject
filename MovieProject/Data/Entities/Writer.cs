using System.ComponentModel.DataAnnotations;

namespace MovieProject.Data.Entities
{
    public class Writer
    {
        public Writer()
        {
            this.MoviesWriters = new List<MovieWriter>();
        }

        [Key]
        public string WriterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<MovieWriter> MoviesWriters { get; set; }
    }
}
