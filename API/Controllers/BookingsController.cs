using API.DTOs;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class BookingsController(
    IBookingService bookingService
) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<BookingReviewDto>> CreatePendingBooking(
        CreateBookingDto createBookingDto)
    {
        try
        {
            var userId = User.GetUserId();

            var booking = await bookingService.CreatePendingBookingAsync(
                createBookingDto,
                userId);

            return Ok(booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{bookingId:int}/review")]
    public async Task<ActionResult<BookingReviewDto>> GetBookingReview(int bookingId)
    {
        var userId = User.GetUserId();

        var booking = await bookingService.GetBookingReviewAsync(bookingId, userId);

        if (booking == null)
        {
            return NotFound(new { message = "Booking not found." });
        }

        return Ok(booking);
    }

    [HttpGet("{bookingId:int}/confirmation")]
    public async Task<ActionResult<BookingReviewDto>> GetConfirmedBooking(int bookingId)
    {
        var userId = User.GetUserId();

        var booking = await bookingService.GetConfirmedBookingAsync(bookingId, userId);

        if (booking == null)
        {
            return NotFound(new { message = "Confirmed booking not found." });
        }

        return Ok(booking);
    }
}