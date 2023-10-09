using UserManagementAPI.Helpers;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers(UserParameters parameters);
    }
}
