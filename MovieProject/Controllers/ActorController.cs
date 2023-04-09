using Microsoft.AspNetCore.Mvc;

namespace MovieProject.Controllers
{
    public class ActorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
