namespace MvcMovie.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; }

        public Movie Movie { get; set; }
    }
}
