using AutoMapper;
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
            await this.movieDbContext.Actors.AddAsync(actor);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<ActorViewModel>> GetAllActorsAsync()
        {
            List<Actor> actors = await this.movieDbContext.Actors.ToListAsync();
            List<ActorViewModel> actorsViewModels = this.mapper.Map<List<ActorViewModel>>(actors);
            return actorsViewModels;
        }
        public async Task<ActorViewModel> GetActorByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Actor? actor = await this.movieDbContext.Actors.FindAsync(id);
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
       
        

        public async Task<ActorViewModel> UpdateActorByIdAsync(string id, ActorViewModel actorVM)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            if (actorVM == null)
            {
                throw new ArgumentNullException("The ActorViewModel parameter cannot be null.", nameof(actorVM));
            }
            Actor? actorEntity = await this.movieDbContext.Actors.FindAsync(id);
            Actor? actorUpdated = this.mapper.Map(actorVM, actorEntity);

            this.movieDbContext.Actors.Update(actorUpdated);
            await this.movieDbContext.SaveChangesAsync();

            ActorViewModel actorViewModel = this.mapper.Map<ActorViewModel>(actorUpdated);
            return actorViewModel;
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
    }
}
