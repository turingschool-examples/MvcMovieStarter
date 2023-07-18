using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.DataAccess;

namespace MvcMovie.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly MvcMovieContext _context;

        public ReviewsController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: /movies/:movieId/reviews
        [Route("Movies/{movieId:int}/reviews")]
        public IActionResult Index(int movieId)
        {
            var movie = _context.Movies
                .Where(m => m.Id == movieId)
                .Include(m => m.Reviews)
                .First();

            //var movie2 = _context.Movies
            //    .Find(movieId)

            return View(movie);
        }
    }
}
