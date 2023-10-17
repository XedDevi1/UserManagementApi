using UserManagementAPI.Exceptions;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserDeletionService : IUserDeletionService
    {
        private readonly UserManagementDbContext _context;

        public UserDeletionService(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException("Пользователь не найден");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
