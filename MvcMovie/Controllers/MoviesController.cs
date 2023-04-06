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
            foreach (var movie in _context.Movies)
            {
                Console.WriteLine($"Movie {movie.Id}, Title: {movie.Title}, Genre {movie.Genre}");
            }
            return View();
        }
    }
}