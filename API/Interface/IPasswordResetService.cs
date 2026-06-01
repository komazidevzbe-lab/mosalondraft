using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IPasswordResetService
{
    Task<AuthMessageDto> CreateResetCode(AppUser user);

    Task<bool> VerifyResetCode(AppUser user, string code);

    Task<bool> ResetPassword(AppUser user, string code, string newPassword);
}