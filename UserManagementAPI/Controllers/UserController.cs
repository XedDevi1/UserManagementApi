using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using UserManagementAPI.Dto;
using UserManagementAPI.Exceptions;
using UserManagementAPI.Helpers;
using UserManagementAPI.Models;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services;
using UserManagementAPI.Services.Interfaces;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRetrievalService _userRetrievalService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserCreationService _userCreationService;
        private readonly IUserUpdateService _userUpdateService;
        private readonly IUserDeletionService _userDeletionService;

        public UserController(UserManagementDbContext userManagementDbContext, IUserService userService, IUserRetrievalService userRetrievalService, IUserRoleService userRoleService, IUserCreationService userCreationService, IUserUpdateService userUpdateService, IUserDeletionService userDeletionService)
        {
            _userService = userService;
            _userRetrievalService = userRetrievalService;
            _userRoleService = userRoleService;
            _userCreationService = userCreationService;
            _userUpdateService = userUpdateService;
            _userDeletionService = userDeletionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParameters parameters)
        {
            var users = await _userService.GetUsers(parameters);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRetrievalService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] UserRoleDto userRole)
        {
            await _userRoleService.AssignRoleToUserAsync(userRole);
            return NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var user = await _userCreationService.CreateUser(userDto);
            return Ok(user);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            var updatedUser = await _userUpdateService.UpdateUser(id, userDto);
            return Ok(updatedUser);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userDeletionService.DeleteUser(id);
            return NoContent();
        }
    }
}