using Microsoft.AspNetCore.Mvc;

namespace MovieProject.Controllers
{
    public class DirectorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
