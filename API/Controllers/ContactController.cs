using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ContactController(IContactService contactService) : BaseApiController
{
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
        var messages = await contactService.GetMyContactMessagesAsync(User);

        return Ok(messages);
    }

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
}