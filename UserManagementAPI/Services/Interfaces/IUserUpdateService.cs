using UserManagementAPI.Dto;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserUpdateService
    {
        Task<UpdateUserDto> UpdateUser(int id, UpdateUserDto userDto);
    }
}
