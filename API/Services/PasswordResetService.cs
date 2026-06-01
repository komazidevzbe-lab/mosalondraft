using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PasswordResetService(
    DataContext context,
    UserManager<AppUser> userManager
) : IPasswordResetService
{
    private const int ResetCodeExpiryMinutes = 15;

    public async Task<AuthMessageDto> CreateResetCode(AppUser user)
    {
        await ExpireOldUnusedCodes(user.Id);

        var plainCode = GenerateSixDigitCode();
        var codeHash = HashCode(plainCode);
        var identityResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        var resetCode = new PasswordResetCode
        {
            UserId = user.Id,
            CodeHash = codeHash,
            IdentityResetToken = identityResetToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(ResetCodeExpiryMinutes),
            IsUsed = false
        };

        context.PasswordResetCodes.Add(resetCode);
        await context.SaveChangesAsync();

        return new AuthMessageDto
        {
            Message = "A password reset code has been created.",
            DevelopmentResetCode = plainCode
        };
    }

    public async Task<bool> VerifyResetCode(AppUser user, string code)
    {
        var resetCode = await GetActiveResetCode(user.Id, code);

        return resetCode != null;
    }

    public async Task<bool> ResetPassword(AppUser user, string code, string newPassword)
    {
        var resetCode = await GetActiveResetCode(user.Id, code);

        if (resetCode == null)
        {
            return false;
        }

        var result = await userManager.ResetPasswordAsync(
            user,
            resetCode.IdentityResetToken,
            newPassword);

        if (!result.Succeeded)
        {
            return false;
        }

        resetCode.IsUsed = true;

        await context.SaveChangesAsync();

        return true;
    }

    private async Task<PasswordResetCode?> GetActiveResetCode(int userId, string code)
    {
        var codeHash = HashCode(code.Trim());

        return await context.PasswordResetCodes
            .Where(resetCode =>
                resetCode.UserId == userId &&
                resetCode.CodeHash == codeHash &&
                !resetCode.IsUsed &&
                resetCode.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(resetCode => resetCode.CreatedAt)
            .FirstOrDefaultAsync();
    }

    private async Task ExpireOldUnusedCodes(int userId)
    {
        var oldCodes = await context.PasswordResetCodes
            .Where(resetCode =>
                resetCode.UserId == userId &&
                !resetCode.IsUsed)
            .ToListAsync();

        foreach (var code in oldCodes)
        {
            code.IsUsed = true;
        }

        await context.SaveChangesAsync();
    }

    private static string GenerateSixDigitCode()
    {
        var number = RandomNumberGenerator.GetInt32(100000, 1000000);

        return number.ToString();
    }

    private static string HashCode(string code)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(code));

        return Convert.ToBase64String(bytes);
    }
}