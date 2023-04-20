using Microsoft.AspNetCore.Mvc;
using MovieProject.Services;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;

namespace MovieProject.Controllers
{
    public class WriterController : Controller
    {
        public WriterService writerService { get; set; }

        public WriterController(WriterService writerService)
        {
            this.writerService = writerService;
        }

        public async Task<IActionResult> Index()
        {
            List<WriterViewModel> writers = await writerService.GetAllWritersAsync();
            return this.View(writers);
        }
        public async Task<IActionResult> Details(string id)
        {
            WriterViewModel writer = await writerService.GetWriterByIdAsync(id);
            return this.View(writer);
        }
    }
}
