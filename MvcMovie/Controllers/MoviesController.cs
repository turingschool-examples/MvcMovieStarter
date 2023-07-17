using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        //GET:/movies/
        public IActionResult Index()
        {
            return View();
        }
    }
}
