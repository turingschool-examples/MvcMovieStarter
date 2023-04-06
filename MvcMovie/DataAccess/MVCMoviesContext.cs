using Microsoft.EntityFrameworkCore;

namespace MvcMovie.DataAccess
{
    public class MVCMoviesContext : DbContext
    {
        public MVCMoviesContext(DbContextOptions<MVCMoviesContext> options) : base(options)
        {

        }
    }
}
