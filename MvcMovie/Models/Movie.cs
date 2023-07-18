namespace MvcMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}