using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.Services;
using MovieProject.Services.ApiClient;
using MovieProject.Services.ApiClient.ViewModels.ImportViewModel;
using MovieProject.ViewModels;
using System.Data;

namespace MovieProject.Controllers
{
    [Authorize]
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
            ViewBag.FetchedStatus = await movieService.GetAPIFetchedStatus();
            return this.View(movies);
        }
        [HttpGet]

        public async Task<IActionResult> FetchMovie()
        {
            await this.movieService.FetchMovies();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> ClearData()
        {
            await this.movieService.ClearData();
            return RedirectToAction("Index");

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
            IEnumerable<Director> existingDirectors = await movieService.GetExistingDirectors();
            ViewBag.Directors = existingDirectors;
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string title)
        {
            if(title == null)
            {
                return RedirectToAction("Index");
            }
            List<MovieViewModel> searchResults = await movieService.SearchByTitle(title);
            ViewBag.SearchTitle = title;
            return this.View(searchResults);
        }


    }
}
