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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] UserParameters parameters)
        {
            try
            {
                return Ok(await _userService.GetUsers(parameters));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRetrievalService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] UserRoleDto userRole)
        {
            try
            {
                await _userRoleService.AssignRoleToUserAsync(userRole);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                var user = await _userCreationService.CreateUser(userDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<UpdateUserDto>> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                var updatedUser = await _userUpdateService.UpdateUser(id, userDto);

                if (updatedUser == null)
                {
                    return NotFound("Пользователь не найден");
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRetrievalService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Пользователь не найден");
                }

                await _userDeletionService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}