using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService
) : BaseApiController
{
    // ===============================
    // Register
    // Creates a new salon user account.
    // ===============================

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerDto)
    {
        var email = registerDto.Email.Trim().ToLowerInvariant();

        if (await EmailExists(email))
        {
            return BadRequest(new
            {
                email = "This email address is already registered."
            });
        }

        if (!PasswordsMatch(registerDto.Password, registerDto.ConfirmPassword))
        {
            return BadRequest(new
            {
                confirmPassword = "Passwords do not match."
            });
        }

        var user = new AppUser
        {
            FirstName = registerDto.FirstName.Trim(),
            LastName = registerDto.LastName.Trim(),
            UserName = email,
            Email = email,
            PhoneNumber = registerDto.PhoneNumber.Trim(),
            JoinDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return await CreateUserDto(user);
    }

    // ===============================
    // Login
    // Authenticates an existing salon user.
    // Users sign in with email and password.
    // ===============================

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var email = loginDto.Email.Trim().ToLowerInvariant();

        var user = await userManager.Users
            .SingleOrDefaultAsync(user => user.NormalizedEmail == email.ToUpperInvariant());

        if (user == null)
        {
            return Unauthorized(new
            {
                email = "Email address not found."
            });
        }

        var passwordIsValid = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!passwordIsValid)
        {
            return Unauthorized(new
            {
                password = "Invalid password."
            });
        }

        return await CreateUserDto(user);
    }

    // ===============================
    // Current user
    // Returns the logged-in user from the JWT token.
    // Useful later when Angular needs to refresh user state.
    // ===============================

    [Authorize]
    [HttpGet("current-user")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var email = User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized();
        }

        var user = await userManager.Users
            .SingleOrDefaultAsync(user => user.NormalizedEmail == email.ToUpperInvariant());

        if (user == null)
        {
            return Unauthorized();
        }

        return await CreateUserDto(user);
    }

    // ===============================
    // Check email
    // Allows the frontend to check whether an email is already registered.
    // ===============================

    [HttpGet("check-email")]
    public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email address is required.");
        }

        return await EmailExists(email.Trim().ToLowerInvariant());
    }

    // ===============================
    // Logout
    // JWT logout happens on the Angular side by clearing the stored token.
    // ===============================

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        return Ok(new
        {
            message = "Logged out. Please clear the token on the client."
        });
    }

    // ===============================
    // Private helper methods
    // Keeps the controller actions cleaner.
    // ===============================

    private async Task<bool> EmailExists(string email)
    {
        return await userManager.Users
            .AnyAsync(user => user.NormalizedEmail == email.ToUpperInvariant());
    }

    private async Task<UserDto> CreateUserDto(AppUser user)
    {
        var roles = await userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Token = await tokenService.CreateToken(user),
            Roles = roles.ToArray()
        };
    }

    private static bool PasswordsMatch(string password, string confirmPassword)
    {
        return password == confirmPassword;
    }
}