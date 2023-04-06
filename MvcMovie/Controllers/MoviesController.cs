using Microsoft.AspNetCore.Mvc;
using MvcMovie.DataAccess;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {

        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies;
            return View(movies);
        }
    }
}