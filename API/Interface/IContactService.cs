using System.Security.Claims;
using API.DTOs;

namespace API.Interfaces;

public interface IContactService
{
    Task<ContactMessageDto> CreateContactMessageAsync(
        CreateContactMessageDto createContactMessageDto,
        ClaimsPrincipal user);

    Task<List<ContactMessageDto>> GetMyContactMessagesAsync(ClaimsPrincipal user);

    Task<ClientReviewDto> CreateClientReviewAsync(
        CreateClientReviewDto createClientReviewDto,
        ClaimsPrincipal user);
}