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

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActorViewModel actorViewModel)
        {
            //if (ModelState.IsValid)
            //{
            await actorService.CreateActorAsync(actorViewModel);
            TempData["success"] = "Actor was created successfully!";
            return RedirectToAction(nameof(Index));
            //}
            //return this.View();


        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            ActorViewModel actorViewModel = await this.actorService.GetActorByIdAsync(id);
            if(actorViewModel == null)
            {
                return NotFound();
            }
            return this.View(actorViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ActorViewModel actorViewModel)
        {
            await this.actorService.UpdateActorAsync(actorViewModel);
            TempData["success"] = "Actor was updated successfully!";
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            ActorViewModel actorViewModel = await this.actorService.GetActorByIdAsync(id);
            if (actorViewModel == null)
            {
                return NotFound();
            }
            return this.View(actorViewModel);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            await this.actorService.DeleteActorByIdAsync(id);
            TempData["success"] = "Actor was deleted successfully!";
            return RedirectToAction("index");
        }
    }
}
