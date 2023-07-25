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

            return View(movie);
        }

        // GET: /movies/:movieId/reviews/new
        [Route("Movies/{movieId:int}/reviews/new")]
        public IActionResult New(int movieId)
        {
            var movie = _context.Movies
                .Where(m => m.Id == movieId)
                .Include(m => m.Reviews)
                .First();

            return View(movie);
        }

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

		[Route("Movies/{movieId:int}/reviews/{reviewId:int}/edit")]
		public IActionResult Edit(int reviewId)
		{
            var review = _context.Reviews
                .Where(r => r.Id == reviewId)
                .Include(r => r.Movie)
                .First();

			return View(review);
		}

		[HttpPut]
		[Route("Movies/{movieId:int}/reviews")]
		public IActionResult Update(int id, Review review)
		{
			review.Id = id;
			_context.Reviews.Update(review);
			_context.SaveChanges();

			return RedirectToAction("index", new { id = review.Id });
		}

	}
}
