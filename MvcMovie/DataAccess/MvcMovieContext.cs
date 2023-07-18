using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.DataAccess
{
    public class MvcMovieContext : DbContext
    {

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public MvcMovieContext(DbContextOptions<MvcMovieContext> options) : base(options)
        {

        }
    }
}
