using Microsoft.AspNetCore.Mvc;

namespace MovieProject.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
