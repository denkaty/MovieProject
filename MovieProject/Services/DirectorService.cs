using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;
using System.IO;

namespace MovieProject.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public DirectorService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }

        public async Task CreateDirectorAsync(DirectorViewModel directorVM)
        {
            if (directorVM == null)
            {
                throw new ArgumentNullException("The DirectorViewModel parameter cannot be null.", nameof(directorVM));
            }

            Director director = this.mapper.Map<Director>(directorVM);
            director.DirectorId = director.GenerateDirectorId();
            await this.movieDbContext.Directors.AddAsync(director);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<DirectorViewModel>> GetAllDirectorsAsync()
        {
            List<Director> directors = await this.movieDbContext.Directors.Include(d => d.Movies).ToListAsync();
            List<DirectorViewModel> directorViewModels = this.mapper.Map<List<DirectorViewModel>>(directors);
            return directorViewModels;
        }

        public async Task<DirectorViewModel> GetDirectoryIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }
           
            Director? director = await this.movieDbContext.Directors.Include(d => d.Movies).FirstOrDefaultAsync(d => d.DirectorId ==id);
            if (director == null)
            {
                throw new ArgumentException("No Director was found with the given id.", nameof(id));
            }

            DirectorViewModel directorViewModel = this.mapper.Map<DirectorViewModel>(director);
            return directorViewModel;
        }

        public async Task UpdateDirectorAsync(DirectorViewModel directorVM)
        {
            if (directorVM == null)
            {
                throw new ArgumentNullException("The DirectorViewModel parameter cannot be null.", nameof(directorVM));
            }

            Director director = this.mapper.Map<Director>(directorVM);
            this.movieDbContext.Directors.Update(director);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<DirectorViewModel> UpdateDirectorByIdAsync(string id, DirectorViewModel directorVM)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            if (directorVM == null)
            {
                throw new ArgumentNullException("The DirectorViewModel parameter cannot be null.", nameof(directorVM));
            }

            Director? directorEntity = await this.movieDbContext.Directors.FindAsync(id);
            Director? directorUpdated = this.mapper.Map(directorVM, directorEntity);

            this.movieDbContext.Directors.Update(directorUpdated);
            await this.movieDbContext.SaveChangesAsync();

            DirectorViewModel directorViewModel = this.mapper.Map<DirectorViewModel>(directorUpdated);
            return directorViewModel;
        }
        public async Task DeleteDirectorByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Director? director = await this.movieDbContext.Directors.FindAsync(id);
            if (director == null)
            {
                throw new ArgumentException($"There is no director with the id {id} in the database.", nameof(id));
            }
            this.movieDbContext.Directors.Remove(director);
            await this.movieDbContext.SaveChangesAsync();
        }
    }
}
