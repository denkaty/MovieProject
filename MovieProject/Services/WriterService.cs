using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;

namespace MovieProject.Services
{
    public class WriterService : IWriterService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public WriterService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }

        public async Task CreateWriterAsync(WriterViewModel writerVM)
        {
            if (writerVM == null)
            {
                throw new ArgumentNullException("The WriterViewModel parameter cannot be null.", nameof(writerVM));
            }

            Writer writer = this.mapper.Map<Writer>(writerVM);
            await this.movieDbContext.Writers.AddAsync(writer);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<List<WriterViewModel>> GetAllWritersAsync()
        {
            List<Writer> writers = await this.movieDbContext.Writers.ToListAsync();
            List<WriterViewModel> writersViewModels = this.mapper.Map<List<WriterViewModel>>(writers);
            return writersViewModels;
        }
        public async Task<WriterViewModel> GetWriterByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Writer? writer = await this.movieDbContext.Writers.FindAsync(id);
            if (writer == null)
            {
                throw new ArgumentException("No Writer was found with the given id.", nameof(id));
            }

            WriterViewModel writerViewModel = this.mapper.Map<WriterViewModel>(writer);
            return writerViewModel;
        }
        public async Task UpdateWriterAsync(WriterViewModel writerVM)
        {
            if (writerVM == null)
            {
                throw new ArgumentNullException("The WriterViewModel parameter cannot be null.", nameof(writerVM));
            }
            Writer writer = this.mapper.Map<Writer>(writerVM);
            this.movieDbContext.Writers.Update(writer);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<WriterViewModel> UpdateWriterByIdAsync(string id, WriterViewModel writerVM)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            if (writerVM == null)
            {
                throw new ArgumentNullException("The WriterViewModel parameter cannot be null.", nameof(writerVM));
            }
            Writer? writerEntity = await this.movieDbContext.Writers.FindAsync(id);
            Writer? writerUpdated = this.mapper.Map(writerVM, writerEntity);

            this.movieDbContext.Writers.Update(writerUpdated);
            await this.movieDbContext.SaveChangesAsync();

            WriterViewModel writerViewModel = this.mapper.Map<WriterViewModel>(writerUpdated);
            return writerViewModel;
        }

        public async Task DeleteWriterByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            Writer? writer = await this.movieDbContext.Writers.FindAsync(id);
            if (writer == null)
            {
                throw new ArgumentException($"There is no writer with the id {id} in the database.", nameof(id));
            }

            this.movieDbContext.Writers.Remove(writer);
            await this.movieDbContext.SaveChangesAsync();
        }
    }
}
