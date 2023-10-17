using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Dto;
using UserManagementAPI.Exceptions;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserUpdateService : IUserUpdateService
    {
        private readonly UserManagementDbContext _userManagementDbContext;
        private readonly IMapper _mapper;

        public UserUpdateService(UserManagementDbContext userManagementDbContext, IMapper mapper)
        {
            _userManagementDbContext = userManagementDbContext;
            _mapper = mapper;
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
