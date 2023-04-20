using Microsoft.AspNetCore.Mvc;
using MovieProject.Services;
using MovieProject.ViewModels;

namespace MovieProject.Controllers
{
    public class ActorController : Controller
    {
        public ActorService actorService { get; set; }

        public ActorController(ActorService actorService)
        {
            this.actorService = actorService;
        }

        public async Task<IActionResult> Index()
        {
            List<ActorViewModel> actors = await actorService.GetAllActorsAsync();
            return this.View(actors);
        }

        public async Task<IActionResult> Details(string id)
        {
            ActorViewModel actor = await actorService.GetActorByIdAsync(id);
            return this.View(actor);
        }
    }
}
