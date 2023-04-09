using Microsoft.AspNetCore.Mvc;

namespace MovieProject.Controllers
{
    public class WriterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
