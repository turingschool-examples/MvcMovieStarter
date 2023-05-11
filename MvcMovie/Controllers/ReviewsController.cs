using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.DataAccess;
using MvcMovie.Models;

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
            var reviews = movie.Reviews;
            
            ViewData["MovieTitle"] = movie.Title;
            ViewData["MovieId"] = movie.Id;
            return View(reviews);
        }

        // GET: /movies/:movieId/reviews/new
        [Route("Movies/{movieId:int}/reviews/new")]
        public IActionResult New(int movieId)
        {
            var movie = _context.Movies
                .Where(m => m.Id == movieId)
                .Include(m => m.Reviews)
                .First();

            ViewData["MovieTitle"] = movie.Title;
            ViewData["MovieId"] = movie.Id;
            return View();
        }

        // POST: /movies/:movieId/reviews
        [HttpPost]
        [Route("/Movies/{movieId:int}/reviews")]
        public IActionResult Create(int movieId, Review review)
        {
            var movie = _context.Movies
                .Where(m => m.Id == movieId)
                .Include(m => m.Reviews)
                .First();
            movie.Reviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("index", new { movieId = movie.Id });
        }
    }
}
