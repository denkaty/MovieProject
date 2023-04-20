using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using MovieProject.Services;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;

namespace MovieProject.Controllers
{
    public class DirectorController : Controller
    {
        public DirectorService directorService { get; set; }

        public DirectorController(DirectorService directorService)
        {
            this.directorService = directorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<DirectorViewModel> directors = await directorService.GetAllDirectorsAsync();

            return this.View(directors);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            DirectorViewModel directorViewModel = await directorService.GetDirectoryIdAsync(id);

            if (directorViewModel == null)
            {
                return NotFound();
            }

            return this.View(directorViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DirectorViewModel directorVM)
        {
            //if (ModelState.IsValid)
            //{
            await directorService.CreateDirectorAsync(directorVM);
            TempData["success"] = "Director was created successfully!";
            return RedirectToAction(nameof(Index));
            //}
            //return this.View();


        }

        public async Task<IActionResult> Edit(string id)
        {
            DirectorViewModel director = await directorService.GetDirectoryIdAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return this.View(director);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DirectorViewModel directorVM)
        {
            await directorService.UpdateDirectorAsync(directorVM);
            TempData["success"] = "Director was updated successfully!";
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            DirectorViewModel director = await directorService.GetDirectoryIdAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return this.View(director);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            await directorService.DeleteDirectorByIdAsync(id);
            TempData["success"] = "Director was deleted successfully!";
            return RedirectToAction("index");
        }
    }
}
