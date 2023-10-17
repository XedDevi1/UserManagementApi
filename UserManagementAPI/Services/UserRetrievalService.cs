using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Dto;
using UserManagementAPI.Exceptions;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Services
{
    public class UserRetrievalService : IUserRetrievalService
    {
        private readonly UserManagementDbContext _context;
        private readonly IMapper _mapper;

        public UserRetrievalService(UserManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.Include(u => u.UserRoles)
                                            .ThenInclude(ur => ur.Role)
                                            .FirstOrDefaultAsync(u => u.Id == id);
            return user == null ? throw new UserNotFoundException("Пользователь не найден") : _mapper.Map<UserDto>(user);
        }
    }
}
