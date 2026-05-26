using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UsersController(
    UserManager<AppUser> userManager
) : BaseApiController
{
    // ===============================
    // Get all registered users
    // Returns all salon users currently stored in the database.
    // This is useful for admin/user-management testing in Postman.
    // ===============================

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await userManager.Users
            .AsNoTracking()
            .ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);

            userDtos.Add(new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Token = string.Empty,
                Roles = roles.ToArray()
            });
        }

        return Ok(userDtos);
    }

    // ===============================
    // Get user by id
    // Returns one registered salon user by database id.
    // ===============================

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            return NotFound(new
            {
                message = "User not found."
            });
        }

        var roles = await userManager.GetRolesAsync(user);

        return Ok(new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Token = string.Empty,
            Roles = roles.ToArray()
        });
    }
}