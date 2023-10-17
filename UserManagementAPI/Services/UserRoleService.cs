using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Dto;
using UserManagementAPI.Exceptions;
using UserManagementAPI.Models;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManagementDbContext _context;

        public UserRoleService(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task AssignRoleToUserAsync(UserRoleDto userRoles)
        {
            var user = await _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userRoles.UserId);

            if (user == null)
            {
                throw new UserNotFoundException("Пользователь не найден");
            }

            foreach (var roleId in userRoles.RoleIds)
            {
                var role = await _context.Roles.FindAsync(roleId);

                if (role == null)
                {
                    throw new UserNotFoundException($"Роль с ID {roleId} не найдена");
                }

                if (user.UserRoles != null && user.UserRoles.Any(ur => ur.RoleId == roleId))
                {
                    throw new BadRequestException($"Пользователь уже имеет роль с ID {roleId}");
                }

                var newUserRole = new UserRole { UserId = userRoles.UserId, RoleId = roleId };
                _context.UserRoles.Add(newUserRole);
            }

            await _context.SaveChangesAsync();
        }
    }
}
