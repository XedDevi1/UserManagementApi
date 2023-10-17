using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Dto;
using UserManagementAPI.Exceptions;
using UserManagementAPI.Models;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserCreationService : IUserCreationService
    {
        private readonly UserManagementDbContext _userManagementDbContext;
        private readonly IMapper _mapper;

        public UserCreationService(UserManagementDbContext userManagementDbContext, IMapper mapper)
        {
            _userManagementDbContext = userManagementDbContext;
            _mapper = mapper;
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
    }
}