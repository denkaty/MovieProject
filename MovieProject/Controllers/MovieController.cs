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
using System.Text.RegularExpressions;

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
            List<MovieViewModel> movies = await movieService.GetMoviesToShowAsync(1);
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = (int)Math.Ceiling((double)movieService.GetMoviesCount() / 21);
            ViewBag.FetchedStatus = await movieService.GetAPIFetchedStatusAsync();
            return this.View(movies);
        }
        [HttpGet]
        public async Task<IActionResult> Page(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            List<MovieViewModel> movies = await movieService.GetMoviesToShowAsync(page);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)movieService.GetMoviesCount() / 21);
            ViewBag.FetchedStatus = await movieService.GetAPIFetchedStatusAsync();
            return this.View(movies);
        }
        [HttpGet]

        public async Task<IActionResult> FetchMovie()
        {
            await this.movieService.FetchMoviesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> ClearData()
        {
            await this.movieService.ClearDataAsync();
            return RedirectToAction("Index");

        }




        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            MovieViewModel movieViewModel = await movieService.GetMovieByIdAsync(id);

            if (movieViewModel == null)
            {
                return NotFound();
            }

            return this.View(movieViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Director> existingDirectors = await movieService.GetExistingDirectorsAsync();
            ViewBag.Directors = existingDirectors;
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CreateMovieViewModel movieVM)
        {
            CheckFormFieldsFormat(movieVM);
            if (!ModelState.IsValid)
            {
                return View();
            }
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
        public async Task<IActionResult> Update(UpdateMovieViewModel movieVM)
        {
            CheckFormFieldsFormat(movieVM);
            if (!ModelState.IsValid)
            {
                return View();
            }
            await this.movieService.UpdateMovieAsync(movieVM);
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
            if (title == null)
            {
                return RedirectToAction("Index");
            }
            List<MovieViewModel> searchResults = await movieService.SearchByTitleAsync(title);
            ViewBag.SearchTitle = title;
            ViewBag.FetchedStatus = await movieService.GetAPIFetchedStatusAsync();
            return this.View(searchResults);
        }
        private void CheckFormFieldsFormat(CreateMovieViewModel movieVM)
        {
            if (!Configuration.IsValidYearFormat(movieVM.Year.ToString()))
            {
                ModelState.AddModelError("Year", "Invalid input. Please enter the year in the format 'YYYY'.");
            }
            if (!Configuration.IsValidReleasedFormat(movieVM.Released))
            {
                ModelState.AddModelError("Released", "Invalid input. Please enter the release date in the format 'YYYY-MM-DD'.");
            }
            if (!Configuration.IsValidDirectorFullNameFormat(movieVM.DirectorFullName))
            {
                ModelState.AddModelError("DirectorFullName", "Invalid input. Please enter the director's full name in the format 'FirstName LastName'.");
            }
            if (!Configuration.IsValidGenresFormat(movieVM.Genres))
            {
                ModelState.AddModelError("Genres", "Invalid input. Please enter the genres separated by commas (e.g., Adventure, Animation, Science Fiction).");
            }
        }
        private void CheckFormFieldsFormat(UpdateMovieViewModel movieVM)
        {
            if (!Configuration.IsValidYearFormat(movieVM.Year.ToString()))
            {
                ModelState.AddModelError("Year", "Invalid input. Please enter the year in the format 'YYYY'.");
            }
            if (!Configuration.IsValidReleasedFormat(movieVM.Released))
            {
                ModelState.AddModelError("Released", "Invalid input. Please enter the release date in the format 'YYYY-MM-DD'.");
            }
            if (!Configuration.IsValidDirectorFullNameFormat(movieVM.DirectorFullName))
            {
                ModelState.AddModelError("DirectorFullName", "Invalid input. Please enter the director's full name in the format 'FirstName LastName'.");
            }
            if (!Configuration.IsValidGenresFormat(movieVM.Genres))
            {
                ModelState.AddModelError("Genres", "Invalid input. Please enter the genres separated by commas (e.g., Adventure, Animation, Science Fiction).");
            }
        }

    }
}
