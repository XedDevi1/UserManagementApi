using UserManagementAPI.Dto;
using UserManagementAPI.Helpers;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers(UserParameters parameters);
        Task<CreateUserDto> CreateUser(CreateUserDto userDto);
        Task DeleteUser(int id);
        Task<UserDto> GetUserByIdAsync(int id);
        Task AssignRoleToUserAsync(UserRoleDto userRole);
        Task<UpdateUserDto> UpdateUser(int id, UpdateUserDto userDto);
    }
}
