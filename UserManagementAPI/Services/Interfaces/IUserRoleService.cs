using UserManagementAPI.Dto;

namespace UserManagementAPI.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task AssignRoleToUserAsync(UserRoleDto userRole);
    }
}
