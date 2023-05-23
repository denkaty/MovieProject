using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using MovieProject.Services;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using System.Data;

namespace MovieProject.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(GenreViewModel genreVM)
        {
            await genreService.CreateGenreAsync(genreVM);
            TempData["success"] = "Genre was created successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(GenreViewModel genreVM)
        {
            await genreService.UpdateGenreAsync(genreVM);
            TempData["success"] = "Genre was updated successfully!";
            return RedirectToAction("index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            await genreService.DeleteGenreByIdAsync(id);
            TempData["success"] = "Genre was deleted successfully!";
            return RedirectToAction("index");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveGenreFromMovie(string movieId, string genreId)
        {
            await this.genreService.RemoveGenreFromMovie(movieId, genreId);
            TempData["success"] = "Genre was removed successfully!";
            return RedirectToAction("Details", "Genre", new { id = genreId });
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ManageNewMovieGenre(string genreId)
        {
            GenreViewModel genreViewModel = await this.genreService.GetGenreByIdAsync(genreId);
            MovieViewModel movieViewModel = new MovieViewModel();
            MovieGenreViewModel movieGenreViewModel = new MovieGenreViewModel
            {
                GenreId = genreId,
                Genre = genreViewModel,
                Movie = movieViewModel
            };
            ViewBag.ExistingMovies = await this.genreService.GetMoviesAsync();
            return this.View(movieGenreViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ManageNewMovieGenre(MovieGenreViewModel movieGenreViewModel)
        {
            if (!await genreService.ManageNewMovieGenre(movieGenreViewModel))
            {
                TempData["error"] = "Genre was not set successfully!";
                return RedirectToAction("ManageNewMovieGenre", new { genreId = movieGenreViewModel.GenreId });
            }
            TempData["success"] = "Genre was set successfully!";
            return RedirectToAction("Details", "Genre", new { id = movieGenreViewModel.GenreId });

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string genre)
        {
            if (genre == null)
            {
                return RedirectToAction("Index");
            }
            List<GenreViewModel> searchResults = await genreService.SearchByName(genre);
            return this.View(searchResults);
        }
    }
}
