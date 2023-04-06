using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
