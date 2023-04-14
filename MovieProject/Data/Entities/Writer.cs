using System.ComponentModel.DataAnnotations;

namespace MovieProject.Data.Entities
{
    public class Writer
    {
        public Writer()
        {
            this.WriterId = Guid.NewGuid().ToString();
            this.MoviesWriters = new List<MovieWriter>();
        }

        [Key]
        public string WriterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<MovieWriter> MoviesWriters { get; set; }
    }
}
