using Microsoft.EntityFrameworkCore;

namespace MvcMovie.DataAccess
{
    public class MvcMovieContext : DbContext
    {
        public MvcMovieContext(DbContextOptions<MvcMovieContext> options) : base(options)
        {

        }
    }
}
