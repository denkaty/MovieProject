﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Models;
using MovieProject.Services.Interfaces;
using System.IO;

namespace MovieProject.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public MovieService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }

        public async Task CreateMovieAsync(MovieViewModel movieVM)
        {
            if (movieVM == null)
            {
                throw new ArgumentNullException("The MovieViewModel parameter cannot be null.", nameof(movieVM));
            }

            Movie movie = this.mapper.Map<Movie>(movieVM);
            await this.movieDbContext.Movies.AddAsync(movie);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<MovieViewModel>> GetAllMoviesAsync()
        {
            List<Movie> movies = await this.movieDbContext.Movies.Include(m => m.Director).ToListAsync();
            List<MovieViewModel> movieViewModels = this.mapper.Map<List<MovieViewModel>>(movies);
            return movieViewModels;
        }

        public async Task<MovieViewModel> GetMovieByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Movie? movie = await this.movieDbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new ArgumentException("No Movie was found with the given id.", nameof(id));
            }
            MovieViewModel movieViewModel = this.mapper.Map<MovieViewModel>(movie);
            return movieViewModel;
        }

        public async Task UpdateMovieAsync(MovieViewModel movieVM)
        {
            if (movieVM == null)
            {
                throw new ArgumentNullException("The MovieViewModel parameter cannot be null.", nameof(movieVM));
            }

            Movie movie = this.mapper.Map<Movie>(movieVM);
            this.movieDbContext.Movies.Update(movie);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<MovieViewModel> UpdateMovieByIdAsync(string id)
        {
            Movie? movie = await movieDbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new ArgumentException($"There is no movie with the id {id} in the database.", nameof(id));
            }

            MovieViewModel movieViewModel = mapper.Map<MovieViewModel>(movie);

            return movieViewModel;

        }
        public async Task DeleteMovieByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Movie? movie = await this.movieDbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new ArgumentException($"There is no movie with the id {id} in the database.", nameof(id));
            }

            this.movieDbContext.Movies.Remove(movie);
            await movieDbContext.SaveChangesAsync();
        }
    }
}
