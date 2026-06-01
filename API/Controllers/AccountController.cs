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
    RoleManager<AppRole> roleManager,
    ITokenService tokenService,
    IPasswordResetService passwordResetService
) : BaseApiController
{
    // ===============================
    // Register
    // Creates a new salon client account.
    // ===============================

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerDto)
    {
        var email = registerDto.Email.Trim().ToLowerInvariant();
        var phoneNumber = registerDto.PhoneNumber.Trim();

        if (await EmailExists(email))
        {
            return BadRequest(new
            {
                email = "This email address is already registered."
            });
        }

        if (await PhoneNumberExists(phoneNumber))
        {
            return BadRequest(new
            {
                phoneNumber = "This phone number is already registered."
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
            PhoneNumber = phoneNumber,
            JoinDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        await EnsureRoleExists("Client");
        await userManager.AddToRoleAsync(user, "Client");

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
    // Forgot password
    // Creates a 6-digit reset code.
    // Later this code can be sent by email or SMS.
    // For development, the code is returned in the response.
    // ===============================

    [HttpPost("forgot-password")]
    public async Task<ActionResult<AuthMessageDto>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await FindUserByEmailOrPhone(forgotPasswordDto.EmailOrPhone);

        if (user == null)
        {
            return NotFound(new
            {
                emailOrPhone = "No account was found with this email address or phone number."
            });
        }

        var response = await passwordResetService.CreateResetCode(user);

        return Ok(response);
    }

    // ===============================
    // Verify reset code
    // Checks whether the reset code exists, is unused, and has not expired.
    // ===============================

    [HttpPost("verify-reset-code")]
    public async Task<ActionResult<AuthMessageDto>> VerifyResetCode(VerifyResetCodeDto verifyResetCodeDto)
    {
        var user = await FindUserByEmailOrPhone(verifyResetCodeDto.EmailOrPhone);

        if (user == null)
        {
            return NotFound(new
            {
                emailOrPhone = "No account was found with this email address or phone number."
            });
        }

        var codeIsValid = await passwordResetService.VerifyResetCode(
            user,
            verifyResetCodeDto.VerificationCode);

        if (!codeIsValid)
        {
            return BadRequest(new
            {
                verificationCode = "The reset code is invalid or has expired."
            });
        }

        return Ok(new AuthMessageDto
        {
            Message = "Verification successful. You can now reset your password."
        });
    }

    // ===============================
    // Reset password
    // Resets the user's password after a valid code is provided.
    // ===============================

    [HttpPost("reset-password")]
    public async Task<ActionResult<AuthMessageDto>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await FindUserByEmailOrPhone(resetPasswordDto.EmailOrPhone);

        if (user == null)
        {
            return NotFound(new
            {
                emailOrPhone = "No account was found with this email address or phone number."
            });
        }

        var passwordResetSuccessful = await passwordResetService.ResetPassword(
            user,
            resetPasswordDto.VerificationCode,
            resetPasswordDto.NewPassword);

        if (!passwordResetSuccessful)
        {
            return BadRequest(new
            {
                verificationCode = "The reset code is invalid, expired, already used, or the password could not be reset."
            });
        }

        return Ok(new AuthMessageDto
        {
            Message = "Password reset successfully. You can now sign in with your new password."
        });
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

    [Authorize]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        return Ok(new AuthMessageDto
        {
            Message = "Logged out successfully. Please clear the token on the client."
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

    private async Task<bool> PhoneNumberExists(string phoneNumber)
    {
        return await userManager.Users
            .AnyAsync(user => user.PhoneNumber == phoneNumber);
    }

    private async Task<AppUser?> FindUserByEmailOrPhone(string emailOrPhone)
    {
        var value = emailOrPhone.Trim().ToLowerInvariant();
        var phoneValue = emailOrPhone.Trim().Replace(" ", string.Empty);

        return await userManager.Users
            .SingleOrDefaultAsync(user =>
                user.NormalizedEmail == value.ToUpperInvariant() ||
                user.PhoneNumber == phoneValue);
    }

    private async Task EnsureRoleExists(string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        await roleManager.CreateAsync(new AppRole
        {
            Name = roleName
        });
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