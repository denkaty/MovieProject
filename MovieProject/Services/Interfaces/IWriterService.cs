using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IWriterService
    {
        public Task CreateWriterAsync(WriterViewModel writerVM);
        public Task<List<WriterViewModel>> GetAllWritersAsync();
        public Task<WriterViewModel> GetWriterByIdAsync(string id);
        public Task UpdateWriterAsync(WriterViewModel writerVM);
        public Task<WriterViewModel> UpdateWriterByIdAsync(string id, WriterViewModel writerVM);
        public Task DeleteWriterByIdAsync(string id);
    }
}
