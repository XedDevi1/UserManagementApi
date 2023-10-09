using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Helpers;
using UserManagementAPI.Models;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManagementDbContext _context;

        public UserService(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers(UserParameters parameters)
        {
            IQueryable<User> query = _context.Users;

            // Filtering
            if (!string.IsNullOrEmpty(parameters.FilterName))
                query = query.Where(u => u.Name.Contains(parameters.FilterName));
            if (parameters.FilterAge.HasValue)
                query = query.Where(u => u.Age == parameters.FilterAge.Value);
            if (!string.IsNullOrEmpty(parameters.FilterEmail))
                query = query.Where(u => u.Email.Contains(parameters.FilterEmail));

            // Sorting
            if (parameters.SortDescending)
                query = query.OrderByDescending(u => EF.Property<object>(u, parameters.SortBy));
            else
                query = query.OrderBy(u => EF.Property<object>(u, parameters.SortBy));

            // Pagination
            query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);

            return await query.ToListAsync();
        }
    }
}
