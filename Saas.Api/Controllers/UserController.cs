using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Api.Helpers;
using Saas.Api.Model;
using Saas.Entities;
using Saas.Repository.Interfaces;

namespace Saas.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserRepository _userRepository,
    IMapper _mapper) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var userId = GetCurrentUserId();


        var user = await _userRepository.GetUserByUserIdAsync(userId);

        if (user == null)
        {
            return NotFound(Constant.UserNotFound);
        }
        var userToSend = _mapper.Map<UserModel>(user);
        return Ok(userToSend);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
    {
        var userId = GetCurrentUserId();


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<User>(userModel);
        user.UserId = userId;
        await _userRepository.AddAsync(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserModel userModel)
    {
        if (id != userModel.UserId)
        {
            return BadRequest();
        }

        try
        {
            var user = _mapper.Map<User>(userModel);
            await _userRepository.UpdateAsync(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exception
            return StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(Constant.UserNotFound);
        }

        await _userRepository.DeleteAsync(user);

        return NoContent();
    }
}
