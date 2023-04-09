using MovieProject.ViewModels;

namespace MovieProject.Services.Interfaces
{
    public interface IUserService
    {
        public Task CreateUserAsync(UserViewModel userVM);
        public Task<List<UserViewModel>> GetAllUsersAsync();
        public Task<UserViewModel> GetUserByIdAsync(string id);
        public Task UpdateUserAsync(UserViewModel userVM);
        public Task<UserViewModel> UpdateUserByIdAsync(string id, UserViewModel userVM);
        public Task DeleteUserByIdAsync(string id);
    }
}
