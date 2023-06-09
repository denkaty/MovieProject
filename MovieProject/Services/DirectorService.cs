﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Models;
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
            Director director = this.mapper.Map<Director>(directorVM);
            director.DirectorId = Configuration.GenerateId();
            await this.movieDbContext.Directors.AddAsync(director);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task<List<DirectorViewModel>> GetAllDirectorsAsync()
        {
            List<Director> directors = await this.movieDbContext
                .Directors
                .Include(d => d.Movies)
                .ToListAsync();

            List<DirectorViewModel> directorViewModels = this.mapper.Map<List<DirectorViewModel>>(directors);
            return directorViewModels;
        }
        public async Task<DirectorViewModel> GetDirectoryIdAsync(string id)
        {
            Director? director = await this.movieDbContext
                .Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.DirectorId ==id);

            DirectorViewModel directorViewModel = this.mapper.Map<DirectorViewModel>(director);
            return directorViewModel;
        }
        public async Task UpdateDirectorAsync(DirectorViewModel directorVM)
        {
            Director director = this.mapper.Map<Director>(directorVM);
            this.movieDbContext.Directors.Update(director);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task DeleteDirectorByIdAsync(string id)
        {
            Director? director = await this.movieDbContext.Directors.FindAsync(id);
            this.movieDbContext.Directors.Remove(director);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task RemoveDirectorFromMovieAsync(string movieId)
        {
            Movie? movie = await this.movieDbContext.Movies.FindAsync(movieId);
            movie.Director = null;
            movie.DirectorId = null;
            this.movieDbContext.Movies.Update(movie);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task<List<DirectorViewModel>> SearchByNameAsync(string name)
        {
            List<Director> directors = await this.movieDbContext
                .Directors
                .OrderByDescending(d => d.Movies.Count())
                .Where(x => (x.FirstName.ToLower().TrimStart() + " " + x.LastName.ToLower().TrimEnd())
                    .Contains(name.ToLower().Trim()))
                .Include(d=>d.Movies)
                .ToListAsync();

            List<DirectorViewModel> directorViewModels = this.mapper.Map<List<DirectorViewModel>>(directors);
            return directorViewModels;
        }
        public async Task<List<DirectorViewModel>> GetDirectorsToShowAsync(int? page)
        {
            int directorsPerPage = 21;
            int startIndex = (int)((page - 1) * directorsPerPage);

            List<Director> directors = await this.movieDbContext
                .Directors
                .OrderByDescending(d => d.Movies.Count())
                .Skip(startIndex)
                .Include(d => d.Movies)
                .Take(directorsPerPage)
                .ToListAsync();

            List<DirectorViewModel> directorViewModels = this.mapper.Map<List<DirectorViewModel>>(directors);
            return directorViewModels;
        }
        public int GetDirectorsCount()
        {
            int count = this.movieDbContext.Directors.Count();
            return count;
        }

    }
}
