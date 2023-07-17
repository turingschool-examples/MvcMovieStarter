﻿using Microsoft.AspNetCore.Mvc;
using MvcMovie.DataAccess;
using MvcMovie.Models;

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

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Movie movie)
        {
            //Take the movie sent in the request and save it to the database
            _context.Movies.Add(movie);
            _context.SaveChanges();

            // The id generated by the database is now on the object we added to the context
            var newMovieId = movie.Id;

            // Redirect to our route /movies/show and pass in the newMovieId for the id parameter
            return RedirectToAction("show", new { id = newMovieId });
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