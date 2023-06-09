﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;
using MovieProject.Services;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using System.Data;

namespace MovieProject.Controllers
{
    [Authorize]
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
            List<DirectorViewModel> directors = await directorService.GetDirectorsToShowAsync(1);
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = (int)Math.Ceiling((double)directorService.GetDirectorsCount() / 21);

            return this.View(directors);
        }
        [HttpGet]
        public async Task<IActionResult> Page(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            List<DirectorViewModel> directors = await directorService.GetDirectorsToShowAsync(page);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)directorService.GetDirectorsCount() / 21);
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
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(DirectorViewModel directorVM)
        {
            await directorService.CreateDirectorAsync(directorVM);
            TempData["success"] = "Director was created successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(string id)
        {
            DirectorViewModel director = await directorService.GetDirectoryIdAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return this.View(director);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(DirectorViewModel directorVM)
        {
            await directorService.UpdateDirectorAsync(directorVM);
            TempData["success"] = "Director was updated successfully!";
            return RedirectToAction("index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePOST(string id)
        {
            await directorService.DeleteDirectorByIdAsync(id);
            TempData["success"] = "Director was deleted successfully!";
            return RedirectToAction("index");
        }

        
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveDirectorFromMovie(string movieId, string directorId)
        {
            await this.directorService.RemoveDirectorFromMovieAsync(movieId);
            TempData["success"] = "Director was removed successfully!";
            return RedirectToAction("Details", "Director", new { id = directorId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string name)
        {
            if (name == null)
            {
                return RedirectToAction("Index");
            }
            List<DirectorViewModel> searchResults = await directorService.SearchByNameAsync(name);
            return this.View(searchResults);
        }
    }
}
