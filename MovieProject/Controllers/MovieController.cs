using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using MovieProject.Services;
using MovieProject.ViewModels;
using System.Data;

namespace MovieProject.Controllers
{
    public class MovieController : Controller
    {
        public MovieService movieService { get; set; }

        public MovieController(MovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<MovieViewModel> movies = await movieService.GetAllMoviesAsync();
            return this.View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            MovieViewModel movieViewModel = await movieService.GetMovieByIdAsync(id);

            if(movieViewModel == null)
            {
                return NotFound();
            }

            return this.View(movieViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(MovieViewModel movieVM)
        {
            await movieService.CreateMovieAsync(movieVM);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(string id)
        {
            MovieViewModel movieVM = await this.movieService.GetMovieByIdAsync(id);

            return this.View(movieVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(MovieViewModel movieViewModel)
        {
            await this.movieService.UpdateMovieAsync(movieViewModel);
            TempData["success"] = "Movie was updated successfully!";
            return RedirectToAction("index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            MovieViewModel movieViewModel = await this.movieService.GetMovieByIdAsync(id);
            return this.View(movieViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            await this.movieService.DeleteMovieByIdAsync(id);
            TempData["success"] = "Movie was deleted successfully!";
            return RedirectToAction("index");
        }
    }
}
