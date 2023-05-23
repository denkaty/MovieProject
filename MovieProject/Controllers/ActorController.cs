using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.Services;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using System.Data;

namespace MovieProject.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(ActorViewModel actorViewModel)
        {
            await actorService.CreateActorAsync(actorViewModel);
            TempData["success"] = "Actor was created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(ActorViewModel actorViewModel)
        {
            await this.actorService.UpdateActorAsync(actorViewModel);
            TempData["success"] = "Actor was updated successfully!";
            return RedirectToAction("index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            await this.actorService.DeleteActorByIdAsync(id);
            TempData["success"] = "Actor was deleted successfully!";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveActorFromMovie(string movieId, string actorId)
        {
            await this.actorService.RemoveActorFromMovie(movieId, actorId);
            TempData["success"] = "Role was removed successfully!";
            return RedirectToAction("Details", "Actor", new { id = actorId });
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateNewRole(string actorId)
        {
            ActorViewModel actorViewModel = await this.actorService.GetActorByIdAsync(actorId);
            MovieViewModel movieViewModel = new MovieViewModel();
            MovieActorViewModel movieActorViewModel = new MovieActorViewModel
            {
                ActorId = actorId,
                Actor = actorViewModel,
                Movie = movieViewModel
            };
            ViewBag.ExistingMovies = await this.actorService.GetMoviesAsync();
            return this.View(movieActorViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateNewRole(MovieActorViewModel movieActorViewModel)
        {
            if (!await actorService.CreateNewRole(movieActorViewModel))
            {
                TempData["error"] = "Role was not created successfully!";
                return RedirectToAction("CreateNewRole", new { actorId = movieActorViewModel.ActorId });
            }
            TempData["success"] = "Role was created successfully!";
            return RedirectToAction("Details", "Actor", new { id = movieActorViewModel.ActorId });

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string name)
        {
            if (name == null)
            {
                return RedirectToAction("Index");
            }
            List<ActorViewModel> searchResults = await actorService.SearchByName(name);
            return this.View(searchResults);
        }



    }
}
