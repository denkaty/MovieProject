using Microsoft.AspNetCore.Mvc;
using MovieProject.Services;
using MovieProject.ViewModels;

namespace MovieProject.Controllers
{
    public class GenreController : Controller
    {
        public GenreService genreService { get; set; }

        public GenreController(GenreService genreService)
        {
            this.genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<GenreViewModel> genres = await genreService.GetAllGenresAsync();

            return this.View(genres);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            GenreViewModel genreViewModel = await genreService.GetGenreByIdAsync(id);

            if (genreViewModel == null)
            {
                return NotFound();
            }

            return this.View(genreViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreViewModel genreVM)
        {
            //if (ModelState.IsValid)
            //{
            await genreService.CreateGenreAsync(genreVM);
            TempData["success"] = "Genre was created successfully!";
            return RedirectToAction(nameof(Index));
            //}
            //return this.View();


        }

        public async Task<IActionResult> Update(string id)
        {
            GenreViewModel genre = await genreService.GetGenreByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return this.View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GenreViewModel genreVM)
        {
            await genreService.UpdateGenreAsync(genreVM);
            TempData["success"] = "Genre was updated successfully!";
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            GenreViewModel genre = await genreService.GetGenreByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return this.View(genre);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            await genreService.DeleteGenreByIdAsync(id);
            TempData["success"] = "Genre was deleted successfully!";
            return RedirectToAction("index");
        }
    }
}
