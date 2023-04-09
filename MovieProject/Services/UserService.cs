using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieProject.Data;
using MovieProject.Data.Entities;
using MovieProject.Services.Interfaces;
using MovieProject.ViewModels;

namespace MovieProject.Services
{
    public class UserService : IUserService
    {
        private readonly MovieDbContext movieDbContext;
        private readonly IMapper mapper;

        public UserService(MovieDbContext movieDbContext, IMapper mapper)
        {
            this.movieDbContext = movieDbContext;
            this.mapper = mapper;
        }

        public async Task CreateUserAsync(UserViewModel userVM)
        {
            if (userVM == null)
            {
                throw new ArgumentNullException("The UserViewModel parameter cannot be null.", nameof(userVM));
            }

            User user = this.mapper.Map<User>(userVM);
            await this.movieDbContext.Users.AddAsync(user);
            await this.movieDbContext.SaveChangesAsync();
        }
        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            List<User> users = await this.movieDbContext.Users.ToListAsync();
            List<UserViewModel> usersViewModels = this.mapper.Map<List<UserViewModel>>(users);
            return usersViewModels;
        }

        public async Task<UserViewModel> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            User? user = await this.movieDbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentException("No User was found with the given id.", nameof(id));
            }

            UserViewModel userViewModel = this.mapper.Map<UserViewModel>(user);
            return userViewModel;
        }

        public async Task UpdateUserAsync(UserViewModel userVM)
        {
            if (userVM == null)
            {
                throw new ArgumentNullException("The UserViewModel parameter cannot be null.", nameof(userVM));
            }
            User user = this.mapper.Map<User>(userVM);
            this.movieDbContext.Users.Update(user);
            await this.movieDbContext.SaveChangesAsync();
        }

        public async Task<UserViewModel> UpdateUserByIdAsync(string id, UserViewModel userVM)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            if (userVM == null)
            {
                throw new ArgumentNullException("The UserViewModel parameter cannot be null.", nameof(userVM));
            }
            User? userEntity = await this.movieDbContext.Users.FindAsync(id);
            User? userUpdated = this.mapper.Map(userVM, userEntity);

            this.movieDbContext.Users.Update(userUpdated);
            await this.movieDbContext.SaveChangesAsync();

            UserViewModel userViewModel = this.mapper.Map<UserViewModel>(userUpdated);
            return userViewModel;
        }
        public async Task DeleteUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("The id parameter cannot be null or empty.", nameof(id));
            }

            User? user = await this.movieDbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentException($"There is no user with the id {id} in the database.", nameof(id));
            }

            this.movieDbContext.Users.Remove(user);
            await this.movieDbContext.SaveChangesAsync();
        }
    }
}
