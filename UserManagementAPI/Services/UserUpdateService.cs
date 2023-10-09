using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Dto;
using UserManagementAPI.Models;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserUpdateService : IUserUpdateService
    {
        private readonly UserManagementDbContext _userManagementDbContext;

        public UserUpdateService(UserManagementDbContext userManagementDbContext)
        {
            _userManagementDbContext = userManagementDbContext;
        }

        public async Task<UpdateUserDto> UpdateUser(int id, UpdateUserDto userDto)
        {
            var existingUser = await _userManagementDbContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email && u.Id != id);
            if (existingUser != null)
            {
                throw new Exception("Email уже существует");
            }

            var user = await _userManagementDbContext.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            user.Name = userDto.Name;
            user.Age = userDto.Age;
            user.Email = userDto.Email;

            await _userManagementDbContext.SaveChangesAsync();

            var updatedUser = new UpdateUserDto
            {
                Name = user.Name,
                Age = user.Age,
                Email = user.Email
            };

            return updatedUser;
        }
    }
}
