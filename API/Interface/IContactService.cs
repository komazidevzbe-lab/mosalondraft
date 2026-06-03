using System.Security.Claims;
using API.DTOs;

namespace API.Interfaces;

public interface IContactService
{
    Task<ContactMessageDto> CreateContactMessageAsync(
        CreateContactMessageDto createContactMessageDto,
        ClaimsPrincipal user);

    Task<List<ContactMessageDto>> GetMyContactMessagesAsync(ClaimsPrincipal user);

    Task<List<ContactMessageDto>> GetAllContactMessagesAsync();

    Task<ContactMessageDto?> UpdateContactMessageStatusAsync(
        int messageId,
        UpdateContactMessageStatusDto updateContactMessageStatusDto);

    Task<ClientReviewDto> CreateClientReviewAsync(
        CreateClientReviewDto createClientReviewDto,
        ClaimsPrincipal user);

    Task<List<ClientReviewDto>> GetAllClientReviewsAsync();

    Task<List<ClientReviewDto>> GetPendingClientReviewsAsync();

    Task<List<ClientReviewDto>> GetApprovedClientReviewsAsync();

    Task<ClientReviewDto?> UpdateClientReviewApprovalAsync(
        int reviewId,
        UpdateClientReviewApprovalDto updateClientReviewApprovalDto);
}