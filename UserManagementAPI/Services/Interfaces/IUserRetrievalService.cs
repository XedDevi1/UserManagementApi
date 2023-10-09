using UserManagementAPI.Dto;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserRetrievalService
    {
        Task<UserDto> GetUserByIdAsync(int id);
    }
}
