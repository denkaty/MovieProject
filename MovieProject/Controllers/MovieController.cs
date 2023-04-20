using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using MovieProject.Services;

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
        public  IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel movieVM)
        {
            await movieService.CreateMovieAsync(movieVM);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            await movieService.DeleteMovieByIdAsync(id);
            return this.RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            MovieViewModel movie = await this.movieService.UpdateMovieByIdAsync(id);

            return this.View(movie);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(MovieViewModel movieViewModel)
        {
            //if (this.ModelState.IsValid == false)
            //{
            //    return this.View(movieViewModel);
            //}

            await this.movieService.UpdateMovieAsync(movieViewModel);

            return this.Redirect("index");
        }
    }
}
