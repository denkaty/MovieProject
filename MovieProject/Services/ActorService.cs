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
    public class ActorService : IActorService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public ActorService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }
        public async Task CreateActorAsync(ActorViewModel actorVM)
        {
            if (actorVM == null)
            {
                throw new ArgumentNullException("The ActorViewModel parameter cannot be null.", nameof(actorVM));
            }

            Actor actor = this.mapper.Map<Actor>(actorVM);
            actor.ActorId = Configuration.GenerateId();
            await this.movieDbContext.Actors.AddAsync(actor);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<ActorViewModel>> GetAllActorsAsync()
        {
            List<Actor> actors = await this.movieDbContext.Actors.Include(a => a.MoviesActors).ToListAsync();
            List<ActorViewModel> actorsViewModels = this.mapper.Map<List<ActorViewModel>>(actors);
            return actorsViewModels;
        }
        public async Task<ActorViewModel> GetActorByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Actor? actor = await this.movieDbContext.Actors.Include(a=>a.MoviesActors).ThenInclude(a=> a.Movie).FirstOrDefaultAsync(a=>a.ActorId == id);
            if (actor == null)
            {
                throw new ArgumentException("No Actor was found with the given id.", nameof(id));
            }

            ActorViewModel actorViewModel = this.mapper.Map<ActorViewModel>(actor);
            return actorViewModel;
        }
        public async Task UpdateActorAsync(ActorViewModel actorVM)
        {
            if (actorVM == null)
            {
                throw new ArgumentNullException("The ActorViewModel parameter cannot be null.", nameof(actorVM));
            }
            Actor actor = this.mapper.Map<Actor>(actorVM);
            this.movieDbContext.Actors.Update(actor);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task DeleteActorByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Actor? actor = await this.movieDbContext.Actors.FindAsync(id);
            if(actor == null)
            {
                throw new ArgumentException($"There is no actor with the id {id} in the database.", nameof(id));
            }

            this.movieDbContext.Actors.Remove(actor);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task RemoveActorFromMovie(string movieId, string actorId)
        {
            Movie? movie = await this.movieDbContext.Movies.FindAsync(movieId);
            Actor? actor = await this.movieDbContext.Actors.FindAsync(actorId);

            MovieActor movieActor = new MovieActor
            {
                MovieId = movie.MovieId,
                Movie = movie,
                ActorId = actor.ActorId,
                Actor = actor
            };
            this.movieDbContext.MovieActors.Remove(movieActor);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            IEnumerable<Movie> movies = await this.movieDbContext.Movies.ToListAsync();
            return movies;
        }
        public async Task<bool> CreateNewRole(MovieActorViewModel movieActorViewModel)
        {
            string movieTitle = movieActorViewModel.Movie.Title;
            string actorId = movieActorViewModel.ActorId;

            Movie? movie = await this.movieDbContext.Movies.SingleOrDefaultAsync(m => m.Title == movieActorViewModel.Movie.Title);
            Actor? actor = await this.movieDbContext.Actors.SingleOrDefaultAsync(a => a.ActorId == actorId);

            if(movie == null || actor == null)
            {
                return false;
            }
            MovieViewModel movieViewModel = this.mapper.Map<MovieViewModel>(movie);
            ActorViewModel actorViewModel = this.mapper.Map<ActorViewModel>(actor);

            MovieActor movieActor = new MovieActor
            {
                MovieId = movie.MovieId,
                Movie = movie,
                ActorId = actor.ActorId,
                Actor = actor
            };

            if (this.movieDbContext.MovieActors.Contains(movieActor))
            {
                return false;
            }
            this.movieDbContext.MovieActors.Add(movieActor);
            await this.movieDbContext.SaveChangesAsync();
            return true;
        }
    }
}
