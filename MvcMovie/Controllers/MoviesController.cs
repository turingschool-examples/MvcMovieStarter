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

        // GET: /Movies/

        public IActionResult Index()
        {
            foreach (var movie in _context.Movies)
            {
                Console.WriteLine($"Movie {movie.Id}, Title: {movie.Title}, Genre {movie.Genre}");
            }
            return View();
        }

        // GET: /Movies/<id>
        [Route("Movies/{id:int}")]
        public IActionResult Show(int id)
        {
            var movie = _context.Movies.Find(id);
            return View(movie);
        }
    }
}
