using UserManagementAPI.Dto;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserCreationService
    {
        Task<CreateUserDto> CreateUser(CreateUserDto userDto);
    }
}
