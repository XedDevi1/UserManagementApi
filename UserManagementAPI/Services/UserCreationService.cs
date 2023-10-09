using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Dto;
using UserManagementAPI.Models;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserCreationService : IUserCreationService
    {
        private readonly UserManagementDbContext _userManagementDbContext;

        public UserCreationService(UserManagementDbContext userManagementDbContext)
        {
            _userManagementDbContext = userManagementDbContext;
        }

        public async Task<User> CreateUser(CreateUserDto userDto)
        {
            var existingUser = await _userManagementDbContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                throw new Exception("Email уже существует");
            }

            var user = new User
            {
                Name = userDto.Name,
                Age = userDto.Age,
                Email = userDto.Email
            };

            _userManagementDbContext.Users.Add(user);
            await _userManagementDbContext.SaveChangesAsync();

            var role = await _userManagementDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (role != null)
            {
                var userRole = new UserRole { UserId = user.Id, RoleId = role.Id };
                _userManagementDbContext.UserRoles.Add(userRole);
                await _userManagementDbContext.SaveChangesAsync();
            }

            return user;
        }
    }
}
