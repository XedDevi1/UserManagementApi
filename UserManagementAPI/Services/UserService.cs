using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Dto;
using UserManagementAPI.Exceptions;
using UserManagementAPI.Helpers;
using UserManagementAPI.Models;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManagementDbContext _userManagementDbContext;
        private readonly IMapper _mapper;

        public UserService(UserManagementDbContext userManagementDbContext, IMapper mapper)
        {
            _userManagementDbContext = userManagementDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUsers(UserParameters parameters)
        {
            IQueryable<User> query = _userManagementDbContext.Users;

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
        public async Task<CreateUserDto> CreateUser(CreateUserDto userDto)
        {
            var existingUser = await _userManagementDbContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException("Email уже существует");
            }

            var user = _mapper.Map<User>(userDto);

            _userManagementDbContext.Users.Add(user);
            await _userManagementDbContext.SaveChangesAsync();

            var role = await _userManagementDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (role != null)
            {
                var userRole = new UserRole { UserId = user.Id, RoleId = role.Id };
                _userManagementDbContext.UserRoles.Add(userRole);
                await _userManagementDbContext.SaveChangesAsync();
            }

            return _mapper.Map<CreateUserDto>(user);
        }
        public async Task DeleteUser(int id)
        {
            var user = await _userManagementDbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException("Пользователь не найден");
            }

            _userManagementDbContext.Users.Remove(user);
            await _userManagementDbContext.SaveChangesAsync();
        }
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userManagementDbContext.Users.Include(u => u.UserRoles)
                                            .ThenInclude(ur => ur.Role)
                                            .FirstOrDefaultAsync(u => u.Id == id);
            return user == null ? throw new UserNotFoundException("Пользователь не найден") : _mapper.Map<UserDto>(user);
        }
        public async Task AssignRoleToUserAsync(UserRoleDto userRole)
        {
            var user = await _userManagementDbContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userRole.UserId);

            if (user == null)
            {
                throw new UserNotFoundException("Пользователь не найден");
            }

            foreach (var roleId in userRole.RoleIds)
            {
                var role = await _userManagementDbContext.Roles.FindAsync(roleId);

                if (role == null)
                {
                    throw new UserNotFoundException($"Роль с ID {roleId} не найдена");
                }

                if (user.UserRoles != null && user.UserRoles.Any(ur => ur.RoleId == roleId))
                {
                    throw new BadRequestException($"Пользователь уже имеет роль с ID {roleId}");
                }

                var newUserRole = new UserRole { UserId = userRole.UserId, RoleId = roleId };
                _userManagementDbContext.UserRoles.Add(newUserRole);
            }

            await _userManagementDbContext.SaveChangesAsync();
        }
        public async Task<UpdateUserDto> UpdateUser(int id, UpdateUserDto userDto)
        {
            var existingUser = await _userManagementDbContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email && u.Id != id);
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException("Email уже существует");
            }

            var user = await _userManagementDbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException("Пользователь не найден");
            }

            _mapper.Map(userDto, user);

            await _userManagementDbContext.SaveChangesAsync();

            return _mapper.Map<UpdateUserDto>(user);
        }
    }
}
