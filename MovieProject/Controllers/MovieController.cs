﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Details(string id)
        {
            MovieViewModel movieViewModel = await movieService.GetMovieByIdAsync(id);

            if(movieViewModel == null)
            {
                return NotFound();
            }

            return this.View(movieViewModel);
        }
    }
}
