using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ContactController(IContactService contactService) : BaseApiController
{
    // ===============================
    // Contact messages
    // Logged-in clients send and view their own messages.
    // Admin endpoints are prepared for the future dashboard.
    // ===============================

    [Authorize]
    [HttpPost("messages")]
    public async Task<ActionResult<ContactMessageDto>> CreateContactMessage(
        CreateContactMessageDto createContactMessageDto)
    {
        try
        {
            var message = await contactService.CreateContactMessageAsync(
                createContactMessageDto,
                User);

            return Ok(message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [Authorize]
    [HttpGet("messages/my-messages")]
    public async Task<ActionResult<List<ContactMessageDto>>> GetMyMessages()
    {
        try
        {
            var messages = await contactService.GetMyContactMessagesAsync(User);

            return Ok(messages);
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("messages")]
    public async Task<ActionResult<List<ContactMessageDto>>> GetAllMessages()
    {
        var messages = await contactService.GetAllContactMessagesAsync();

        return Ok(messages);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("messages/{id:int}/status")]
    public async Task<ActionResult<ContactMessageDto>> UpdateMessageStatus(
        int id,
        UpdateContactMessageStatusDto updateContactMessageStatusDto)
    {
        try
        {
            var message = await contactService.UpdateContactMessageStatusAsync(
                id,
                updateContactMessageStatusDto);

            if (message == null)
            {
                return NotFound(new
                {
                    message = "Contact message not found."
                });
            }

            return Ok(message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    // ===============================
    // Client reviews
    // Reviews are saved as pending until approved/featured by admin.
    // ===============================

    [Authorize]
    [HttpPost("reviews")]
    public async Task<ActionResult<ClientReviewDto>> CreateClientReview(
        [FromForm] CreateClientReviewDto createClientReviewDto)
    {
        try
        {
            var review = await contactService.CreateClientReviewAsync(
                createClientReviewDto,
                User);

            return Ok(review);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("reviews")]
    public async Task<ActionResult<List<ClientReviewDto>>> GetAllReviews()
    {
        var reviews = await contactService.GetAllClientReviewsAsync();

        return Ok(reviews);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("reviews/pending")]
    public async Task<ActionResult<List<ClientReviewDto>>> GetPendingReviews()
    {
        var reviews = await contactService.GetPendingClientReviewsAsync();

        return Ok(reviews);
    }

    [Authorize]
    [HttpGet("reviews/approved")]
    public async Task<ActionResult<List<ClientReviewDto>>> GetApprovedReviews()
    {
        var reviews = await contactService.GetApprovedClientReviewsAsync();

        return Ok(reviews);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("reviews/{id:int}/approval")]
    public async Task<ActionResult<ClientReviewDto>> UpdateReviewApproval(
        int id,
        UpdateClientReviewApprovalDto updateClientReviewApprovalDto)
    {
        var review = await contactService.UpdateClientReviewApprovalAsync(
            id,
            updateClientReviewApprovalDto);

        if (review == null)
        {
            return NotFound(new
            {
                message = "Client review not found."
            });
        }

        return Ok(review);
    }
}